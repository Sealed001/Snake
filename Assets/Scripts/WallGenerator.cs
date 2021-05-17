using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public GameObject prefab;
    [Range(0, 20)] public int wallMinDistance = 4;

    private List<GameObject> _walls = new List<GameObject> { };
    private Game _game;

    void Start()
    {
        _game = GameObject.Find("Game Manager").GetComponent<Game>();
    }

    public List<Vector2Int> GetWallsPositions()
    {
        List<Vector2Int> walls = new List<Vector2Int>() { };
        foreach (GameObject wall in _walls)
        {
            walls.Add(Vector2Int.FloorToInt(wall.transform.position));
        }
        return walls;
    }

    public void GenerateWall()
    {
        List<Vector2Int> spawnPositions = new List<Vector2Int> { };

        for (int x = 0; x < (_game.worldSize.x * 2 + 1); x++)
        {
            for (int y = 0; y < (_game.worldSize.y * 2 + 1); y++)
            {
                if (!_game.world[x, y])
                {
                    if (DistanceFromPlayers(new Vector2Int(x, y)) > wallMinDistance)
                    {
                        spawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        if (spawnPositions.Count > 0)
        {
            AddWall(spawnPositions[Random.Range(0, spawnPositions.Count)]);
        }
    }

    private float DistanceFromPlayers(Vector2Int position)
    {
        float maxDist = 0f;

        foreach (GameObject player in _game.players)
        {
            foreach (GameObject segment in player.GetComponent<Player>().GetPlayerSegments())
            {
                float dist = Vector2Int.Distance(position - new Vector2Int(_game.worldSize.x, _game.worldSize.y), Vector2Int.FloorToInt(segment.transform.position));
                if (maxDist < dist)
                {
                    maxDist = dist;
                }
            }
        }

        return maxDist;
    }

    private void AddWall(Vector2Int position)
    {
        _game.world[position.x, position.y] = true;
        _walls.Add(Instantiate(prefab, new Vector3(position.x - _game.worldSize.x, position.y - _game.worldSize.y), Quaternion.identity, transform));
    }

    public void ClearWalls()
    {
        _walls = new List<GameObject> { };

        foreach (Transform wall in transform.GetComponentInChildren<Transform>())
        {
            Destroy(wall.gameObject);
        }
    }
}

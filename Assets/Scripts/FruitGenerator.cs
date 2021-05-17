using System.Collections.Generic;
using UnityEngine;

public class FruitGenerator : MonoBehaviour
{
    public GameObject prefab;
    [Range(0, 20)] public int fruitMinDistance = 6;

    private List<GameObject> _fruits = new List<GameObject> { };
    private Game _game;

    public void Start()
    {
        _game = GameObject.Find("Game Manager").GetComponent<Game>();
    }

    public List<Vector2Int> GetFruitsPositions()
    {
        List<Vector2Int> fruits = new List<Vector2Int>() { };
        foreach (GameObject fruit in _fruits)
        {
            fruits.Add(Vector2Int.FloorToInt(fruit.transform.position));
        }
        return fruits;
    }

    public void GenerateFruit()
    {
        List<Vector2Int> spawnPositions = new List<Vector2Int> { };

        for (int x = 0; x < (_game.worldSize.x * 2 + 1); x++)
        {
            for (int y = 0; y < (_game.worldSize.y * 2 + 1); y++)
            {
                if (!_game.world[x, y])
                {
                    if (DistanceFromPlayers(new Vector2Int(x, y)) > fruitMinDistance)
                    {
                        spawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        if (spawnPositions.Count > 0)
        {
            AddFruit(spawnPositions[Random.Range(0, spawnPositions.Count)]);
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

    private void AddFruit(Vector2Int position)
    {
        _game.world[position.x, position.y] = true;
        _fruits.Add(Instantiate(prefab, new Vector3(position.x - _game.worldSize.x, position.y - _game.worldSize.y), Quaternion.identity, transform));
    }

    public void RemoveFruit(Vector2Int position)
    {
        int fruitIndex = -1;
        for (int i = 0; i < _fruits.Count; i++)
        {
            if (Equals(_fruits[i].transform.position, new Vector3(position.x, position.y)))
            {
                fruitIndex = i;
            }
        }

        if (fruitIndex != -1)
        {
            _game.world[position.x + _game.worldSize.x, position.y + _game.worldSize.y] = false;

            GameObject fruit = _fruits[fruitIndex];
            _fruits.RemoveAt(fruitIndex);
            Destroy(fruit);
        }
    }

    public void ClearFruits()
    {
        _fruits = new List<GameObject> { };

        foreach (Transform fruit in transform.GetComponentInChildren<Transform>())
        {
            Destroy(fruit.gameObject);
        }
    }
}

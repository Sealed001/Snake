using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool isPaused = true;
    [Range(0f, 100f)] public float wallSpawningRate = 5f;
    [Range(0f, 100f)] public float fruitSpawningRate = 5f;

    private bool _isAbleToPause = false;
    public bool _isPartyLaunched = false;

    private float _timer;
    private float _wallTimer;
    private float _fruitTimer;

    [HideInInspector] public Vector2Int worldSize;
    [HideInInspector] public bool[,] world;
    [HideInInspector] public List<GameObject> players;

    private AudioSource _launchCountdownSource;
    private TimerUpdater _timerUpdater;
    private WallGenerator _wallGenerator;
    private FruitGenerator _fruitGenerator;

    public void Start()
    {
        _wallTimer = wallSpawningRate;
        _fruitTimer = fruitSpawningRate;

        _launchCountdownSource = gameObject.GetComponent<AudioSource>();
        _timerUpdater = GameObject.Find("Timer").GetComponent<TimerUpdater>();
        _wallGenerator = GameObject.Find("Walls").GetComponent<WallGenerator>();
        _fruitGenerator = GameObject.Find("Fruits").GetComponent<FruitGenerator>();

        Invoke("Test", 10f);
    }

    public void Test()
    {
        if (players.Count >= 2)
        {
            StartParty(new Vector2Int(8, 5), 60 * 10);
        }
        else
        {
            Invoke("Test", 30f);
        }
    }

    public void StartParty(Vector2Int size, float duration)
    {
        if (!_isPartyLaunched)
        {
            _isPartyLaunched = true;

            worldSize = size;
            world = new bool[worldSize.x * 2 + 1, worldSize.y * 2 + 1];
            for (int x = 0; x < world.GetLength(0); x++)
            {
                for (int y = 0; y < world.GetLength(1); y++)
                {
                    world[x, y] = false;
                }
            }

            _timer = duration;

            GameObject.Find("Clouds").GetComponent<CloudGenerator>().GenerateClouds();
           
            foreach (GameObject player in players)
            {
                player.GetComponent<Player>().Spawn();
            }

            _timerUpdater.UpdateTimer(_timer);

            StartLaunchCountdown();
        }
    }

    public void PauseParty()
    {
        if (_isAbleToPause)
        {
            isPaused = !isPaused;
        }
    }

    public void StopParty()
    {
        _isPartyLaunched = false;
        foreach (GameObject player in players)
        {
            player.GetComponent<Player>().DeSpawn();
        }

        GameObject.Find("Clouds").GetComponent<CloudGenerator>().ClearClouds();

        _isAbleToPause = false;
        isPaused = true;

        Invoke("Test", 30f);
    }

    public void StartLaunchCountdown()
    {
        _launchCountdownSource.Play(0);
        Invoke("EndLaunchCountdown", 5f);
    }

    public void EndLaunchCountdown()
    {
        isPaused = false;
        _isAbleToPause = true;
    }

    void Update()
    {
        if (_isAbleToPause)
        {
            if (_timer <= 0f)
            {
                StopParty();
            }
            else
            {
                _timer -= Time.deltaTime;
                _timerUpdater.UpdateTimer(_timer);
            }

            if (_wallTimer <= 0f)
            {
                _wallTimer += wallSpawningRate;
                _wallGenerator.GenerateWall();
            }
            else
            {
                _wallTimer -= Time.deltaTime;
            }

            if (_fruitTimer <= 0f)
            {
                _fruitTimer += fruitSpawningRate;
                _fruitGenerator.GenerateFruit();
            }
            else
            {
                _fruitTimer -= Time.deltaTime;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public enum PlayerOrientations
	{
		Right,
		Down,
		Left,
		Up
	};

	[Header("Customization")]
	public GameObject segment;
	public Vector3 bubbleOffset;

	[Header("Properties")]
	[HideInInspector] public int playerLength = 3;
	[HideInInspector] public int playerScore = 0;
	[Range(0f, 10f)] public float playerSpeed = 1f;
	public PlayerOrientations playerOrientation;

	private bool _isDead = false;
	private bool _hasSpawned = false;
	private List<GameObject> _playerSegments = new List<GameObject>();

	private float _timer = 0f;

	private Game _game;
	private FruitGenerator _fruitGenerator;
	private WallGenerator _wallGenerator;

	private int _playerIndex;
	public Color playerColor;

	void Start()
	{
		_game = GameObject.Find("Game Manager").GetComponent<Game>();
		_fruitGenerator = GameObject.Find("Fruits").GetComponent<FruitGenerator>();
		_wallGenerator = GameObject.Find("Walls").GetComponent<WallGenerator>();

		_game.players.Add(gameObject);
		_playerIndex = _game.players.IndexOf(gameObject);

		playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
	}

	void Update()
	{
		if (!_game.isPaused && !_isDead && _hasSpawned)
		{
			_timer += Time.deltaTime;
			if (_timer >= (1f / playerSpeed))
			{
				// Check if player is gonna die
				Vector3 playerNextPosition = GetNextPosition();
				bool isGonnaDie = false;

                foreach (GameObject player in _game.players) // Iterate over all players
                {
					Player playerComponent = player.GetComponent<Player>();

					if (player != gameObject && !playerComponent.IsDead()) // Make iteration over all players minus this player
                    {
						List<Vector3> playerPositions = new List<Vector3>();

						playerPositions.AddRange(playerComponent.GetPlayerSegmentsPositions().GetRange(1, playerComponent.GetPlayerSegmentsPositions().Count - 1));
						playerPositions.Add(playerComponent.GetNextPosition());

                        foreach (Vector3 playerPosition in playerPositions)
                        {
							if (Equals(playerNextPosition, playerPosition))
                            {
								isGonnaDie = true;
							}
                        }
					}
                }

				if (!isGonnaDie)
                {
					foreach (Vector2Int wallPosition in _wallGenerator.GetWallsPositions())
					{
						if (Equals(new Vector3(wallPosition.x, wallPosition.y), playerNextPosition))
						{
							isGonnaDie = true;
						}
					}
				}

				if (isGonnaDie)
				{
					Die();
				}
				else
                {
					bool isGonnaGrow = false;

					foreach (Vector2Int fruitPosition in _fruitGenerator.GetFruitsPositions())
					{
						if (Equals(new Vector3(fruitPosition.x, fruitPosition.y), playerNextPosition))
						{
							isGonnaGrow = true;
							_fruitGenerator.RemoveFruit(fruitPosition);
						}
					}

					if (isGonnaGrow)
					{
						EatFruit();
					}
				}

				UpdatePosition();
				_timer -= (1f / playerSpeed);
			}
		}
	}

	void UpdatePosition()
	{
		if (!_isDead)
        {
			_playerSegments.Insert(0, Instantiate(segment, GetNextPosition(), Quaternion.identity, transform));
			while (_playerSegments.Count > playerLength)
			{
				GameObject obj = _playerSegments[_playerSegments.Count - 1];
				_playerSegments.RemoveAt(_playerSegments.Count - 1);
				Destroy(obj);
			}
			transform.Find("Bubble").transform.position = _playerSegments[0].transform.position + bubbleOffset;
		}
	}

	#region Match Making

	public void Spawn()
	{
		playerLength = 2;
		playerScore = 0;

		_hasSpawned = true;

		Vector3 spawnPoint;
		if (_game.players.Count == 2)
		{
			switch (_playerIndex)
			{
				case 0:
					spawnPoint = new Vector3(-_game.worldSize.x + 1, 0f);
					playerOrientation = PlayerOrientations.Right;
					break;
				case 1:
					spawnPoint = new Vector3(_game.worldSize.x - 1, 0f);
					playerOrientation = PlayerOrientations.Left;
					break;
				default:
					spawnPoint = Vector3.zero;
					break;
			}
		}
		else
		{
			switch (_playerIndex)
			{
				case 0:
					spawnPoint = new Vector3(-_game.worldSize.x + 1, _game.worldSize.y - 1);
					playerOrientation = PlayerOrientations.Right;
					break;
				case 1:
					spawnPoint = new Vector3(_game.worldSize.x - 1, _game.worldSize.y - 1);
					playerOrientation = PlayerOrientations.Left;
					break;
				case 2:
					spawnPoint = new Vector3(-_game.worldSize.x + 1, -_game.worldSize.y + 1);
					playerOrientation = PlayerOrientations.Right;
					break;
				case 3:
					spawnPoint = new Vector3(_game.worldSize.x - 1, -_game.worldSize.y + 1);
					playerOrientation = PlayerOrientations.Left;
					break;
				default:
					spawnPoint = Vector3.zero;
					break;
			}
		}

		_playerSegments.Add(Instantiate(segment, spawnPoint, Quaternion.identity, transform));
		switch (playerOrientation)
		{
			case PlayerOrientations.Right:
				_playerSegments.Add(Instantiate(segment, spawnPoint + new Vector3(-1f, 0f, 0f), Quaternion.identity, transform));
				break;
			case PlayerOrientations.Down:
				_playerSegments.Add(Instantiate(segment, spawnPoint + new Vector3(0f, 1f, 0f), Quaternion.identity, transform));
				break;
			case PlayerOrientations.Left:
				_playerSegments.Add(Instantiate(segment, spawnPoint + new Vector3(1f, 0f, 0f), Quaternion.identity, transform));
				break;
			case PlayerOrientations.Up:
				_playerSegments.Add(Instantiate(segment, spawnPoint + new Vector3(0f, -1f, 0f), Quaternion.identity, transform));
				break;
		}
		transform.Find("Bubble").transform.position = _playerSegments[0].transform.position + bubbleOffset;
	}

	public void DeSpawn()
	{
		_hasSpawned = false;
	}

	#endregion

	#region Gameplay

	public void EatFruit()
	{
		playerScore++;
		playerLength++;
	}

	public void Die()
	{
		_isDead = true;
		playerLength = 0;
		foreach (GameObject playerSegment in _playerSegments)
		{
			Destroy(playerSegment);
		}
		_playerSegments = new List<GameObject>();
	}
	#endregion

	#region Methods
	public Vector3 GetNextPosition()
	{
		Vector2 worldSize = _game.worldSize;
		if (!_isDead)
        {
			switch (playerOrientation)
			{
				case PlayerOrientations.Right:
					if (_playerSegments[0].transform.position.x > (worldSize.x - 1f))
					{
						return new Vector3(-worldSize.x, _playerSegments[0].transform.position.y, 0f);
					}
					else
					{
						return _playerSegments[0].transform.position + new Vector3(1f, 0f, 0f);
					}
				case PlayerOrientations.Down:
					if (_playerSegments[0].transform.position.y < (-worldSize.y + 1f))
					{
						return new Vector3(_playerSegments[0].transform.position.x, worldSize.y, 0f);
					}
					else
					{
						return _playerSegments[0].transform.position + new Vector3(0f, -1f, 0f);
					}
				case PlayerOrientations.Left:
					if (_playerSegments[0].transform.position.x < (-worldSize.x + 1f))
					{
						return new Vector3(worldSize.x, _playerSegments[0].transform.position.y, 0f);
					}
					else
					{
						return _playerSegments[0].transform.position + new Vector3(-1f, 0f, 0f);
					}
				case PlayerOrientations.Up:
					if (_playerSegments[0].transform.position.y > (worldSize.y - 1f))
					{
						return new Vector3(_playerSegments[0].transform.position.x, -worldSize.y, 0f);
					}
					else
					{
						return _playerSegments[0].transform.position + new Vector3(0f, 1f, 0f);
					}
				default:
					return Vector3.zero;
			}
		}
		else
        {
			return Vector3.zero;
		}
	}

	public PlayerOrientations GetPastOrientation()
	{
		if (_hasSpawned)
		{
			Vector3 transformation = _playerSegments[0].transform.position - _playerSegments[1].transform.position;
			if (transformation.magnitude > 1)
			{
				transformation = transformation / transformation.magnitude;
				if (transformation.Equals(new Vector3(1f, 0f, 0f)))
				{
					return PlayerOrientations.Right;
				}
				else if (transformation.Equals(new Vector3(0f, -1f, 0f)))
				{
					return PlayerOrientations.Down;
				}
				else if (transformation.Equals(new Vector3(-1f, 0f, 0f)))
				{
					return PlayerOrientations.Left;
				}
				else if (transformation.Equals(new Vector3(0f, 1f, 0f)))
				{
					return PlayerOrientations.Up;
				}
				else
				{
					return PlayerOrientations.Right;
				}
			}
			else
			{
				if (transformation.Equals(new Vector3(1f, 0f, 0f)))
				{
					return PlayerOrientations.Left;
				}
				else if (transformation.Equals(new Vector3(0f, -1f, 0f)))
				{
					return PlayerOrientations.Up;
				}
				else if (transformation.Equals(new Vector3(-1f, 0f, 0f)))
				{
					return PlayerOrientations.Right;
				}
				else if (transformation.Equals(new Vector3(0f, 1f, 0f)))
				{
					return PlayerOrientations.Down;
				}
				else
				{
					return PlayerOrientations.Left;
				}
			}
		}
		else
		{
			return PlayerOrientations.Right;
		}
	}

	public List<GameObject> GetPlayerSegments()
	{
		return _playerSegments;
	}

	public List<Vector3> GetPlayerSegmentsPositions()
	{
		List<Vector3> playersSegmentsPositions = new List<Vector3>() { };

        foreach (GameObject playerSegment in _playerSegments)
        {
			playersSegmentsPositions.Add(playerSegment.transform.position);

		}

		return playersSegmentsPositions;
	}

	public bool IsDead()
    {
		return _isDead;
    }

	public bool HasSpawned()
    {
		return _hasSpawned;
    }

	#endregion

	#region Movements
	public void Move(PlayerOrientations orientation)
    {
		if (!_game.isPaused && !_isDead && _hasSpawned)
		{
			if (GetPastOrientation() != orientation)
            {
				playerOrientation = orientation;
            }
		}
	}
    #endregion
}
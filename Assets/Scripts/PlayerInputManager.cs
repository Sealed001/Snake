using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	[Header("Controls")]
	[Range(0f, 1f)]
	public float joystickSensitivity = 0.65f;

	[Header("Objects")]
	public SpriteRenderer bubble;
	public SpriteRenderer buttonUpRaised;
	public SpriteRenderer buttonUpPushed;
	public SpriteRenderer buttonLeftRaised;
	public SpriteRenderer buttonLeftPushed;
	public SpriteRenderer buttonRightRaised;
	public SpriteRenderer buttonRightPushed;
	public SpriteRenderer buttonDownRaised;
	public SpriteRenderer buttonDownPushed;

	private Player _player;
	private CameraControl _camera;
	private Game _game;

	private Vector2 _joystickMovement = new Vector2(0, 0);
	private Vector2 _joystickCameraMovement = new Vector2(0, 0);
	private float _alpha = 0f;

	private void Start()
	{
		_player = transform.GetComponent<Player>();
		_camera = GameObject.Find("Camera").GetComponent<CameraControl>();
		_game = GameObject.Find("Game Manager").GetComponent<Game>();
	}

	private void FixedUpdate()
	{
		#region Movements

		if (_joystickMovement.magnitude >= joystickSensitivity && !_player.IsDead() && _player.HasSpawned() && !_game.isPaused)
		{
			_alpha = 1f;
			if (_joystickMovement.normalized.y >= .7071f)
			{
				_player.Move(Player.PlayerOrientations.Up);
			}
			else if (_joystickMovement.normalized.y <= -0.7071f)
			{
				_player.Move(Player.PlayerOrientations.Down);
			}
			else
			{
				if (_joystickMovement.normalized.x > 0f)
				{
					_player.Move(Player.PlayerOrientations.Right);
				}
				else if (_joystickMovement.normalized.x < 0f)
				{
					_player.Move(Player.PlayerOrientations.Left);
				}
			}
		}
		else
		{
			_alpha -= .018f;
		}

		_alpha = Mathf.Clamp(_alpha, 0f, 1f);

		#endregion
	}

	private void Update()
	{
		if (_joystickCameraMovement.magnitude >= joystickSensitivity)
		{
			_camera.angle += _joystickCameraMovement * Time.deltaTime * _camera.cameraMovementSpeed;
		}
		
		#region Movements UI
		
		Color baseColor = new Color(1f, 1f, 1f, _alpha);

		bubble.color = baseColor;

		buttonUpPushed.color = baseColor;
		buttonLeftPushed.color = baseColor;
		buttonRightPushed.color = baseColor;
		buttonDownPushed.color = baseColor;
		buttonUpRaised.color = baseColor;
		buttonLeftRaised.color = baseColor;
		buttonRightRaised.color = baseColor;
		buttonDownRaised.color = baseColor;

		if (!_player.IsDead() && !_player.IsDead() && _player.HasSpawned() && !_game.isPaused)
		{
			switch (_player.GetPastOrientation())
			{
				case Player.PlayerOrientations.Right:
					buttonRightPushed.color = new Color(1f, 1f, 1f, 0f);
					buttonRightRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Left:
					buttonLeftPushed.color = new Color(1f, 1f, 1f, 0f);
					buttonLeftRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Up:
					buttonUpPushed.color = new Color(1f, 1f, 1f, 0f);
					buttonUpRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Down:
					buttonDownPushed.color = new Color(1f, 1f, 1f, 0f);
					buttonDownRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
			}

			switch (_player.playerOrientation)
			{
				case Player.PlayerOrientations.Right:
					buttonRightRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Left:
					buttonLeftRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Down:
					buttonDownRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
				case Player.PlayerOrientations.Up:
					buttonUpRaised.color = new Color(1f, 1f, 1f, 0f);
					break;
			}
		}

		#endregion
	}
	public void OnMove(InputValue v)
	{
		_joystickMovement = v.Get<Vector2>();
	}
	public void OnMoveCamera(InputValue v)
	{
		_joystickCameraMovement = v.Get<Vector2>();
	}
}

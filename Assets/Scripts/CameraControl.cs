using UnityEngine;
public class CameraControl : MonoBehaviour
{
	private Game _game;
	// public float aspectRatio = 0f;
	public float distanceFromScene = 5f;
	public float cameraMovementSpeed = 5f;
	public float cameraMaxMagnitude = 30f;

	[HideInInspector]
	public Vector2 angle = Vector2.zero;
	void Start()
	{
		_game = GameObject.Find("Game Manager").GetComponent<Game>();
	}
    private void Update()
	{
		angle = Vector2.ClampMagnitude(angle, cameraMaxMagnitude);
		transform.rotation = Quaternion.Euler(angle.y, -angle.x, 0f);

		Vector3[] gameZonePoints = new Vector3[4];
		int index = 0;
		for (int x = -1; x <= 1; x += 2)
		{
			for (int y = -1; y <= 1; y += 2)
			{
				gameZonePoints[index] = new Vector3((_game.worldSize.x + 1) * x, (_game.worldSize.y + 1) * y, -1f);
				index++;
			}
		}

		Vector3[] gameZoneCenteredPoints = new Vector3[4];
		if (transform.rotation.x > 0)
		{
			if (transform.rotation.y > 0)
			{
				// 1
				gameZoneCenteredPoints[1] = gameZonePoints[1];

				float angle = Vector3.Angle(Vector3.right, transform.rotation * Vector3.right);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[3]);
				gameZoneCenteredPoints[3] = gameZonePoints[3] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.down, transform.rotation * Vector3.down);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[0]);
				gameZoneCenteredPoints[0] = gameZonePoints[0] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[2] = gameZoneCenteredPoints[0] + (gameZoneCenteredPoints[3] - gameZoneCenteredPoints[1]);
			}
			else
			{
				// 3
				gameZoneCenteredPoints[3] = gameZonePoints[3];

				float angle = Vector3.Angle(Vector3.left, transform.rotation * Vector3.left);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[3]);
				gameZoneCenteredPoints[1] = gameZonePoints[1] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.down, transform.rotation * Vector3.down);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[2], gameZonePoints[3]);
				gameZoneCenteredPoints[2] = gameZonePoints[2] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[0] = gameZoneCenteredPoints[2] + (gameZoneCenteredPoints[1] - gameZoneCenteredPoints[3]);
			}
		}
		else
		{
			if (transform.rotation.y > 0)
			{
				// 0
				gameZoneCenteredPoints[0] = gameZonePoints[0];

				float angle = Vector3.Angle(Vector3.right, transform.rotation * Vector3.right);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[0], gameZonePoints[2]);
				gameZoneCenteredPoints[2] = gameZonePoints[2] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.up, transform.rotation * Vector3.up);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[0]);
				gameZoneCenteredPoints[1] = gameZonePoints[1] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[3] = gameZoneCenteredPoints[1] + (gameZoneCenteredPoints[2] - gameZoneCenteredPoints[0]);
			}
			else
			{
				// 2
				gameZoneCenteredPoints[2] = gameZonePoints[2];

				float angle = Vector3.Angle(Vector3.left, transform.rotation * Vector3.left);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[0], gameZonePoints[2]);
				gameZoneCenteredPoints[0] = gameZonePoints[0] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.up, transform.rotation * Vector3.up);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[3], gameZonePoints[2]);
				gameZoneCenteredPoints[3] = gameZonePoints[3] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[1] = gameZoneCenteredPoints[3] + (gameZoneCenteredPoints[0] - gameZoneCenteredPoints[2]);
			}
		}

		Vector3[] gameZoneCenteredDistancedPoints = new Vector3[4];
		for (int i = 0; i < 4; i++)
		{
			gameZoneCenteredDistancedPoints[i] = gameZoneCenteredPoints[i] + transform.rotation * Vector3.back * distanceFromScene;
		}

		Vector3 cameraPosition = gameZoneCenteredDistancedPoints[1] + (gameZoneCenteredPoints[2] - gameZoneCenteredPoints[1]) / 2;
		transform.position = cameraPosition;
	}
	private void OnDrawGizmos()
	{
		_game = GameObject.Find("Game Manager").GetComponent<Game>();

		Gizmos.color = Color.yellow;
		Vector3[] gameZonePoints = new Vector3[4];
		int index = 0; 
		for (int x = -1; x <= 1; x+=2)
		{
			for (int y = -1; y <= 1; y+=2)
			{
				gameZonePoints[index] = new Vector3((_game.worldSize.x + 1) * x, (_game.worldSize.y + 1) * y, -1f);
				index++;
			}
		}

		Vector3[] gameZoneCenteredPoints = new Vector3[4];
		if (transform.rotation.x > 0)
        {
			if (transform.rotation.y > 0)
			{
				// 1
				gameZoneCenteredPoints[1] = gameZonePoints[1];

				float angle = Vector3.Angle(Vector3.right, transform.rotation * Vector3.right);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[3]);
				gameZoneCenteredPoints[3] = gameZonePoints[3] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.down, transform.rotation * Vector3.down);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[0]);
				gameZoneCenteredPoints[0] = gameZonePoints[0] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[2] = gameZoneCenteredPoints[0] + (gameZoneCenteredPoints[3] - gameZoneCenteredPoints[1]);
			}
			else
			{
				// 3
				gameZoneCenteredPoints[3] = gameZonePoints[3];

				float angle = Vector3.Angle(Vector3.left, transform.rotation * Vector3.left);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[3]);
				gameZoneCenteredPoints[1] = gameZonePoints[1] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.down, transform.rotation * Vector3.down);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[2], gameZonePoints[3]);
				gameZoneCenteredPoints[2] = gameZonePoints[2] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[0] = gameZoneCenteredPoints[2] + (gameZoneCenteredPoints[1] - gameZoneCenteredPoints[3]);
			}
		}
		else
        {
			if (transform.rotation.y > 0)
			{
				// 0
				gameZoneCenteredPoints[0] = gameZonePoints[0];

				float angle = Vector3.Angle(Vector3.right, transform.rotation * Vector3.right);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[0], gameZonePoints[2]);
				gameZoneCenteredPoints[2] = gameZonePoints[2] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.up, transform.rotation * Vector3.up);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[1], gameZonePoints[0]);
				gameZoneCenteredPoints[1] = gameZonePoints[1] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[3] = gameZoneCenteredPoints[1] + (gameZoneCenteredPoints[2] - gameZoneCenteredPoints[0]);
			}
			else
			{
				// 2
				gameZoneCenteredPoints[2] = gameZonePoints[2];

				float angle = Vector3.Angle(Vector3.left, transform.rotation * Vector3.left);
				float a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[0], gameZonePoints[2]);
				gameZoneCenteredPoints[0] = gameZonePoints[0] + transform.rotation * Vector3.back * a;

				angle = Vector3.Angle(Vector3.up, transform.rotation * Vector3.up);
				a = Mathf.Asin(angle / 360 * Mathf.PI * 2) * Vector3.Distance(gameZonePoints[3], gameZonePoints[2]);
				gameZoneCenteredPoints[3] = gameZonePoints[3] + transform.rotation * Vector3.back * a;

				gameZoneCenteredPoints[1] = gameZoneCenteredPoints[3] + (gameZoneCenteredPoints[0] - gameZoneCenteredPoints[2]);
			}
		}

		Gizmos.DrawLine(gameZonePoints[0], gameZonePoints[2]);
		Gizmos.DrawLine(gameZonePoints[0], gameZonePoints[1]);
		Gizmos.DrawLine(gameZonePoints[3], gameZonePoints[2]);
		Gizmos.DrawLine(gameZonePoints[3], gameZonePoints[1]);

		Gizmos.DrawLine(gameZoneCenteredPoints[1], gameZoneCenteredPoints[3]);
		Gizmos.DrawLine(gameZoneCenteredPoints[0], gameZoneCenteredPoints[2]);
		Gizmos.DrawLine(gameZoneCenteredPoints[0], gameZoneCenteredPoints[1]);
		Gizmos.DrawLine(gameZoneCenteredPoints[2], gameZoneCenteredPoints[3]);

		Vector3[] gameZoneCenteredDistancedPoints = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
			Gizmos.DrawLine(gameZoneCenteredPoints[i], gameZonePoints[i]);
			gameZoneCenteredDistancedPoints[i] = gameZoneCenteredPoints[i] + transform.rotation * Vector3.back * distanceFromScene;
			Gizmos.DrawLine(gameZoneCenteredDistancedPoints[i], gameZoneCenteredPoints[i]);
		}

		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[0], gameZoneCenteredDistancedPoints[2]);
		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[0], gameZoneCenteredDistancedPoints[1]);
		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[3], gameZoneCenteredDistancedPoints[2]);
		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[3], gameZoneCenteredDistancedPoints[1]);

		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[1], gameZoneCenteredDistancedPoints[2]);
		Gizmos.DrawLine(gameZoneCenteredDistancedPoints[3], gameZoneCenteredDistancedPoints[0]);

		Vector3 cameraPosition = gameZoneCenteredDistancedPoints[1] + (gameZoneCenteredPoints[2] - gameZoneCenteredPoints[1]) / 2;
		Gizmos.DrawWireSphere(cameraPosition, 0.5f);
		transform.position = cameraPosition;
	}
}

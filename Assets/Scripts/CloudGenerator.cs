using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
	public GameObject prefab;
	[Range(1, 10)]
	public int borderLength = 4;
	public void GenerateClouds()
    {
		Vector2 worldSize = GameObject.Find("Game Manager").GetComponent<Game>().worldSize;
		for (int l = 1; l <= borderLength; l++)
		{
			// Left side
			for (int y = 0; y < worldSize.y * 2 + l * 2; y++)
			{
				Instantiate(prefab, new Vector3(-worldSize.x - l, y - worldSize.y - l, 0), Quaternion.identity, transform);
			}
			// Right side
			for (int y = 0; y < worldSize.y * 2 + l * 2; y++)
			{
				Instantiate(prefab, new Vector3(worldSize.x + l, y - worldSize.y - l + 0.5f, 0), Quaternion.identity, transform);
			}
			// Bottom side
			for (int x = 0; x < worldSize.x * 2 + l * 2; x++)
			{
				Instantiate(prefab, new Vector3(x - worldSize.x - l, -worldSize.y - l, 0), Quaternion.identity, transform);
			}
			// Top side
			for (int x = 0; x < worldSize.x * 2 + l * 2; x++)
			{
				Instantiate(prefab, new Vector3(x - worldSize.x - l, worldSize.y + l, 0), Quaternion.identity, transform);
			}
		}
	}

	public void ClearClouds()
    {
        foreach (Transform cloud in transform.GetComponentInChildren<Transform>())
        {
			Destroy(cloud.gameObject);
        }
    }

	void OnDrawGizmos()
	{
		Vector2 worldSize = GameObject.Find("Game Manager").GetComponent<Game>().worldSize;

		Vector3[] vertices = new Vector3[16];

		// Points
		int index = 0;
		// Outside points
		for (int z = -1; z <= 1; z+=2)
		{
			for (int x = -1; x <= 1; x += 2)
			{
				for (int y = -1; y <= 1; y += 2)
				{
					vertices[index] = new Vector3((worldSize.x + borderLength) * x, (worldSize.y + borderLength) * y, z);
					index++;
				}
			}
		}

		// Inside points
		for (int z = -1; z <= 1; z += 2)
		{
			for (int x = -1; x <= 1; x += 2)
			{
				for (int y = -1; y <= 1; y += 2)
				{
					vertices[index] = new Vector3(worldSize.x * x, worldSize.y * y, z);
					index++;
				}
			}
		}

		Gizmos.color = Color.white;

		Gizmos.DrawLine(vertices[0], vertices[1]);
		Gizmos.DrawLine(vertices[0], vertices[2]);
		Gizmos.DrawLine(vertices[1], vertices[3]);
		Gizmos.DrawLine(vertices[2], vertices[3]);
		Gizmos.DrawLine(vertices[8], vertices[9]);
		Gizmos.DrawLine(vertices[8], vertices[10]);
		Gizmos.DrawLine(vertices[9], vertices[11]);
		Gizmos.DrawLine(vertices[10], vertices[11]);
		Gizmos.DrawLine(vertices[4], vertices[5]);
		Gizmos.DrawLine(vertices[4], vertices[6]);
		Gizmos.DrawLine(vertices[5], vertices[7]);
		Gizmos.DrawLine(vertices[6], vertices[7]);
		Gizmos.DrawLine(vertices[12], vertices[13]);
		Gizmos.DrawLine(vertices[12], vertices[14]);
		Gizmos.DrawLine(vertices[13], vertices[15]);
		Gizmos.DrawLine(vertices[14], vertices[15]);
		Gizmos.DrawLine(vertices[0], vertices[4]);
		Gizmos.DrawLine(vertices[1], vertices[5]);
		Gizmos.DrawLine(vertices[3], vertices[7]);
		Gizmos.DrawLine(vertices[2], vertices[6]);
		Gizmos.DrawLine(vertices[8], vertices[12]);
		Gizmos.DrawLine(vertices[9], vertices[13]);
		Gizmos.DrawLine(vertices[11], vertices[15]);
		Gizmos.DrawLine(vertices[10], vertices[14]);
		Gizmos.DrawLine(vertices[0], vertices[8]);
		Gizmos.DrawLine(vertices[1], vertices[9]);
		Gizmos.DrawLine(vertices[3], vertices[11]);
		Gizmos.DrawLine(vertices[2], vertices[10]);
		Gizmos.DrawLine(vertices[12], vertices[4]);
		Gizmos.DrawLine(vertices[13], vertices[5]);
		Gizmos.DrawLine(vertices[14], vertices[6]);
		Gizmos.DrawLine(vertices[15], vertices[7]);
	}
}

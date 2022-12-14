using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityPlatform : MonoBehaviour
{
	public float yAmplitude;

	private GameObject prefab;
	private GameObject topPrefab;
	private GameObject bottomPrefab;
	private GameObject middle; // middle = 'main' in some sort
	private GameObject temp; // newly spawned platform
	private string side; // the platforms side (either Left or Right)

	private void Start()
	{
		prefab = gameObject;
		Vector3 platformPos = gameObject.transform.position;
		switch (platformPos.x)
		{
			case > 0:
				side = "Right";
				break;

			case < 0:
				side = "Left";
				break;
		}
		if (platformPos.y == 0) // if platform is the one in the middle
		{
			middle = gameObject;
			Vector3 prefabPos = new Vector3(platformPos.x, platformPos.y + yAmplitude/2, platformPos.z);

			topPrefab = Instantiate(prefab, prefabPos, Quaternion.identity);
			topPrefab.name = prefab.name; // renaming them for clarity
			topPrefab.transform.parent = GameObject.Find(side).transform;

			prefabPos.y = -yAmplitude/2;
			bottomPrefab = Instantiate(prefab, prefabPos, Quaternion.identity);
			bottomPrefab.name = prefab.name;
            bottomPrefab.transform.parent = GameObject.Find(side).transform;
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (gameObject == middle) // if we are the 'main' platform
		{
			if (collision.CompareTag("Wall")) // if we collided with the wall
			{
				// calculates where to spawn platform
				Vector3 pos = gameObject.transform.position;
				pos.y += pos.y > 0 ? -yAmplitude : yAmplitude;
				temp = Instantiate(prefab, pos, Quaternion.identity); // spawn new platform below/above
				temp.name = prefab.name;
				temp.transform.parent = GameObject.Find(side).transform;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (gameObject == middle) // checks if we're the 'main' platform
		{
			if (collision.name == "WallTop") // if we collided with the wall on the top
			{
				if (gameObject.transform.position.y < collision.transform.position.y) // if platform stayed in playable area
				{
					Destroy(temp);
				}
				else // if platform left playable area
				{
					Destroy(topPrefab);

					topPrefab = middle;
					middle = bottomPrefab;
					bottomPrefab = temp;

					Object[] objects = FindObjectsOfType<InfinityPlatform>();
					foreach (InfinityPlatform pc in objects)
					{
						// updates roles/positions for all platforms
						if (pc.side == side)
							pc.UpdatePositions(topPrefab, middle, bottomPrefab);
					}
				}
			}
			else if (collision.name == "WallBottom")
			{
				if (gameObject.transform.position.y > collision.transform.position.y) // if platform stayed in playable area
				{
					Destroy(temp);
				}
				else // if platform left playable area
				{
					Destroy(bottomPrefab);

					bottomPrefab = middle;
					middle = topPrefab;
					topPrefab = temp;

					Object[] objects = FindObjectsOfType<InfinityPlatform>();
					foreach (InfinityPlatform pc in objects)
					{
						// updates roles/positions for all platforms
						if (pc.side == side)
							pc.UpdatePositions(topPrefab, middle, bottomPrefab);
					}
				}
			}
		}
	}

	// updates roles/positions for all platforms
	private void UpdatePositions(GameObject topPlatform, GameObject middlePlatform, GameObject bottomPlatform)
	{
		topPrefab = topPlatform;
		middle = middlePlatform;
		bottomPrefab = bottomPlatform;
	}
}
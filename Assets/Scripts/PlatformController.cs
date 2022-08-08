using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
	private float speed;
	private string side; // the platforms side (either Left or Right)

	// Start is called before the first frame update
	private void Start()
	{
		speed = 12;
		switch (gameObject.transform.position.x) // finds the platforms side
		{
			case < 0:
				side = "Left";
				break;

			case > 0:
				side = "Right";
				break;
		}
	}

	// Update is called once per frame
	private void Update()
	{
		// controls all platforms on either side
		transform.Translate(new Vector3(0, Input.GetAxis($"Vertical{side}") * speed * Time.deltaTime, 0));
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
	private GameObject prefab;
	private Rigidbody2D rigidbody2;
	private TrailRenderer trail;

	private void Start()
	{
		prefab = gameObject;
		trail = gameObject.GetComponent<TrailRenderer>();
		StartCoroutine(Waiter());
		gameObject.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "WallLeft" || collision.name == "WallRight") // if ball hits the goal
		{
			// disables trail so it doesn't spazm out
			trail.time = 0;
			// calls pointmanager to add points on score
			PointManager pm = FindObjectOfType<PointManager>();
			pm.OnPlayerScore(collision.transform.position.x > 0 ? 0 : 1);
			// reset position and velocity
			transform.position = Vector2.zero;
			rigidbody2.velocity = Vector2.zero;
			// if gameobject is active, continue
			if (gameObject.activeSelf)
				StartCoroutine(Waiter());
		}
		else
		{
			// calculates and applies reflection
			Vector2 normalPos = collision.gameObject.transform.position.normalized;
			Vector2 vel = rigidbody2.velocity;
			Vector2 reflection = Vector2.Reflect(vel, normalPos);
			reflection.x = reflection.x > 0 ? 8 : -8;
			rigidbody2.velocity = reflection;
		}
	}

	private IEnumerator Waiter()
	{
		yield return new WaitForSeconds(1);

		trail.time = 0.1F;

		rigidbody2 = gameObject.GetComponent<Rigidbody2D>();

		// random Y value on start
		float randomForce = Random.Range(-2F, 2F);
		Vector2 force = new Vector2(8, randomForce);
		// random direction on start
		int randomSide = Random.Range(0, 2);
		force.x = randomSide == 0 ? -force.x : force.x;
		rigidbody2.velocity = force;
	}

	public void StopBall()
	{
		gameObject.SetActive(false);
	}
}
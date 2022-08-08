using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointManager : MonoBehaviour
{
	private int[] points; // index 0 - left; index 1 - right

	public GameObject leftPointsGameObject;
	public GameObject rightPointsGameObject;
	public GameObject winnerScreen;

	private TMPro.TextMeshProUGUI[] pointTexts;
	private TMPro.TextMeshProUGUI winnerText;

	// Start is called before the first frame update
	private void Start()
	{
		// sets variables
		points = new int[2] { 0, 0 };
		pointTexts = new TMPro.TextMeshProUGUI[2] { leftPointsGameObject.GetComponent<TMPro.TextMeshProUGUI>(),
			rightPointsGameObject.GetComponent<TMPro.TextMeshProUGUI>() };
		winnerText = winnerScreen.transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
	}

	public void OnPlayerScore(int playerScored) // playerScored = 0 - Left; 1 - Right
	{
		points[playerScored]++;
		pointTexts[playerScored].text = points[playerScored].ToString();

		if (points[0] == 3 || points[1] == 3) // if any player scores x points - wins
		{
			// configure winner text screen
			winnerText.text = (points[0] > points[1] ? "Left " : "Right ") + "player won!";
			winnerScreen.SetActive(true);
			// stop the ball from moving and creating errors
			BallManager bm = FindObjectOfType<BallManager>();
			bm.StopBall();

			StartCoroutine(Waiter()); // loads main screen after 3 seconds
		}
	}

	private IEnumerator Waiter()
	{
		yield return new WaitForSeconds(3);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // loads back into main menu
	}
}
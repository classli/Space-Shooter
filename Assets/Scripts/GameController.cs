using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float waveWait;
	public float startWait;

	public Text scoreText;
	private int score;
	public GameObject restartButton;

//	public GUIText restartText;
	public Text gameOverText;
	private bool gameOver;
	private bool restart;

	void Start () {
		score = 0;
		gameOver = false;
		restart = false;
		restartButton.SetActive (false);
		gameOverText.text = "";
		UpdateScore ();
		StartCoroutine(SpawnWaves ());
	}

//	void Update(){
//		if (restart) {
//			if (Input.GetKey (KeyCode.R)) {
//				SceneManager.LoadScene("Main", LoadSceneMode.Single);
//			}
//		}
//	}
	
	IEnumerator SpawnWaves (){
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++){
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation); 
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			if (gameOver) {
				restartButton.SetActive (true);
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore(){
		scoreText.text = "Score: " + score;
	}

	public void GameOver(){
		gameOverText.text = "Game Over";
		gameOver = true;
	}

	public void RestartGame() {
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

	public GameObject attractionObj;
	public Transform character;
	int totalAttractedObjs = 200;
	List<GameObject> allAttrObjs = new List<GameObject> ();

	public GameObject enemy;
	public EnemyType[] enemyTypes;
	List<GameObject> allEnemies = new List<GameObject> ();

	public GameObject[] obstacles;

	public ColorPalettes[] colorPalettes;
	ColorPalettes curPalette;

	public LevelSetting[] levels = new LevelSetting[30];

	LevelSetting curLevel;
	public int curLvlIdx = 0;

	public GameObject nextBtn;
	public GameObject resetBtn;
	public GameObject firstLevelBtn;
	// Use this for initialization
	void Awake () {

		Time.timeScale = 1f;

		if (PlayerPrefs.HasKey("CurrentLevel"))
			curLvlIdx = PlayerPrefs.GetInt ("CurrentLevel");
		
		curLevel = levels[curLvlIdx];

		curPalette = colorPalettes[Random.Range(0, colorPalettes.Length)];

		Camera.main.backgroundColor = curPalette.levelColors [0];

		gameObject.GetComponent<Walls> ().CreateWalls(curLevel.levelSize, curPalette.levelColors [1]);

		character.GetComponentInChildren<Shoot>().playerColor = curPalette.levelColors [2];
		SummonAttrObjs ();
		SummonEnemies ();
		SummonObstacles ();

		StartCoroutine (BringNewAttrs ());
	}
		

	void SummonAttrObjs(){
		for (int i = 0; i < totalAttractedObjs; i++) {

			GameObject ao = Instantiate (attractionObj) as GameObject;
			ao.GetComponent<SpriteRenderer> ().color = curPalette.levelColors [4];
			ao.SetActive (false);
			allAttrObjs.Add (ao);
		}
		Destroy (attractionObj);

		for (int i = 0; i < curLevel.startAttrObjs; i++) {
			ActivateAttrObj(RandomZeroExclud(1f), new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f,0.5f)));
		}

	}

	void SummonEnemies(){
		
		for (int e = 0; e < enemyTypes.Length; e++) {
			for (int en = 0; en < curLevel.numOfEnemies [e]; en++) {
				GameObject enm = Instantiate (enemy) as GameObject;
				enm.transform.localScale = new Vector2 (enemyTypes [e].size, enemyTypes [e].size);

				EnemyHealth enHealth = enm.GetComponent<EnemyHealth> ();
				enHealth.totalHealth = enemyTypes [e].totalHealth;
				enHealth.levelScr = this;
				enHealth.enemyColor = curPalette.levelColors [3];

				AI enemyAI = enm.GetComponent<AI> ();
				enemyAI.huntThreshold = enemyTypes [e].huntThreshHold;
				enemyAI.huntChance = enemyTypes [e].huntChance;
				enemyAI.huntForceMult = enemyTypes [e].huntForceMult + (enemyTypes [e].huntForceMult * 0.1f * (curLvlIdx + 1));
				enemyAI.character = character;

				if (enemyTypes [e].shield != null) {
					GameObject shield = Instantiate (enemyTypes [e].shield) as GameObject;
					shield.transform.parent = enm.transform;
				}

				Rigidbody2D enRgd = enm.GetComponent<Rigidbody2D> ();
				enRgd.mass = enemyTypes [e].mass;
				enRgd.drag = enemyTypes [e].drag;
				
				allEnemies.Add (enm);

			}
		}

		for (int p = 0; p < allEnemies.Count; p++) {
			allEnemies [p].transform.position = curLevel.enemyPositions [p];
		}
	}

	void SummonObstacles(){

		List<GameObject> lObstacles = new List<GameObject> ();
		for (int i = 0; i < obstacles.Length; i++) {
			for(int j=0; j<curLevel.numOfObstacles[i]; j++){
				GameObject obst = Instantiate (obstacles [i]) as GameObject;
				lObstacles.Add (obst);
			}
		}

		for (int p = 0; p < lObstacles.Count; p++) {
			lObstacles [p].transform.position = curLevel.obstaclePositions [p];
			lObstacles [p].GetComponent<SpriteRenderer> ().color = curPalette.levelColors [1];
			lObstacles [p].transform.eulerAngles = new Vector3(0,0, Random.Range(0f, 360f));
		}
			
	}

	public void ActivateAttrObj (Vector2 position, Vector2 force){

		foreach (GameObject ao in allAttrObjs) {

			if (!ao.activeSelf) {
				ao.SetActive (true);
				ao.transform.position = position;

				ao.GetComponent<Rigidbody2D> ().AddForce (force, ForceMode2D.Impulse);
				break;
			}

		}
	}
	
	private Vector2 RandomZeroExclud(float offset){

		Vector2 rdmVec = new Vector2 ();

		int rdmSideX = Random.Range (0, 2);
		if (rdmSideX == 0)
			rdmVec.x = Random.Range (-curLevel.levelSize.x+offset, -offset);
		else
			rdmVec.x = Random.Range (offset, curLevel.levelSize.x-offset);

		int rdmSideY = Random.Range (0, 2);
		if (rdmSideY == 0)
			rdmVec.y = Random.Range (-curLevel.levelSize.y+offset, -offset);
		else
			rdmVec.y = Random.Range (offset, curLevel.levelSize.y-offset);

		return rdmVec;
	
	}

	IEnumerator BringNewAttrs(){

		while (allEnemies.Count > 0 && character.gameObject.activeSelf){

			yield return new WaitForSeconds (Random.Range(5f, 10f));

			for (int i = 0; i < Random.Range(3, 10); i++) {
				ActivateAttrObj(RandomZeroExclud(1f), new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f,0.5f)));
			}
				
		}
			
	}

	public void HeroKilled(){
		resetBtn.transform.localPosition = new Vector2(0f, resetBtn.transform.localPosition.y);
		firstLevelBtn.SetActive (true);
		resetBtn.SetActive (true);
	}

	public void EnemyKilled(GameObject enemy){

		allEnemies.Remove (enemy);
		if (allEnemies.Count == 0) {
			nextBtn.SetActive (true);
			resetBtn.SetActive (true);
			firstLevelBtn.SetActive (true);
			character.GetComponent<PlayerControl> ().enabled = false;
			Time.timeScale = 0.5f;
		}
	}
		
//	void Update(){
//
//		if (Input.GetKeyDown(KeyCode.F5))
//			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//	}

	public void OnGuiButtonClick(bool next){

		if (next) {
			curLvlIdx++;
			PlayerPrefs.SetInt ("CurrentLevel", curLvlIdx);
		}

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void GoToFirstLevel(){
		PlayerPrefs.SetInt ("CurrentLevel", 0);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
		
}

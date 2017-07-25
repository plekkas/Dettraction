using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot : MonoBehaviour {

	List<AttractedObj> availableBullets = new List<AttractedObj>();
	SpriteRenderer nextBullerRnd;
	AttractedObj nextBulletScr;

	public Transform canon;

	public Color playerColor = Color.cyan;
	public SpriteRenderer sprRnd;

	void Start(){
		sprRnd.color = playerColor;
	}

	public void IsAttracted(AttractedObj att){
		if (!availableBullets.Contains (att)) {
			availableBullets.Add (att);
		}
	}

	public void IsOut(AttractedObj att){

		if (availableBullets.Contains(att)){
			availableBullets.Remove (att);
		}

	}

	float lastShotStamp = 0.0f;
	void Update(){
		//Debug.Log (availableBullets.Count);
		if ((Input.GetButtonDown ("Fire1") || Input.GetAxis("TriggerR") > 0.5f)  && Time.time - lastShotStamp > 0.15f && availableBullets.Count > 0 && Time.timeScale == 1f) {

			AttractedObj nextBulletScr = availableBullets [0];

			lastShotStamp = Time.time;
			ShootAttr (nextBulletScr);

		} else if ((Input.GetButtonDown ("Fire2") || Input.GetAxis("TriggerL") > 0.5f) && Time.time - lastShotStamp > 0.15f && availableBullets.Count > 0) {

			lastShotStamp = Time.time;

			List<AttractedObj> tempB = new List<AttractedObj> (availableBullets);
			foreach (AttractedObj ao in tempB) 
				ShootAttr (ao);
		}
	}

	void ShootAttr(AttractedObj nb){
		
		nb.GotShot (canon.up);
		availableBullets.Remove (nb);
	}
}

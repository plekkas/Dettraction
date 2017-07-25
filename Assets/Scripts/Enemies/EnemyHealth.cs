using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int totalHealth = 200;
	int curHealth;
	float healthPerc;

	public Color enemyColor = Color.white;

	SpriteRenderer sprRnd;

	public GameObject hitParticle;
	public GameObject destroyedParticle;

	[HideInInspector]
	public Level levelScr;

	void Start(){
		sprRnd = GetComponent<SpriteRenderer> ();
		sprRnd.color = enemyColor;
		curHealth = totalHealth;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "bullet") {
			Rigidbody2D bRgd = coll.gameObject.GetComponent<Rigidbody2D> ();
			Vector2 bVel = bRgd.velocity;
			ApplyDamage (bVel.magnitude, coll.contacts[0].point);

			if (!coll.gameObject.GetComponent<AttractedObj> ().freshSpawn) {
				bRgd.velocity = Vector2.zero;
				coll.gameObject.SetActive (false);
			}
//			Vector2 forceVec = bVel.normalized * 15f;
//			bRgd.AddForce (forceVec, ForceMode2D.Impulse);

		}

	}

	float lastShotStamp = 0.0f;
	void ApplyDamage(float fMagn, Vector2 colPoint){
		
		if (fMagn > 15f && Time.time - lastShotStamp > 0.075f) {

			GameObject hitPart = Instantiate (hitParticle, colPoint, Quaternion.identity)as GameObject;
			hitPart.GetComponent<ParticleSystem> ().startColor = sprRnd.color;

			Destroy (hitPart, 2f);

			lastShotStamp = Time.time;

			int dmg = (int)fMagn / 2;
			curHealth -= dmg;

			for (int i= 0; i < Random.Range(0, 3); i++){
				Vector2 spOffset = (colPoint - new Vector2 (transform.position.x, transform.position.y)).normalized;
				levelScr.ActivateAttrObj (colPoint,  spOffset * Random.Range(5f, 10f));
			}

			if (curHealth < 1) {
				GameObject destrPart = Instantiate (destroyedParticle, colPoint, Quaternion.identity)as GameObject;
				destrPart.GetComponent<ParticleSystem> ().startColor = sprRnd.color;
				Destroy (destrPart, 3f);

				for (int i= 0; i < Random.Range(3, 10); i++)
					levelScr.ActivateAttrObj (new Vector2(transform.position.x + Random.Range(-transform.localScale.x, transform.localScale.x), 
						transform.position.y + Random.Range(-transform.localScale.y, transform.localScale.y)),
						new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f,0.5f)));

				levelScr.EnemyKilled (gameObject);
				Destroyed ();
			}
		}
	}


	void Update(){
		
		healthPerc = ((float)totalHealth - (float)curHealth) / (float)totalHealth;
		if (healthPerc > 0.5f)
			healthPerc += 0.15f;
		sprRnd.color = Color.Lerp(enemyColor, Color.red, healthPerc);
	}

	void Destroyed(){
		gameObject.SetActive (false);
	}

	public float GetHealthPerc(){
		return healthPerc;
	}
}

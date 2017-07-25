using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed = 2f;
	public float rotationSpeed = 4f;

	public GameObject destroyedParticle;

	Rigidbody2D _rigidbody;

	public Level levelScr;

	void Start(){
		_rigidbody = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void FixedUpdate () {
	
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.velocity = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * speed * Time.fixedDeltaTime;
		//transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * speed);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		
		if (coll.gameObject.tag == "charDestoyer" && this.enabled) {
			levelScr.HeroKilled ();
			gameObject.SetActive (false);
			GameObject destrPart = Instantiate (destroyedParticle, transform.position, Quaternion.identity)as GameObject;
			destrPart.GetComponent<ParticleSystem> ().startColor = gameObject.GetComponentInChildren<Shoot> ().playerColor;
			Destroy (destrPart, 3f);
		}

	}
}

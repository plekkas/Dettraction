using UnityEngine;
using System.Collections;

public class AttractedObj : MonoBehaviour {

	Rigidbody2D _rigidbody;

	bool isShot = false;
	Vector3 shotDirection;

	SpriteRenderer _sprRnd;

	public Shoot shootScr;

	[HideInInspector]
	public bool freshSpawn = true;

	Color startColor;

	AudioSource _audioSource;

	void OnEnable(){
		freshSpawn = true;
		StartCoroutine (NotFresh ());
	}
		
	void OnDisable(){
		IsOutOfTheZone ();
	}

	IEnumerator NotFresh(){
		yield return new WaitForSeconds (1f);
		freshSpawn = false;
	}

	void Start(){
		_rigidbody = GetComponent<Rigidbody2D> ();
		_sprRnd = GetComponent<SpriteRenderer> ();
		_audioSource = GetComponent<AudioSource> ();
		startColor = _sprRnd.color;
	}

	void OnTriggerStay2D(Collider2D other){

		if (other.gameObject.tag == "attraction" && !isShot) {
			shootScr.IsAttracted (this);
			_rigidbody.AddForce ((other.transform.parent.position - transform.position).normalized * 30f);
			if (_sprRnd.color != shootScr.playerColor)
				_sprRnd.color = shootScr.playerColor;
		}
	}

	void OnTriggerExit2D(Collider2D other){

		if (other.gameObject.tag == "attraction") {
			IsOutOfTheZone ();
		}
	}
		

	void IsOutOfTheZone(){
		if (_sprRnd != null) {
			shootScr.IsOut (this);
			_sprRnd.color = startColor;
		}
	}


	public void GotShot(Vector2 direction){
		_audioSource.PlayOneShot (_audioSource.clip);
		shotDirection = direction;
		_rigidbody.velocity = Vector2.zero;
		isShot = true;
	}
		

	void FixedUpdate(){

		if (isShot) {
			_rigidbody.AddForce(shotDirection * 40000f * Time.fixedDeltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		
		if (coll.gameObject.tag == "charDestoyer"){
			//Debug.Log (_rigidbody.velocity + " " + -_rigidbody.velocity.normalized * 5f);
			isShot = false;
			Vector2 vel = _rigidbody.velocity;
			//_rigidbody.velocity = Vector2.zero;
			//_rigidbody.AddForce (-vel.normalized * 5f, ForceMode2D.Impulse);
		}

	}

		
}

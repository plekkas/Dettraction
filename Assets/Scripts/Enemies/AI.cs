using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	Rigidbody2D _rigidbody;

	enum EnemyState {Wandering, Hunting};
	EnemyState enemyState = EnemyState.Wandering;

	float wanderingStamp = 0.0f;
	float wanderingNext = 0.0f;
	Vector2 wanderingForce = Vector2.zero;

	EnemyHealth enemyHealth;

	public float huntThreshold = 0.25f;
	public int huntChance = 2;
	public float huntForceMult = 10f;

	[HideInInspector]
	public Transform character;

	void Start(){
		_rigidbody = GetComponent<Rigidbody2D> ();
		enemyHealth = gameObject.GetComponent<EnemyHealth> ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log (enemyState);
		HuntTarget ();
		switch (enemyState) {

		case EnemyState.Wandering:
			WanderingRoutine ();
			break;

		case EnemyState.Hunting:
			HuntRoutine ();
			break;

		}
		EscapeStuck ();
	}

	void WanderingRoutine(){

		if (Time.time - wanderingStamp > wanderingNext || _rigidbody.velocity.magnitude < 0.05f) {
			wanderingStamp = Time.time;
			wanderingNext = Random.Range (2f, 5f);
			wanderingForce = new Vector2 (Random.Range (-800f, 800f), Random.Range (-800f, 800f));
		}

		_rigidbody.AddForce (wanderingForce * Time.fixedDeltaTime);
	}

	void HuntTarget(){

		if (enemyState == EnemyState.Wandering && enemyHealth.GetHealthPerc () > huntThreshold && Random.Range (0, huntChance) == 0 && character.gameObject.activeSelf) {
			enemyState = EnemyState.Hunting;
			huntChance += 1;
		} else if (enemyState == EnemyState.Hunting && _rigidbody.velocity.magnitude < 2f && Random.Range (0, (int)(transform.localScale.x * 4f)) == 0) {
			enemyState = EnemyState.Wandering;
		}
	
	}

	void HuntRoutine(){

		if (!character.gameObject.activeSelf)
			enemyState = EnemyState.Wandering;
			
		_rigidbody.AddForce ((character.position - transform.position).normalized * huntForceMult *Time.fixedDeltaTime);
	}


	Vector2[] ustuckNorms = new Vector2[8]{Vector2.up, -Vector2.up, Vector2.right, -Vector2.right, new Vector2(1,1), new Vector2(-1,1), new Vector2(-1,-1), new Vector2(1,-1)};
	int unstuckIdx = 0;
	void EscapeStuck(){

		if (_rigidbody.velocity.magnitude == 0.0f) {

			_rigidbody.AddForce (ustuckNorms [unstuckIdx] * _rigidbody.mass * 2f, ForceMode2D.Impulse);
			unstuckIdx++;
		} else if (unstuckIdx > 0) {
			unstuckIdx = 0;
		}

	}
}

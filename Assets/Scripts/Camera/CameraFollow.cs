using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform character;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		transform.position = new Vector3(character.position.x, character.position.y, -10f);
	}
}

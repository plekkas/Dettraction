using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {


	Vector2 prevMousePos = Vector3.zero;
	// Update is called once per frame
	void Update () {

		Vector2 mousePos = Input.mousePosition;

		if (Vector2.Distance (mousePos, prevMousePos) > 0.01) {

			Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Quaternion rot = Quaternion.LookRotation (transform.position - mouseWorldPosition, Vector3.forward);

			transform.rotation = rot;  
			transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z); 
		}

		prevMousePos = mousePos;

		float rInputH = Input.GetAxis ("RightH");
		float rInputV = Input.GetAxis ("RightV");

		if (Mathf.Abs (rInputH) > 0.0f || Mathf.Abs (rInputV) > 0.0f) { 
			float angle = Mathf.Atan2 (-rInputH, -rInputV) * Mathf.Rad2Deg; 
			transform.rotation =  Quaternion.Lerp(transform.rotation, Quaternion.Euler (new Vector3 (0, 0, angle)), Time.deltaTime * 10f);

		}
	}
}

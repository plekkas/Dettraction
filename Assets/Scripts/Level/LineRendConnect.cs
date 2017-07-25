using UnityEngine;
using System.Collections;

public class LineRendConnect : MonoBehaviour {

	LineRenderer lineRenderer;
	public Transform connection;
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition (0, transform.position);
		lineRenderer.SetPosition (1, connection.position);
	}
}

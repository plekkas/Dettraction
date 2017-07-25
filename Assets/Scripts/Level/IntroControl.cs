using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroControl : MonoBehaviour {

	public void PlayButton(){
		SceneManager.LoadScene (1);
	}
}

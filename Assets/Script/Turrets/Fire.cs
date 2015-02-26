using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log("Fuego!");
						fire ();
				}

	}

	public void fire(){
		Debug.Log (particleSystem.enableEmission+":"+particleSystem.isPlaying);
		particleSystem.Play();
	}
}

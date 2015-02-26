using UnityEngine;
using System.Collections;

public class ColorFlash : MonoBehaviour {

	public Color initial;
	public Color alternative;

	private float cooldown;
	private float timeFlash;

	void Start () {
		renderer.material.color = initial;
		timeFlash = -0.3f;
		cooldown = Time.time;
	}

	void Update () {
		if(Time.time - cooldown > timeFlash)
			renderer.material.color = initial;
		else
			renderer.material.color = alternative;
	}

	public void flash(float time){
		timeFlash = time;
		cooldown = Time.time;
	}
}

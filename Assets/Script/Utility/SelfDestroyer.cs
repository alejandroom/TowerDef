using UnityEngine;
using System.Collections;

public class SelfDestroyer : MonoBehaviour {
	
	public int lifeTime = 10;
	private float life;

	void Start () {
		life = Time.time;	
	}

	void Update () {
		if(Time.time-life>lifeTime)
			Destroy (gameObject);	
	}
}

using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {
	public float intensity = 0.5f;

	void Start () {
		this.light.intensity = 0f;
	}

	void Update () {
		this.light.intensity -= intensity * Time.deltaTime;
		if (this.light.intensity < 0)
			this.light.intensity = 0;
	}

	public void flash(){
		this.light.intensity = intensity;
	}
}

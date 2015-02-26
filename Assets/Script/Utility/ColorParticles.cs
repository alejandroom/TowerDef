using UnityEngine;
using System.Collections;

public class ColorParticles : MonoBehaviour {

	public Color newColor;
	private Color oldColor;

	void Start(){
		oldColor = particleSystem.startColor;
	}
	
	public void change(){
		particleSystem.startColor = newColor;
	}
	
	public void revert(){
		particleSystem.startColor = oldColor;
	}
}

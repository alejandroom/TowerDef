using UnityEngine;
using System.Collections;

public class MeneaCubitos : MonoBehaviour {

	public bool meneito = false;
	public bool saltito = false;
	
	private float timeCicloSaltitos;
	private float timeCicloMeneitos;
	private float cooldownCiclo;
	private bool direccion;
	private int contador=0;

	private Quaternion initialRotation;

	void Start(){
		timeCicloSaltitos = 0.8f;
		timeCicloMeneitos = 0.4f;
		initialRotation = gameObject.transform.localRotation;

	}

	void Update(){
		gameObject.transform.localRotation = initialRotation;
		if (saltito) {
			gameObject.transform.localPosition = new Vector3(0, contador*Time.deltaTime*0.5f, 0);
			if(direccion)
				contador++;
			else
				contador--;

			if(Time.time-cooldownCiclo>timeCicloSaltitos){
				cooldownCiclo = Time.time;
				direccion=!direccion;
			}
		}
		if (meneito) {
			gameObject.transform.localPosition = new Vector3(contador*4f*Time.deltaTime, 0, 0);
			if(direccion)
				contador++;
			else
				contador--;

			if(Time.time-cooldownCiclo>timeCicloMeneitos){
				cooldownCiclo = Time.time;
				direccion=!direccion;		
			}
		}
		if (!saltito && !meneito)
			gameObject.transform.localPosition = new Vector3 (0, 0, 0);
	}

	public void saltar(){
		saltito = true;
		direccion = true;
		cooldownCiclo = Time.time;
	}

	public void menear(){
		meneito = true;
		direccion = (Random.value<0.5);
		cooldownCiclo = Time.time;
	}
}

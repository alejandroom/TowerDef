using UnityEngine;
using System.Collections;

public class Thump : MonoBehaviour {

	private float initialPos;
	private float descenso=0.4f;

	void Start () {
		initialPos = transform.position.y;
	}

	void Update () {
		if(transform.position.y<initialPos)
			transform.position = new Vector3 (transform.position.x, transform.position.y + (descenso*Time.deltaTime), transform.position.z);
		if(transform.position.y>initialPos)		
			transform.position = new Vector3 (transform.position.x, initialPos, transform.position.z);
	}

	public void thump(){
		transform.position = new Vector3 (transform.position.x, transform.position.y - descenso, transform.position.z);
	}
}

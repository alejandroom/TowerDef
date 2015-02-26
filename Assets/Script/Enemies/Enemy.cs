using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public int maxHealth=100;
	public int curHealth;
	public float speed=1.0f;
	public int type=2;

	public int path;

	public bool dead;
	
	private float baseHealthBarlenght;
	private float healthBarlenght;
	
	private float slowAmount;
	private float slowTime;
	private float slowTimeStamp;

	public Vector3 direction;

	private Interfaz score;
	
	void Start () {
		curHealth = maxHealth;
		dead = false;
		slowAmount = 1.0f;

		baseHealthBarlenght = Screen.width / 20;
		healthBarlenght = Screen.width / 20;
		
		direction = transform.TransformDirection(Vector3.forward);
		
		if (type == 2) {
			MeneaCubitos meneitos = gameObject.GetComponentInChildren<MeneaCubitos> ();
			if (meneitos != null){
				meneitos.saltar();
			}
		}
		if (type == 3) {
			MeneaCubitos meneitos = gameObject.GetComponentInChildren<MeneaCubitos> ();
			if (meneitos != null)
				meneitos.menear();
		}

		score = GameObject.Find ("Scoreboard").GetComponent<Interfaz> ();
	}

	public void boss(){
		if (type == 1) { /* Tanque */
			maxHealth*=10;
			curHealth = maxHealth;
		}
		if (type == 2) { /* Helicoptero */
			maxHealth*=2;
			curHealth = maxHealth;
			speed*=1.5f;
		}
		if (type == 3) { /* Robot */
			maxHealth*=5;
			curHealth = maxHealth;
			speed*=1.2f;
		}
		transform.localScale *= 2f;
	}

	public void updateDifficulty(float diff){
		maxHealth = Mathf.FloorToInt(maxHealth*(1+diff));
		curHealth = maxHealth;
	}
	
	void Update(){
		transform.rotation = Quaternion.LookRotation (direction, Vector3.up);
		Vector3 velocidad = direction * speed * slowAmount;
		velocidad.y = 0;
		rigidbody.velocity = velocidad;
		
		if (Time.time - slowTimeStamp > slowTime)
			slowAmount=1.0f;
	}

	public void slow(float eff, float time){
		slowTime = time;
		slowAmount = eff;
		slowTimeStamp = Time.time;
	}

	public void newDirection(Vector3 newD){
		direction = newD;
	}

	void OnGUI () {
		Vector3 enemyPos = Camera.main.WorldToScreenPoint(transform.position);
		Rect rect = new Rect(enemyPos.x-baseHealthBarlenght/2, Screen.height-enemyPos.y-baseHealthBarlenght, healthBarlenght, 20);
		Texture tex = Resources.Load<Texture> ("Red");
		
		GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit, true, 10.0F);
	}

	public void AdjustCurHealth (int adj) {
		curHealth += adj;
		if (curHealth < 0) {
			dead = true;
			curHealth = 0;
			score.addPuntuation(maxHealth);
			Destroy(gameObject);
		}
		if(curHealth > maxHealth)
			curHealth = maxHealth;
		if(maxHealth < 1)
			maxHealth = 1;
		healthBarlenght = baseHealthBarlenght * (curHealth / (float)maxHealth);
	}

	public bool isAttackable(){
		return !dead;
	}
}

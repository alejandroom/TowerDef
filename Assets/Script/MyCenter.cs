using UnityEngine;
using System.Collections;

public class MyCenter : MonoBehaviour {
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	public Texture2D progressBarHalf;
	public Texture2D progressBarDead;

	private float barDisplay = 0.0f;
	private Vector2 size = new Vector2(240,40);

	private int maxHealth = 1000;
	public int actualHealth = 1000;

	private ColorFlash[] castle;
	private ColorParticles particles;

	private float flashCooldown;
	private float timeBetweenFlash = 1f;

	public bool showHealthBar = true;

	void Start(){
		actualHealth = maxHealth;
		castle = GameObject.FindObjectsOfType<ColorFlash>();
		particles = GameObject.FindObjectOfType<ColorParticles> ();
	}
	
	void OnGUI(){
		Texture2D tex = progressBarFull;
		barDisplay = ((actualHealth * 1.0f) / (maxHealth * 1.0f));
		
		if (barDisplay < 0.6)
			tex = progressBarHalf;
		if (barDisplay < 0.3)
			tex = progressBarDead;

		if (showHealthBar) {
			GUI.DrawTexture (new Rect (0, 0, size.x, size.y), progressBarEmpty);
			GUI.DrawTexture (new Rect (0, 0, size.x * barDisplay, size.y), tex);
		}
	}

	void OnTriggerEnter(Collider other){
		Enemy intruder = other.GetComponent<Enemy> ();
		if (intruder != null) {
			actualHealth -=intruder.curHealth;
			Destroy (other.gameObject);
			flash (0.1f);
		}

		Healer heal = other.GetComponent<Healer> ();
		if (heal != null) {
			actualHealth += heal.healing;
			if(actualHealth < 0)
				actualHealth = 0;
			Destroy (other.gameObject);
		}
	}

	void Update(){
		if (((actualHealth * 1.0f) / (maxHealth * 1.0f)) < 0.1f) {
			if(Time.time - flashCooldown > timeBetweenFlash){
				flashCooldown = Time.time;
				flash (timeBetweenFlash/2);
			}
		}

		if (((actualHealth * 1.0f) / (maxHealth * 1.0f)) < 0.5f)
			particles.change ();
		else
			particles.revert ();
	}

	void flash(float time){
		foreach (ColorFlash flash in castle) {
			flash.flash(time);
		}
	}
}

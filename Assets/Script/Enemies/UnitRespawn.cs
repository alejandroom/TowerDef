using UnityEngine;
using System.Collections;

public class UnitRespawn : MonoBehaviour {
	
	public GameObject slowBadGuy;
	public GameObject fastBadGuy;
	public GameObject evilBadGuy;
	
	public GameObject healer;
	public bool healerActive;

	public int path;

	/* Enemies */
	private float timeBetweenEnemies=0.3f;
	private float cooldown;
	private int enemiesPerWave=5;
	private Queue nextEnemy;

	/* Healing */
	private float cooldownHeal;
	private float probHeal = 0.3f;
	private float timeBetweenHeals = 3f;
	private float minHeal = 50f;
	private float maxHeal = 100f;


	/* Difficulty */
	private float difPerRound = 0.1f;
	private int numRounds = 0;

	void Start () {
		cooldownHeal = Time.time;
		cooldown = Time.time;
		nextEnemy = new Queue();
	}

	void Update () {
		if (nextEnemy.Count != 0 && Time.time - cooldown > timeBetweenEnemies) {
			Vector3 pos = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
			string type = (string)nextEnemy.Dequeue ();
			GameObject badGuy = evilBadGuy;
			float delay = 0.0f;

			if (type == "Slow") {
				delay = 0.4f;
				badGuy = slowBadGuy;
			}else if (type == "Fast"){
				badGuy = fastBadGuy;
			}else if (type == "Boss"){
				float rand = Random.value;
				Debug.Log("Random boss:"+rand);
				if(rand<0.3){
					delay = 0.4f;
					badGuy = slowBadGuy;
				}else if(rand<0.6){
					badGuy = fastBadGuy;
				}
			}

			GameObject clone = Instantiate (badGuy, pos, transform.rotation) as GameObject;
			Enemy enem = clone.GetComponent<Enemy> ();
			enem.path = path;
			if (type == "Boss")
				enem.boss();
			enem.updateDifficulty(numRounds*1.0f*difPerRound);
			cooldown = Time.time + delay;
		}

		if (!healerActive)
			return;
		if (Time.time - cooldownHeal > timeBetweenHeals) {
			float rnd = Random.value;
			if(rnd < probHeal){
				Vector3 pos = new Vector3 (transform.position.x, transform.position.y+0.2f, transform.position.z);
				GameObject clone = Instantiate (healer, pos, transform.rotation) as GameObject;
				Healer heal = clone.GetComponent<Healer>();
				heal.healing = Mathf.CeilToInt(Random.Range(minHeal, maxHeal));
			}
			cooldownHeal = Time.time;
		}
	}
	
	public void go(){
		if (numRounds % 3 == 0) {
			for(int i=0; i<enemiesPerWave;i++)
				nextEnemy.Enqueue("Slow");
		}
		else if (numRounds % 3 == 1) {
			for(int i=0; i<enemiesPerWave;i++)
				nextEnemy.Enqueue("Fast");			
		}
		else if (numRounds % 3 == 2) {
			for(int i=0; i<enemiesPerWave;i++)
				nextEnemy.Enqueue("Evil");
		}
		numRounds++;
	}
	
	public void boss(){
		nextEnemy.Enqueue("Boss");
		numRounds++;
	}
}

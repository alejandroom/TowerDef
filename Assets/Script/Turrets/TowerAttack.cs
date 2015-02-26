using UnityEngine;
using System.Collections;

public class TowerAttack : MonoBehaviour {
	
	public int damage = 10;
	public float radius = 10.0f;
	public float rate = 1f;
	public int type = 2; /* 1=Arrow, 2=area, 3=nubecita */
	public GameObject proyectile;
	public int level;

	private ArrayList targets;
	private float cooldown;
	private float dmgPerLevel = 0.2f;

	void Start () {
		this.transform.localScale = new Vector3(radius*(0.4f/transform.parent.localScale.x), this.gameObject.transform.localScale.y, radius*(0.4f/transform.parent.localScale.z));
		targets = new ArrayList();
		cooldown = Time.time;
		level = 0;
	}

	void OnTriggerEnter(Collider other){
		Enemy intruder = other.GetComponent<Enemy> ();
		if (intruder != null)
			targets.Add(other);
	}

	void OnTriggerExit(Collider other){
		Enemy intruder = other.GetComponent<Enemy> ();
		if (intruder != null)
			targets.Remove(other);
	}

	void Update(){
		if (type == 1)
			TypeOne ();
		else if (type == 2)
			TypeTwo ();
		else if (type == 3)
			TypeThree ();
	}
	
	void TypeOne(){
		foreach (Collider intrud in targets) {
			if (intrud == null)
				continue;
			
			Enemy intruder = intrud.GetComponent<Enemy> ();
			if (intruder == null)
				continue;
			
			if (!intruder.isAttackable())
				continue;
			
			if (Time.time-cooldown<rate)
				break;
			
			cooldown = Time.time;

			int DMG = Mathf.CeilToInt(damage + damage*level*dmgPerLevel);

			if(Random.value<0.10)
				DMG*=2;

			intruder.AdjustCurHealth(-DMG);
			transform.parent.GetComponentInChildren<AutoCannon>().bang();
			transform.parent.GetComponentInChildren<Fire>().fire();
			transform.parent.GetComponentInChildren<Turret>().target=intrud.transform;
			break;
		}
	}
	
	void TypeTwo(){
		bool attack = false;
		
		if (Time.time-cooldown<rate)
			return;
		
		cooldown = Time.time;
		foreach (Collider intrud in targets) {
			if (intrud == null)
				continue;

			Enemy intruder = intrud.GetComponent<Enemy> ();
			if (intruder == null)
				continue;
			
			if (!intruder.isAttackable())
				continue;
			
			int DMG = Mathf.CeilToInt(damage + damage*level*dmgPerLevel);

			intruder.AdjustCurHealth(-DMG);
			attack=true;
		}
		if (attack) {
			this.transform.parent.GetComponentInChildren<Thump>().thump();
			this.transform.parent.GetComponentInChildren<Flash>().flash();
		}
	}
	
	void TypeThree(){
		foreach (Collider intrud in targets) {
			if (intrud == null)
				continue;
			
			Enemy intruder = intrud.GetComponent<Enemy> ();
			if (intruder == null)
				continue;
			
			if (!intruder.isAttackable())
				continue;
			
			if (Time.time-cooldown<rate)
				break;
			
			cooldown = Time.time;
			
			Vector3 pos = new Vector3(intrud.transform.position.x, intrud.transform.position.y+0.25f, intrud.transform.position.z);
			
			GameObject clone = Instantiate(proyectile, pos, transform.rotation) as GameObject;
			Blizzard proy = clone.GetComponentInParent<Blizzard> ();
			proy.target = intrud.transform;
			proy.lifeTime = rate;
			proy.slowAmount = 0.1f*(level+1);
			if(proy.slowAmount > 0.6f)
				proy.slowAmount = 0.6f;
			proy.timeSlow = 0.1f*(level+1);
			break;
		}
	}
}

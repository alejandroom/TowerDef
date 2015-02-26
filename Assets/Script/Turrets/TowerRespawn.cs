using UnityEngine;
using System.Collections;

public class TowerRespawn : MonoBehaviour {

	//private bool go;
	private bool activeRespawn;
	
	public GameObject MagicTower;
	public GameObject AreaTower;
	public GameObject ArrowTower;

	public Texture2D option;

	private int over; //0 = nada, 1 = over, 2 = popup, 4 = update

	private float cooldown;
	private int waitTime;
	
	private int MagicCost=300;
	private int AreaCost=250;
	private int ArrowCost=200;
	private int costeMejora;

	private Interfaz score;
	private InterfazTutorial scoreTut;

	private TowerAttack tower;

	void Start () {
		over = 0;
		activeRespawn = true;

		cooldown = Time.time;
		waitTime = 3;
	
		score = GameObject.FindObjectOfType<Interfaz> ();
		scoreTut = GameObject.FindObjectOfType<InterfazTutorial> ();
	}

	void Update(){
		if (Time.time - cooldown > waitTime)
			over = 0;
	}
	
	void OnMouseOver () {
		if (activeRespawn && over==0) {
			cooldown = Time.time;
			waitTime = 3;
			over = 1;
		}
		if (!activeRespawn && over==0) {
			cooldown = Time.time;
			waitTime = 3;
			over = 4;
		}
	}
	
	void OnMouseExit () {
		if (activeRespawn && over==1) {
			cooldown = Time.time;
			waitTime = 1;
		}
	}

	void OnGUI(){
		if (over == 1) {
			Vector3 TowerPos = Camera.main.WorldToScreenPoint(transform.position);
			if (GUI.Button (new Rect (TowerPos.x - 25, Screen.height - TowerPos.y -25, 50, 50), option)){
				cooldown = Time.time;
				waitTime = 3;
				over = 2;
			}
		}
		if (over == 2) {
			Vector3 TowerPos = Camera.main.WorldToScreenPoint(transform.position);
			
			if (GUI.Button (new Rect (TowerPos.x-150, Screen.height - TowerPos.y - 70, 140, 80), "WEATHER STATION\n Coste: "+MagicCost+"\n Daño: Bajo\n Alcance: Alto\n Especial: Ralentiza")){
				if((score && score.resources>=MagicCost) || (scoreTut && scoreTut.resources>=MagicCost)){
					if(score)
						score.resources-=MagicCost;
					if(scoreTut)
						scoreTut.resources-=MagicCost;

					Vector3 pos = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
					GameObject torre = Instantiate(MagicTower, pos, transform.rotation) as GameObject;
					tower = torre.GetComponentInChildren<TowerAttack>();
					Debug.Log ("Desplegada FrostTower!");

					activeRespawn = false;
					costeMejora = MagicCost*2;
					over = 0;
				}
			}
			
			if (GUI.Button (new Rect (TowerPos.x-50, Screen.height - TowerPos.y + 20, 130, 80), "THUMPER\n Coste: "+AreaCost+"\n Daño: Medio\n Alcance: Alto\n Especial: Area")){
				if((score && score.resources>=AreaCost) || (scoreTut && scoreTut.resources>=AreaCost)){
					if(score)
						score.resources-=AreaCost;
					if(scoreTut)
						scoreTut.resources-=AreaCost;

					Vector3 pos = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
					GameObject torre = Instantiate(AreaTower, pos, transform.rotation) as GameObject;
					tower = torre.GetComponentInChildren<TowerAttack>();
					Debug.Log ("Desplegada CannonTower!");
				
					activeRespawn=false;
					costeMejora = AreaCost*2;
					over = 0;
				}
			}
			
			if (GUI.Button (new Rect (TowerPos.x+40, Screen.height - TowerPos.y - 70, 130, 80), "AUTOCANNON\n Coste: "+ArrowCost+"\n Daño: Alto\n Alcance: Medio\n Especial: Criticos")){
				if((score && score.resources>=ArrowCost) || (scoreTut && scoreTut.resources>=ArrowCost)){
					if(score)
						score.resources-=ArrowCost;
					if(scoreTut)
						scoreTut.resources-=ArrowCost;

					Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
					GameObject torre = Instantiate(ArrowTower, pos, transform.rotation) as GameObject;
					tower = torre.GetComponentInChildren<TowerAttack>();
					Debug.Log ("Desplegada ArrowTower!");
				
					activeRespawn=false;
					costeMejora = ArrowCost*2;
					over = 0;
				}
			}
		}
		if (over == 4) {
			Vector3 TowerPos = Camera.main.WorldToScreenPoint(transform.position);
			if (GUI.Button (new Rect (TowerPos.x - 75, Screen.height - TowerPos.y -25, 150, 50), "Torre Nivel " + tower.level + "\nCoste mejora: " + costeMejora)){
				if((score && score.resources>=costeMejora) || (scoreTut && scoreTut.resources>=costeMejora)){
					if(score)
						score.resources-=costeMejora;
					if(scoreTut)
						scoreTut.resources-=costeMejora;
					costeMejora*=2;

					tower.level++;

					cooldown = Time.time;
					waitTime = 3;
					over = 0;
				}
			}
		}
	}
}

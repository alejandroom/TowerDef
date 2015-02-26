using UnityEngine;
using System.Collections;
using System;

public class Interfaz : MonoBehaviour {
	
	public int puntuation = 0;
	public int resources = 0;

	private UnitRespawn[] respawns;
	private int nextBoss = 0;
	private int roundsBetweenBoss = 4;
	private int numRound = 0;
	
	private Texture2D roundCharge;
	private Texture2D roundChargeEmpty;
	private int numCharges=5;
	private int maxCharges=5;
	private float chargeRechargeTime = 5f;
	private float chargeCooldown;
	private float roundCooldown;
	private float timeBetweenRounds=20f;

	private bool starting;
	private bool fin;

	public bool genericMap = false;
	
	private Texture2D playTex;
	private Texture2D pauseTex;
	private Texture2D halfTex;
	private Texture2D doubleTex;
	
	private Texture2D playOnTex;
	private Texture2D pauseOnTex;
	private Texture2D halfOnTex;
	private Texture2D doubleOnTex;
	
	private Texture2D playOffTex;
	private Texture2D pauseOffTex;
	private Texture2D halfOffTex;
	private Texture2D doubleOffTex;

	private MyCenter center;
	private int finalPuntuation;
	private float finalTime;
	private float initialTime;
	private bool pro;
	private Score sc;
	private string winnerName = "Nombre Aqui!";
	public bool isTraining;
	
	void Start(){
		if (genericMap)
			GameObject.FindObjectOfType<MapLoader> ().launch ();
		roundCharge = (Texture2D)Resources.Load("RoundCharge", typeof(Texture2D));
		roundChargeEmpty = (Texture2D)Resources.Load("RoundChargeEmpty", typeof(Texture2D));
		playOnTex = (Texture2D)Resources.Load("TimeControls/PlayOn", typeof(Texture2D));
		pauseOnTex = (Texture2D)Resources.Load("TimeControls/PauseOn", typeof(Texture2D));
		halfOnTex = (Texture2D)Resources.Load("TimeControls/HalfOn", typeof(Texture2D));
		doubleOnTex = (Texture2D)Resources.Load("TimeControls/DoubleOn", typeof(Texture2D));
		playOffTex = (Texture2D)Resources.Load("TimeControls/PlayOff", typeof(Texture2D));
		pauseOffTex = (Texture2D)Resources.Load("TimeControls/PauseOff", typeof(Texture2D));
		halfOffTex = (Texture2D)Resources.Load("TimeControls/HalfOff", typeof(Texture2D));
		doubleOffTex = (Texture2D)Resources.Load("TimeControls/DoubleOff", typeof(Texture2D));
		initialize ();
	}

	public void initialize(){
		center = GameObject.FindObjectOfType<MyCenter> ();
		respawns = GameObject.FindObjectsOfType<UnitRespawn> ();
		chargeCooldown = Time.time;
		roundCooldown = Time.time;
		numRound = 0;
		nextBoss = 0;
		numCharges = 0;
		starting = true;
		puntuation = 0;
		Pause ();
		resources = respawns.Length * 1000;
		fin = false;
		initialTime = Time.time;
		pro = false;
	}

	void OnGUI()
	{
		if (fin) {
			int seconds = Mathf.FloorToInt(finalTime-initialTime);
			TimeSpan t = TimeSpan.FromSeconds(seconds);
			string tiempo = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);

			GUI.BeginGroup (new Rect (Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2));
			GUI.Box (new Rect (0, 0, Screen.width/2, Screen.height/2), "");
			GUI.Box (new Rect (Screen.width/8, 0, Screen.width/4, Screen.height/8), "Enhorabuena! Tu puntuacion es: \n" + finalPuntuation + "\nen " + tiempo);
			if (pro) {
				GUI.Box (new Rect (Screen.width/9.5f, Screen.height/6, Screen.width/3.5f, Screen.height/20), "Tu nombre permanecera en las leyendas!");
				winnerName = GUI.TextField (new Rect (Screen.width/8, Screen.height/4, Screen.width/4, Screen.height/20), winnerName);
				if(GUI.Button(new Rect (Screen.width/8, Screen.height/4+Screen.height/8, Screen.width/4, Screen.height/8), "OK!")){
					if(winnerName != "Nombre Aqui!" && winnerName.Trim() != ""){
						sc.newScore(winnerName, finalPuntuation, seconds);
						sc.Save();
					}
					Application.LoadLevel("MainMenu");
				}
			}else{
				if(GUI.Button(new Rect (Screen.width/8, Screen.height/4, Screen.width/4, Screen.height/8), "OK!"))
					Application.LoadLevel("MainMenu");
			}
			GUI.EndGroup();
			return;
		}


		if (starting) {
			if (GUI.Button (new Rect (Screen.width - 120, 0, 120, 50), "START!")) {
				chargeCooldown = Time.time;
				roundCooldown = Time.time;
				starting = false;
				Play ();
			}
		}else{
			if (GUI.Button (new Rect (Screen.width - 120, 0, 120, 50), "Siguiente ronda!")) {
				if (numCharges > 0 && !starting) {
					numCharges--;
					sigRonda ();
				}
			}
		}

		GUI.BeginGroup (new Rect (Screen.width-150, 60, 150, 25));
		if (GUI.Button (new Rect (0, 0, 25, 25), halfTex, new GUIStyle ()))
			Half ();
		if (GUI.Button (new Rect (35, 0, 25, 25), pauseTex, new GUIStyle ()))
			Pause ();
		if (GUI.Button (new Rect (70, 0, 25, 25), playTex, new GUIStyle ()))
			Play ();
		if (GUI.Button (new Rect (105, 0, 25, 25), doubleTex, new GUIStyle ()))
			Double ();
		GUI.EndGroup ();

		GUI.Box (new Rect (Screen.width-150, 90, 150, 25), "Recursos: " + resources);
		GUI.Box (new Rect (Screen.width / 2 - 75, 10, 150, 25), "Puntuacion: " + puntuation);


		float texLength = ((timeBetweenRounds - (Time.time - roundCooldown)) / timeBetweenRounds) * 120;
		GUI.DrawTexture (new Rect (Screen.width - texLength, 50, texLength, 5), roundCharge);

		drawCharges ();
	}

	void sigRonda(){
		bool boss=false;
		if(numRound==roundsBetweenBoss)
			boss=true;
		
		for(int i=0;i<respawns.Length;i++){
			if(boss && nextBoss%respawns.Length==i)
				respawns[i].boss();
			else
				respawns[i].go ();
		}
		if(boss){
			numRound=0;
			nextBoss++;
		}else
			numRound++;
	}

	void drawCharges(){
		for (int i=0; i<maxCharges; i++) {
			if (numCharges >= i+1)
				GUI.DrawTexture (new Rect (Screen.width - 140 - i*10, 5, 4, 40), roundCharge);
			else
				GUI.DrawTexture (new Rect (Screen.width - 140 - i*10, 5, 4, 40), roundChargeEmpty);
		}
	}

	void Update(){
		if (center.actualHealth <= 0 && !fin) {
			finalPuntuation = puntuation;
			finalTime = Time.time;
			fin = true;

			if(!isTraining){
				sc = new Score();
				sc.Load();
				Debug.Log(sc.Debug());
				pro = sc.eval(finalPuntuation);
			}
		}
		if (fin)
			return;

		if (Time.time - chargeCooldown > chargeRechargeTime) {
			if (numCharges < maxCharges)
				numCharges++;
			chargeCooldown = Time.time;
		}

		if (Time.time - roundCooldown > timeBetweenRounds) {
			sigRonda ();
			roundCooldown = Time.time;
		}
	}	

	public void exit(){
		finalPuntuation = puntuation;
		finalTime = Time.time;
		fin = true;
		
		if(!isTraining){
			sc = new Score();
			sc.Load();
			Debug.Log(sc.Debug());
			pro = sc.eval(finalPuntuation);
		}
	}
	
	public void addPuntuation(int punt){
		puntuation += punt;
		resources += punt;
	}

	void Play(){
		playTex = playOnTex;
		pauseTex = pauseOffTex;
		doubleTex = doubleOffTex;
		halfTex = halfOffTex;
		Time.timeScale = 1f;
	}

	void Pause(){
		playTex = playOffTex;
		pauseTex = pauseOnTex;
		doubleTex = doubleOffTex;
		halfTex = halfOffTex;
		Time.timeScale = 0f;
	}

	void Half(){
		playTex = playOffTex;
		pauseTex = pauseOffTex;
		doubleTex = doubleOffTex;
		halfTex = halfOnTex;
		Time.timeScale = 0.5f;
	}

	void Double(){
		playTex = playOffTex;
		pauseTex = pauseOffTex;
		doubleTex = doubleOnTex;
		halfTex = halfOffTex;
		Time.timeScale = 2f;
	}
}

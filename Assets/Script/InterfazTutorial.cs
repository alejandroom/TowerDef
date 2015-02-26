using UnityEngine;
using System.Collections;
using System;

public class InterfazTutorial : MonoBehaviour {
	
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
	
	private Texture2D leftArrow;
	private Texture2D upArrow;
	private Texture2D rightArrow;
	private Texture2D intro;
	private Texture2D centerTex;
	private Texture2D respawnTex;
	private Texture2D roadsTex;
	private Texture2D towersTex;

	private MyCenter center;
	private int finalPuntuation;
	private float finalTime;
	private float initialTime;

	private bool tutorial;
	private bool desplegable;
	private bool vida;
	private bool punt;
	private bool cargas;
	private bool controles;
	private bool recursos;
	private bool movimiento;

	private int numEscena;
	
	void Start(){
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
		
		leftArrow = (Texture2D)Resources.Load("Tutorial/ArrowLeft", typeof(Texture2D));
		upArrow = (Texture2D)Resources.Load("Tutorial/ArrowUp", typeof(Texture2D));
		rightArrow = (Texture2D)Resources.Load("Tutorial/ArrowRight", typeof(Texture2D));
		intro = (Texture2D)Resources.Load("Tutorial/Intro", typeof(Texture2D));
		centerTex = (Texture2D)Resources.Load("Tutorial/Center", typeof(Texture2D));
		respawnTex = (Texture2D)Resources.Load("Tutorial/Respawn", typeof(Texture2D));
		roadsTex = (Texture2D)Resources.Load("Tutorial/Roads", typeof(Texture2D));
		towersTex = (Texture2D)Resources.Load("Tutorial/Towers", typeof(Texture2D));
		initialize ();
		numEscena = 0;
		tutorial = true;
	}

	void tooltips(){
		desplegable = true;
		vida = true;
		punt = true;
		cargas = true;
		controles = true;
		recursos = true;
		movimiento = true;
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
			if(GUI.Button(new Rect (Screen.width/8, Screen.height/4, Screen.width/4, Screen.height/8), "OK!"))
				Application.LoadLevel("MainMenu");
			GUI.EndGroup();
			return;
		}

		if (tutorial) {
			if(numEscena == 0){
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
				GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "");
				GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "Bienvenido a Tower'Em All!");
				if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Siguiente"))
					numEscena++;
				
				GUI.Box (new Rect (4 * Screen.width / 10 - Screen.width / 4, Screen.height / 5, Screen.width / 2, Screen.height / 2), "Debes proteger tu fortaleza de las tropas enemigas!" +
				         "\nEste breve tutorial te mostrara como defenderte!");
				GUI.Box (new Rect (4 * Screen.width / 10 - Screen.width / 6, Screen.height / 5 + 50, Screen.width / 3, Screen.height / 3), intro);
				GUI.EndGroup ();
			}else if(numEscena == 1){
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
				GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
				if(GUI.Button (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Anterior"))
					numEscena--;
				GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "Tu fortaleza");
				if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Siguiente"))
					numEscena++;
				
				GUI.Box (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), "");
				GUI.DrawTexture (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), centerTex);
				
				GUI.Box (new Rect (5*Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 2), "Tu fortaleza es el centro \nde tu pequeño imperio." +
				         "\n\nDebes defenderla de\n las tropas enemigas. \n\nPara ello impide que los\n enemigos logren alcanzarla. \n\nSi la vida de tu fortaleza \nllega a 0, perderas!");
				GUI.EndGroup ();
			}else if(numEscena == 2){
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
				GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
				if(GUI.Button (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Anterior"))
					numEscena--;
				GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "La fuente");
				if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Siguiente"))
					numEscena++;
				
				GUI.Box (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), "");
				GUI.DrawTexture (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), respawnTex);
				
				GUI.Box (new Rect (5*Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 2 + 10), "Todos los enemigos brotan\n de las fuentes!" +
				         "\n\nHay tres tipos de enemigos:\n\n-Tanques: \nLentos pero muy resistentes...\nno les dejes acercarse!" +
				         "\n\n-Helicopteros: \nNo aguantan mucho, \npero son rapidos!" +
				         "\n\n-Robots: \nTemibles enemigos estandar!");
				GUI.EndGroup ();
			}else if(numEscena == 3){
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
				GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
				if(GUI.Button (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Anterior"))
					numEscena--;
				GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "Los caminos");
				if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Siguiente"))
					numEscena++;
				
				GUI.Box (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), "");
				GUI.DrawTexture (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), roadsTex);
				
				GUI.Box (new Rect (5*Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 2 + 10), "Estas carreteras recorren\n el mapa desde las \nfuentes de enemigos\n hasta tu fortaleza." +
				         "\n\nLos enemigos las \nseguiran ciegamente... \naprovecha para poner\n torres que acaben con ellos!");
				GUI.EndGroup ();
			}else if(numEscena == 4){
				GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
				GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
				if(GUI.Button (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Anterior"))
					numEscena--;
				GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "Tus defensas");
				if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Continuar")){
					tutorial = false;
					tooltips ();
				}

				GUI.Box (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), "");
				GUI.DrawTexture (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), towersTex);
				
				GUI.Box (new Rect (5*Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 2 + 10), "Estos nodos de defensa \nte permiten desplegar \ntus mortiferas defensas!" +
					"\n\n-Autocannons:\n Tu principal defensa.\n Daño alto a un solo objetivo." +
					"\n\n-Thumper:\n Martillea el suelo con fuerza,\n dañando todos los\n enemigos cercanos." +
					"\n\n-Weather Station:\n Crea vestiscas sobre el\n enemigo para ralentizar\n su avance!");
				GUI.EndGroup ();
			}
		}
		
		if(desplegable){
			if(GUI.Button (new Rect (Screen.height / 10 + 20, Screen.height - Screen.height / 10, 180, 55), "Este menu desplegable te \npermitira volver al menu \nprincipal o cerrar el juego."))
				desplegable = false;
			GUI.Box (new Rect (Screen.height / 10, Screen.height - Screen.height / 10, 20, 20), leftArrow);
		}
		
		if(vida){
			if(GUI.Button (new Rect (0, 40, 150, 55), "Esta barra representa \nla vida de tu fortaleza!\nDebes defenderla!"))
				vida = false;
			GUI.Box (new Rect (150, 40, 20, 20), upArrow);
		}
		
		if(punt){
			if(GUI.Button (new Rect (Screen.width / 2 - 100, 40, 140, 55), "La puntuacion es\n tu maxima prioridad!\n Mata para conseguirla!"))
				punt = false;
			GUI.Box (new Rect (Screen.width / 2 +40, 40, 20, 20), upArrow);
		}
		
		if(cargas){
			if(GUI.Button (new Rect (Screen.width - 400, 0, 180, 55), "Esta cargas llaman\n oleadas anticipadamente. \nSe recargan con el tiempo!"))
				cargas = false;
			GUI.Box (new Rect (Screen.width - 400 + 180, 0, 20, 20), rightArrow);
		}
		
		if(controles){
			if(GUI.Button (new Rect (Screen.width - 350, 60, 180, 55), "Estos controles te permitiran\n controlar la velocidad.\n Incluyendo pausar el juego!"))
				controles = false;
			GUI.Box (new Rect (Screen.width - 350 + 180, 60, 20, 20), rightArrow);
		}
		
		if(recursos){
			if(GUI.Button (new Rect (Screen.width - 20 - 180, 115, 180, 55), "Los recursos te permiten\n construir defensas.\n Mata para conseguirlos!"))
				recursos = false;
			GUI.Box (new Rect (Screen.width - 20, 115, 20, 20), upArrow);
		}
		
		if(movimiento){
			if(GUI.Button (new Rect (Screen.width - 180, 200, 90, 20), "Movimiento"))
				movimiento = false;
			if(GUI.Button (new Rect (Screen.width - 180 - 80, 220, 250, 100), "Una vez comenzada la partida,\n puedes mover la camara libremente\n para ver mejor el campo de batalla!" +
				"\n\nCon el boton derecho del raton pulsado,\n desplazate con las teclas WASD."))
				movimiento = false;
		}

		if (starting) {
			if (GUI.Button (new Rect (Screen.width - 120, 0, 120, 50), "START!") && !tutorial) {
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

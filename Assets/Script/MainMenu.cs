using UnityEngine;
using System.Collections;
using System;

public class MainMenu : MonoBehaviour {

	public GUIStyle tituloStyle;

	private int state; /* 0 = main menu; 1 = scene selection; 2 = scores*/
	private int numScene;
	private int maxScenes = 2;
	private Texture2D[] sceneTextures;
	private string[] sceneTitles;
	private bool confirmarSalida;

	private float salidaCooldown;

	private Score sc;

	void Start(){
		confirmarSalida = false;
		state = 0;
		numScene = 0;
		MapLoader[] loaders = GameObject.FindObjectsOfType<MapLoader> ();
		
		for (int i=1; i<loaders.Length; i++)
			Destroy (loaders [i].gameObject);

		sceneTextures = new Texture2D[maxScenes];
		for (int i=0; i<maxScenes; i++)
			sceneTextures[i] = Resources.Load("SceneTextures/SceneTexture"+(i+1)) as Texture2D;

		sceneTitles = new string[maxScenes];
		sceneTitles [0] = "Cuadrado";
		sceneTitles [1] = "Rectangular";

		sc = new Score ();
		sc.Load ();
	}

	void Update(){
		if (Time.time - salidaCooldown > 2f)
			confirmarSalida = false;
	}

	void OnGUI () {
		GUIStyle boxCentered = new GUIStyle(GUI.skin.box);
		boxCentered.alignment = TextAnchor.MiddleCenter;
		GUIStyle buttonCentered = new GUIStyle(GUI.skin.button);
		buttonCentered.alignment = TextAnchor.MiddleCenter;
		if (state == 0) {
			GUI.Box (new Rect (Screen.width / 2 - 150, 0, 300, 100), "TOWER'EM ALL!!", tituloStyle);

			GUI.BeginGroup (new Rect (Screen.height / 10, Screen.height / 5, Screen.height / 2, Screen.height / 1.5f));
			GUI.Box (new Rect (0, 0, Screen.height / 2, Screen.height / 1.5f), "");
			if (GUI.Button (new Rect (10, 10, (Screen.height / 2) - 20, Screen.height / 10), "Nuevo juego"))
				state = 1;
			if (GUI.Button (new Rect (10, 10 + (10 + (Screen.height / 10)) * 1, (Screen.height / 2) - 20, Screen.height / 10), "Tutorial"))
				tutorial ();
			if (GUI.Button (new Rect (10, 10 + (10 + (Screen.height / 10)) * 2, (Screen.height / 2) - 20, Screen.height / 10), "Puntuaciones"))
				puntuaciones ();
			if (GUI.Button (new Rect (10, 10 + (10 + (Screen.height / 10)) * 3, (Screen.height / 2) - 20, Screen.height / 10), "Creditos"))
				creditos ();
			if(!confirmarSalida){
				if (GUI.Button (new Rect (10, 10 + (10 + (Screen.height / 10)) * 4, (Screen.height / 2) - 20, Screen.height / 10), "Salir del juego")){
					salidaCooldown = Time.time;
					confirmarSalida = true;
				}
			}else
				if (GUI.Button (new Rect (10, 10 + (10 + (Screen.height / 10)) * 4, (Screen.height / 2) - 20, Screen.height / 10), "Confirmar"))
					salir ();
			GUI.EndGroup ();
		}else if(state == 1){
			GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
			GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
			if(GUI.Button (new Rect (Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Anterior")){
				numScene -= 1;
				if(numScene<0)
					numScene = maxScenes-1;
			}
			GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), sceneTitles[numScene], boxCentered);
			if(GUI.Button (new Rect (6*Screen.width / 10, Screen.height / 20, Screen.width / 10, Screen.height / 10), "Siguiente"))
				numScene = (numScene+1)%maxScenes;

			GUI.Box (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), "");
			GUI.DrawTexture (new Rect (Screen.width / 20, Screen.height / 5, Screen.width / 2.8f, Screen.height / 2), sceneTextures[numScene]);
			
			if(GUI.Button (new Rect (5*Screen.width / 10, Screen.height / 5, Screen.width / 5, Screen.height / 10), "Pequeño"))
				newGame(1);
			if(GUI.Button (new Rect (5*Screen.width / 10, Screen.height / 5 + Screen.height / 10 + Screen.height / 20, Screen.width / 5, Screen.height / 10), "Mediano"))
				newGame(2);
			if(GUI.Button (new Rect (5*Screen.width / 10, Screen.height / 5 + (Screen.height / 10 + Screen.height / 20)*2, Screen.width / 5, Screen.height / 10), "Grande"))
				newGame(3);

			GUI.EndGroup ();

			if (GUI.Button (new Rect (0, Screen.height / 10, (Screen.width / 10), Screen.height / 10), "Volver"))
				state = 0;
		}else if(state == 2){
			GUI.BeginGroup (new Rect (Screen.width / 10, Screen.height / 10, 8 * Screen.width / 10, 8 * Screen.height / 10));
			GUI.Box (new Rect (0, 0, 8 * Screen.width / 10, 8 * Screen.height / 10), "");
			GUI.Box (new Rect (2*Screen.width / 10, Screen.height / 20, 4*Screen.width / 10, Screen.height / 10), "Maximas Puntuaciones!", boxCentered);
			
			string text = string.Format("{0,26}{1,20}{2,20}", "Nombre", "Puntuacion", "Tiempo");
			GUI.Box (new Rect (Screen.width / 10, 3*Screen.height / 20, 6*Screen.width / 10, Screen.height / 20), text);
			for(int i = 0 ; i < sc.data.name.Length ; i++){
				TimeSpan t = TimeSpan.FromSeconds(sc.data.time[i]);
				if(sc.data.score[i] != 0)
					text = string.Format("{0,5}. {1,-20}{2,20}{3,12}{4:D2}:{5:D2}:{6:D2}", i+1, sc.data.name[i], sc.data.score[i], "", t.Hours, 
					                     t.Minutes, t.Seconds);
				else
					text = string.Format("{0,5}. {1,-20}{2,20}{3,20}", i+1, "---", "---","---");

				GUI.Box (new Rect (Screen.width / 10, (i+4)*Screen.height / 20, 6*Screen.width / 10, Screen.height / 20), text);
			}
			GUI.EndGroup ();
			
			if (GUI.Button (new Rect (0, Screen.height / 10, (Screen.width / 10), Screen.height / 10), "Volver"))
				state = 0;
		}
	}

	void newGame(int size){
		MapLoader loader = GameObject.FindObjectOfType<MapLoader> ();
		if (numScene == 0){
			loader.type = "Square";
			loader.Height = 10;
			loader.Width = 10;
			loader.numEntrances = 4;
		}else if(numScene == 1){
			loader.type = "Rectangular";
			loader.Height = 14;
			loader.Width = 6;
			loader.numEntrances = 2;
		}

		if(size == 1){
			loader.Height /= 2;
			loader.Width /= 2;
			loader.numEntrances /= 2;
		}else if(size == 3){
			loader.Height *= 2;
			loader.Width *= 2;
			loader.numEntrances *= 2;
		}

		Application.LoadLevel ("GenericMap");
	}

	void tutorial(){
		Application.LoadLevel ("Tutorial");
	}

	void puntuaciones(){
		state = 2;
	}

	void creditos(){

	}
	
	void salir(){
		Application.Quit ();
	}
}

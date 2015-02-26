using UnityEngine;
using System.Collections;

public class MenuInferior : MonoBehaviour {

	public Texture2D icon;

	private bool standby;
	private float actualX;
	private float actualY;

	private float heightButtons = 50;
	private float widthButtons = 120;

	private Rect textureRect;
	
	private int actualButton;
	private int numButtons = 2;

	private float timeOpened = 0.1f;
	private float cooldown;

	private float speed = 8f;
	
	private float widthExitConfirm;
	public float widthMenuConfirm;

	void Start(){
		widthExitConfirm = 0f;
		widthMenuConfirm = 0f;
		textureRect = new Rect (0, Screen.height-Screen.height / 10, Screen.height / 10, Screen.height / 10);
		standby = true;
	}
	
	void OnGUI(){
		GUI.DrawTexture(textureRect, icon);
		if (!standby) {
			if(actualButton<numButtons){
				if(actualX<heightButtons){
					actualX+=1f*speed;
					if(actualX>heightButtons)
						actualX = heightButtons;
					GUI.Button (new Rect (0, Screen.height - Screen.height/10 - actualX - heightButtons*actualButton, actualY, actualX), "");
				}else if(actualY<widthButtons){
					actualY+=1f*speed;
					if(actualY>widthButtons)
						actualY = widthButtons;
					GUI.Button (new Rect (0, Screen.height - Screen.height/10 - actualX - heightButtons*actualButton, actualY, actualX), "");
				}else{
					actualButton++;
					actualX = 0f;
					actualY = 10f;
				}
			}
			
			if(actualButton>0){
				if (GUI.Button (new Rect (0, Screen.height - Screen.height/10 - heightButtons, widthButtons, heightButtons), new GUIContent("Salir del juego", "BotonExit")) &&
				    widthExitConfirm >= widthButtons){
					Debug.Log("Cerrando aplicacion!");
					Application.Quit();
				}
			}
			
			if(actualButton>1){
				if (GUI.Button (new Rect (0, Screen.height - Screen.height/10 - heightButtons*2, widthButtons, heightButtons), new GUIContent("Menu principal", "BotonMenu")) &&
				    widthMenuConfirm >= widthButtons){
					Interfaz faz = GameObject.FindObjectOfType<Interfaz>();
					if(faz)
						faz.exit();
					else
						Application.LoadLevel("MainMenu");
				}
			}

			if(GUI.tooltip == "BotonMenu"){
				widthMenuConfirm += 5f;
				if(widthMenuConfirm > widthButtons){
					widthMenuConfirm = widthButtons;
				}
				GUI.Button (new Rect (0, Screen.height - Screen.height/10 - heightButtons*2, widthMenuConfirm, heightButtons), "");
				cooldown = Time.realtimeSinceStartup;
			}
			
			if(GUI.tooltip == "BotonExit"){
				widthExitConfirm += 5f;
				if(widthExitConfirm > widthButtons){
					widthExitConfirm = widthButtons;
				}
				GUI.Button (new Rect (0, Screen.height - Screen.height/10 - heightButtons, widthExitConfirm, heightButtons), "");
				cooldown = Time.realtimeSinceStartup;
			}
		}
	}

	void Update(){
		if (Input.mousePosition.x < Screen.height / 10 &&  Input.mousePosition.y < Screen.height / 10)
			cooldown = Time.realtimeSinceStartup;

		if (Input.mousePosition.x < Screen.height / 10 &&  Input.mousePosition.y < Screen.height / 10 && standby) {
			standby = false;
			actualX = 0f;
			actualY = 10f;
			actualButton = 0;
			widthExitConfirm = 0f;
			widthMenuConfirm = 0f;
		}

		if (Time.realtimeSinceStartup - cooldown > timeOpened)
			standby = true;
	}
}

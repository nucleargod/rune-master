using UnityEngine;
using System.Collections;

public class TitleMenu : MonoBehaviour {
	
	private Rect buttonArea;
	// Use this for initialization
	void Start () {
		buttonArea = new Rect(0, 0, Screen.width*0.5f, Screen.width*0.5f);
		buttonArea.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if(GUI.Button(buttonArea, "Tap to Start"))
		{
			SceneManager.GoTo(SceneList.themeMenu);
		}
	}
}

using UnityEngine;
using System.Collections;

public class m_UI : MonoBehaviour {
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(0, Screen.height/4.0f*0.0f, Screen.width, Screen.height/4.0f), "Battle mode"))
			Application.LoadLevel("battleScene");
		if(GUI.Button(new Rect(0, Screen.height/4.0f*1.0f, Screen.width, Screen.height/4.0f), "Practice mode"))
			Application.LoadLevel("practiceScene");
		if(GUI.Button(new Rect(0, Screen.height/4.0f*2.0f, Screen.width, Screen.height/4.0f), "theme mode"))
			Application.LoadLevel("themeScene");
		if(GUI.Button(new Rect(0, Screen.height/4.0f*3.0f, Screen.width, Screen.height/4.0f), "Exit"))
			Application.Quit();
	}
}

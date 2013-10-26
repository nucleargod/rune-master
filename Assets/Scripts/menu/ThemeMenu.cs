using UnityEngine;
using System.Collections;

public struct ThemeSet {
	public int id;
	public string name;
	public Texture texture;
}

public class ThemeMenu : MonoBehaviour {
	public string title;
	//public string[] items;
	public ThemeSet[] themeSets;
	
	private Rect titleRect; // auto X and Y
	private Rect itemRect; // auto X and Y
	
	private Rect groupArea; // auto size
	private Rect tmpRect; // for loop
	
	// Use this for initialization
	void Start () {
		titleRect.x = 0;
		titleRect.y = 0;
		itemRect = new Rect(0.0f, 0.0f, Screen.width*0.9f, Screen.height*0.2f);
		itemRect.center = new Vector2(Screen.width*0.5f, 0);
		groupArea.height = Screen.height;
		groupArea.width = Screen.width;
		//groupArea.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
		tmpRect = itemRect;
		themeSets = new ThemeSet[10];
		for (int i = 0; i < themeSets.Length; i++)
		{
			themeSets[i].id = i;
			themeSets[i].name = "Theme "+i.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private Vector2 scrollViewVector = Vector2.zero;
	void OnGUI () {
		//GUI.Button( titleRect, title)
		
		GUILayout.BeginArea(groupArea);
		scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, true, false);
		GUILayout.BeginVertical();
		for(int i = 0; i < themeSets.Length; i++)
		{
			if(GUILayout.Button(themeSets[i].name, GUILayout.Width(itemRect.width), GUILayout.Height(itemRect.height)))
			{
				SceneManager.GoTo(SceneList.chapterMenu, themeSets[i].id);
			}
		}
		
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		if(GUI.Button(new Rect(Screen.width*0.7f, Screen.height*0.9f, Screen.width*0.3f, Screen.height*0.1f),"Back"))
		{
			SceneManager.GoTo(SceneList.title);
		}
	}
}

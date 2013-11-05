using UnityEngine;
using System.Collections;

public class ThemeMenu : MonoBehaviour {
	public themeRecord[] themes;
	public Texture lockedImg;
	public float aspect = 3.0f;
	
	private Rect itemRect;
	private Rect groupArea;
	// Use this for initialization
	void Start () {
		itemRect = new Rect(0.0f, 0.0f, Screen.width*0.9f, Screen.width*0.9f/aspect);
		itemRect.center = new Vector2(Screen.width*0.5f, 0);
		groupArea.height = Screen.height;
		groupArea.width = Screen.width;
		//groupArea.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
		//themes = new themeRecord[10];
		//for (int i = 0; i < themes.Length; i++)
		//{
		//	themes[i] = new themeRecord();
		//	themes[i].id = i;
		//	//themes[i].name = "Theme "+i.ToString();
		//	if(i < 5)
		//		themes[i].status = themeRecord.ThemeStatus.unlocked;
		//	else
		//		themes[i].status = themeRecord.ThemeStatus.locked;
		//}
		themes = DataManager.Instance.modelComponent.getThemes();
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
		for(int i = 0; i < themes.Length; i++)
		{
			if(themes[i].status == themeRecord.ThemeStatus.locked)
			{
				GUILayout.Button(lockedImg, GUILayout.Width(itemRect.width), GUILayout.Height(itemRect.height));
			}
			else if(GUILayout.Button(themes[i].img, GUILayout.Width(itemRect.width), GUILayout.Height(itemRect.height)))
			{
				SceneManager.GoTo(SceneList.chapterMenu, themes[i].id);
			}
		}
		
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		// go back button
		if(GUI.Button(new Rect(Screen.width*0.7f, Screen.height*0.9f, Screen.width*0.3f, Screen.height*0.1f),"Back"))
		{
			SceneManager.GoTo(SceneList.title);
		}
	}
}

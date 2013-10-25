using UnityEngine;
using System.Collections;

public enum SceneList
{
	title,
	themeMenu,
	chapterMenu,
	game,
	exit
}

public class SceneManager : MonoBehaviour {
	public static SceneManager Instants = null; // SceneManager.Instants
	public int seletedTheme;
	public int seletedChapter;
	public SceneList currentScene;
	
	// change this for your scenes
	public string titleSceneName = "title";
	public string themeMenuSceneName = "themeMenu";
	public string chapterMenuSceneName = "chapterMenu";
	public string gameSceneName = "game";
	
	
	// Use this for initialization
	void Start () {
		if (Instants == null)
		{
			DontDestroyOnLoad(this.gameObject);
			Instants = this;
			seletedTheme = 0;
			seletedChapter = 0;
			currentScene = SceneList.title;
			GoTo(SceneList.title);
		}
		else
		{
			Debug.Log("SceneManager is already exists");
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// call this function for switch scenes
	// if no select, used last one
	public static void GoTo(SceneList sl, int selected = -1) {
		switch(sl) {
		case SceneList.title:
			Application.LoadLevel(Instants.titleSceneName);
			break;
			
		case SceneList.themeMenu:
			Application.LoadLevel(Instants.themeMenuSceneName);
			break;
			
		case SceneList.chapterMenu:
			if (selected!=-1)
				Instants.seletedTheme = selected;
			Application.LoadLevel(Instants.chapterMenuSceneName);
			break;
			
		case SceneList.game:
			if (selected!=-1)
				Instants.seletedChapter = selected;
			Application.LoadLevel(Instants.gameSceneName);
			break;
			
		case SceneList.exit:
			Application.Quit();
			break;
			
		default:
			Debug.Log("Scenes not found");
			return;
			break;
		}
		Instants.currentScene = sl;
	}
}

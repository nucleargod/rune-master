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
	public static void GoTo(SceneList sl) {
		switch(sl) {
		case SceneList.title:
			Application.LoadLevel("title");
			break;
			
		case SceneList.themeMenu:
			Application.LoadLevel("themeMenu");
			break;
			
		case SceneList.chapterMenu:
			Application.LoadLevel("chapterMenu");
			break;
			
		case SceneList.game:
			Application.LoadLevel("game");
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

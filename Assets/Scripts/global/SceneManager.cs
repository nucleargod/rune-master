using UnityEngine;
using System.Collections;

// 如何轉場
// 應該跳去哪個場景

public class SceneManager : MonoBehaviour {
	public static SceneManager Instance = null;	// SceneManager.Instance
	
	// change this for your scenes
	public string titleSceneName = "menuScene";
	public string themeMenuSceneName = "themeMenu";
	public string chapterMenuSceneName = "chapterMenu";
	public string gameSceneName = "game";
	public string resultSceneName = "result";
	
	// Use this for initialization
	void Start () {
		if (Instance == null)
		{
			Instance = this;
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
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
	
	// SceneManager.GoTo(SceneList, int)
	// call this function for switch scenes
	// 當轉換至選單場景時需要selected變數，若不填寫則使用最近一次的資料
	public static void GoTo(SceneList sl, int selected = -1) {
		switch(sl) {
		case SceneList.title:
			Application.LoadLevel(Instance.titleSceneName);
			break;
			
		case SceneList.themeMenu:
			Application.LoadLevel(Instance.themeMenuSceneName);
			break;
			
		case SceneList.chapterMenu:
			if (selected!=-1)
				Global.Instance.seletedTheme = selected;
			Application.LoadLevel(Instance.chapterMenuSceneName);
			break;
			
		case SceneList.game:
			if (selected!=-1)
				Global.Instance.seletedChapter = selected;
			Application.LoadLevel(Instance.gameSceneName);
			break;
			
		case SceneList.result:
			Application.LoadLevel(Instance.resultSceneName);
			break;
			
		case SceneList.exit:
			Application.Quit();
			break;
			
		default:
			Debug.Log("Scenes not found");
			return;
			break;
		}
		Global.Instance.currentScene = sl;
	}
}

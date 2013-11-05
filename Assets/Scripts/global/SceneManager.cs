using UnityEngine;
using System.Collections;

// 如何轉場
// 應該跳去哪個場景

public class SceneManager : MonoBehaviour {
	public static SceneManager Instance = null;	// SceneManager.Instance
	public static AsyncOperation AO = null;
	
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
	
	void OnGUI () {
		GUILayout.Box("Loading..."+(AO.progress*100).ToString("F0")+"%");
		if (AO.isDone) this.enabled = false;
	}
	
	// SceneManager.GoTo(SceneList, int)
	// call this function for switch scenes
	// 當轉換至選單場景時需要selected變數，若不填寫則使用最近一次的資料
	public static void GoTo(SceneList sl, int selected = -1) {
		switch(sl) {
		case SceneList.title:
			AO = Application.LoadLevelAsync(Instance.titleSceneName);
			Instance.enabled = true;
			break;
			
		case SceneList.themeMenu:
			AO = Application.LoadLevelAsync(Instance.themeMenuSceneName);
			Instance.enabled = true;
			break;
			
		case SceneList.chapterMenu:
			if (selected!=-1)
				Global.Instance.seletedTheme = selected;
			AO = Application.LoadLevelAsync(Instance.chapterMenuSceneName);
			Instance.enabled = true;
			break;
			
		case SceneList.game:
			if (selected!=-1)
				Global.Instance.seletedChapter = selected;
			AO = Application.LoadLevelAsync(Instance.gameSceneName);
			Instance.enabled = true;
			break;
			
		case SceneList.result:
			AO = Application.LoadLevelAsync(Instance.resultSceneName);
			Instance.enabled = true;
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

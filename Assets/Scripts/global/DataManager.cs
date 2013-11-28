using UnityEngine;
using System.Collections;

// 負責提供所有場景所需要的資料
// 所有需要資料庫配合的資料結構都在此定義

public class DataManager : MonoBehaviour {
	public static DataManager Instance = null;
	public static model modelComponent = null;
	public static GlobalRecord recordComponent = null;
	// Use this for initialization
	void Start () {
		if(Instance == null) {
			Instance = this;
			modelComponent = this.GetComponentInChildren<model>();
			recordComponent = this.GetComponentInChildren<GlobalRecord>();
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
		}
		else {
			Debug.Log("DataManager is already exists");
			Destroy(this.gameObject);
		}
	}
	//*
	public static themeRecord GetTheme (int selected) {
		return null;
	}//*/
	
	public static chapterRecord GetChapter (int selected) {
		return null;
	}
	/*
	public static ThemeSet[] GetThemeList () {
		return null;
	}//*/
	
	public static chapterRecord[] GetChapterList () {
		return null;
	}
	//*
	public static void UpdateTheme (themeRecord theme) {
	}//*/
	
	public static void UpdateChapter (chapterRecord chapter) {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

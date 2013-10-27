using UnityEngine;
using System.Collections;

// 負責提供所有場景所需要的資料
// 所有需要資料庫配合的資料結構都在此定義

// data struct
/*
[System.Serializable]
public class ThemeSet {
	public int			id;
	public string		name;
	public Texture		img;
	public float		score;
	public ThemeStatus	status;
}//*/

[System.Serializable]
public class ChapterSet {
	public int				id;
	public int				themeId;
	public string			name;
	public Texture			img;
	public float			score;
	public ChapterStatus	status;
}

public class DataManager : MonoBehaviour {
	public static DataManager Instants = null;
	public model modelComponent;
	public GlobalRecord recordComponent;
	// Use this for initialization
	void Start () {
		if(Instants == null) {
			Instants = this;
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
	
	public static ChapterSet GetChapter (int selected) {
		return null;
	}
	/*
	public static ThemeSet[] GetThemeList () {
		return null;
	}//*/
	
	public static ChapterSet[] GetChapterList () {
		return null;
	}
	//*
	public static void UpdateTheme (themeRecord theme) {
	}//*/
	
	public static void UpdateChapter (ChapterSet chapter) {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

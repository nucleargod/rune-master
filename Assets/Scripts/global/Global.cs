using UnityEngine;
using System.Collections;

// 你覺得其他的Scene可能會使用的變數都可以寫在這裡
// enum
public enum SceneList
{
	title,
	themeMenu,
	chapterMenu,
	game,
	exit
}

public enum ChapterStatus{
	locked,
	unlocked,
}

public enum ThemeStatus{
	locked,
	unlocked,
}

public class Global : MonoBehaviour {
	public static Global Instants = null;	// Global.Instants
	public int seletedTheme;	// Global.Instants.seletedTheme
	public int seletedChapter;	// Global.Instants.seletedChapter
	public SceneList currentScene;	// Global.Instants.currentScene
	
	// Use this for initialization
	void Start () {
		if (Instants == null)
		{
			Instants = this;
			seletedChapter = 0;
			seletedTheme = 0;
			currentScene = SceneList.title;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Debug.Log("Global is already exists");
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		SceneManager.GoTo(SceneList.title); // 避免未完全的初始化
		this.enabled = false;
	}
}

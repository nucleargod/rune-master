using UnityEngine;
using System.Collections;

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
	aClass,
	bClass,
	cClass
}

public class Global : MonoBehaviour {
	public static Global Instants = null;
	public int seletedTheme;
	public int seletedChapter;
	public SceneList currentScene;
	
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

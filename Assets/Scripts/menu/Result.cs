using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {
	public float chapterThreshold = 100.0f; // 解鎖新章節的門檻值
	public float themeThreshold = 500.0f; // 解鎖新主題的門檻值
	
	private ThemeSet currentTheme;
	private ThemeSet nextTheme;
	private ChapterSet currentChapter;
	private ChapterSet nextChapter;
	private string rank = "F";
	// Use this for initialization
	void Start () {
		currentTheme = DataManager.GetTheme(Global.Instants.seletedTheme);
		nextTheme = DataManager.GetTheme(Global.Instants.seletedTheme+1);
		currentChapter = DataManager.GetChapter(Global.Instants.seletedChapter);
		nextChapter = DataManager.GetChapter(Global.Instants.seletedChapter+1);
		
		// 更新此次遊戲結果
		if(Global.Instants.battleResult > (float)Rank.A)
			rank = "A";
		else if(Global.Instants.battleResult > (float)Rank.B)
			rank = "B";
		else if(Global.Instants.battleResult > (float)Rank.C)
			rank = "C";
		currentChapter.score = Global.Instants.battleResult;
		DataManager.UpdateChapter(currentChapter);
		DataManager.UpdateTheme(currentTheme);
		
		// 若有下一關且分數大於門檻值則更新相關資料
		if(nextChapter != null && currentChapter.score >= chapterThreshold) {
			nextChapter.status = ChapterStatus.unlocked;
			DataManager.UpdateChapter(nextChapter);
		}
		if(nextTheme != null && currentTheme.score >= themeThreshold) {
			nextTheme.status = ThemeStatus.unlocked;
			DataManager.UpdateTheme(nextTheme);
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUILayout.Button("Rank: "+rank);
		if(GUILayout.Button("Try Again"))
			SceneManager.GoTo(SceneList.game);
		if(GUILayout.Button("Next Chapter") && nextChapter != null && nextChapter.status == ChapterStatus.unlocked)
			SceneManager.GoTo(SceneList.game, Global.Instants.seletedChapter+1);
		if(GUILayout.Button("Go Back To Title Menu"))
			SceneManager.GoTo(SceneList.title);
		if(GUILayout.Button("Go Back To Theme Menu"))
			SceneManager.GoTo(SceneList.themeMenu);
		if(GUILayout.Button("Go Back To Chapter Menu"))
			SceneManager.GoTo(SceneList.chapterMenu);
	}
}

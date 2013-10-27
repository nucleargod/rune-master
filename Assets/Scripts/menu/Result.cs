using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {
	public float chapterThreshold = 100.0f; // 解鎖新章節的門檻值
	public float themeThreshold = 500.0f; // 解鎖新主題的門檻值
	
	private themeRecord currentTheme;
	private themeRecord nextTheme;
	private ChapterSet currentChapter;
	private ChapterSet nextChapter;
	private string rank = "F";
	private float scoreIncrease;
	// Use this for initialization
	void Start () {
		currentTheme = DataManager.GetTheme(Global.Instants.seletedTheme);
		nextTheme = DataManager.GetTheme(Global.Instants.seletedTheme+1);
		currentChapter = DataManager.GetChapter(Global.Instants.seletedChapter);
		nextChapter = DataManager.GetChapter(Global.Instants.seletedChapter+1);
		
		// 計算此次遊戲結果
		if(Global.Instants.battleResult > (float)Rank.A)
			rank = "A";
		else if(Global.Instants.battleResult > (float)Rank.B)
			rank = "B";
		else if(Global.Instants.battleResult > (float)Rank.C)
			rank = "C";
		scoreIncrease = Global.Instants.battleResult-currentChapter.score;
		
		// 若分數沒有增加則不更新結果
		if(scoreIncrease<0.0f)
			return;
		
		// 更新結果
		currentChapter.score = Global.Instants.battleResult;
		currentTheme.score += scoreIncrease;
		
		// 若有下一關且分數大於門檻值則解鎖下一關
		if(nextChapter != null && currentChapter.score >= chapterThreshold) 
			nextChapter.status = ChapterStatus.unlocked;
		
		if(nextTheme != null && currentTheme.score >= themeThreshold)
			nextTheme.status = themeRecord.ThemeStatus.unlocked;
		
		// 將結果寫入資料庫
		DataManager.UpdateChapter(nextChapter);
		DataManager.UpdateTheme(nextTheme);
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

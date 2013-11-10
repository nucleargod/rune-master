using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {
	public float chapterThreshold = 100.0f; // 解鎖新章節的門檻值
	public float themeThreshold = 500.0f; // 解鎖新主題的門檻值
	
	private themeRecord currentTheme = null;
	private themeRecord nextTheme = null;
	private chapterRecord currentChapter = null;
	private chapterRecord nextChapter = null;
	private string rank = "F";
	private float scoreIncrease;
	
	private Rect rRank;
	private Rect rTryAgain;
	private Rect rNextChapter;
	private Rect rTitle;
	private Rect rTheme;
	private Rect rChapter;
	// Use this for initialization
	void Start () {
		// 設定UI位置
		float h = Screen.height;
		float w = Screen.width;
		rRank = new Rect(0, 0, h*0.3f, h*0.3f);
		rRank.center = new Vector2(w*0.5f, h*0.5f);
		rNextChapter = new Rect(rRank);
		rNextChapter.x = w-h*0.3f;
		rTryAgain = new Rect(rRank);
		rTryAgain.x = 0;
		rChapter = new Rect(w-100.0f, h-50.0f, 100.0f, 50.0f);
		rTitle = new Rect(0.0f, h-50.0f, 100.0f, 50.0f);
		rTheme = new Rect(w*0.5f-50, h-50.0f, 100.0f, 50.0f);
		
		// 初始化章節資訊
		currentTheme = DataManager.Instance.modelComponent.getTheme(Global.Instance.seletedTheme);
		nextTheme = DataManager.Instance.modelComponent.getTheme(Global.Instance.seletedTheme+1);
		currentChapter = currentTheme.chapters[Global.Instance.seletedChapter];
		nextChapter = nextTheme.chapters[Global.Instance.seletedChapter+1];
		
		// 計算此次遊戲結果
		if(Global.Instance.battleResult > (float)Rank.A)
			rank = "A";
		else if(Global.Instance.battleResult > (float)Rank.B)
			rank = "B";
		else if(Global.Instance.battleResult > (float)Rank.C)
			rank = "C";
		return;
		scoreIncrease = Global.Instance.battleResult-currentChapter.score;
		
		// 若分數沒有增加則不更新結果
		if(scoreIncrease<0.0f)
			return;
		
		// 更新結果
		currentChapter.score = Global.Instance.battleResult;
		currentTheme.score += scoreIncrease;
		
		// 若有下一關且分數大於門檻值則解鎖下一關
		if(nextChapter != null && currentChapter.score >= chapterThreshold) 
			nextChapter.status = chapterRecord.ChapterStatus.unlocked;
		
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
		GUI.Button(rRank, rank);
		if(GUI.Button(rTryAgain, "Try\nAgain"))
			SceneManager.GoTo(SceneList.game);
		if(GUI.Button(rNextChapter, "Next\nChapter") && nextChapter != null && nextChapter.status == chapterRecord.ChapterStatus.unlocked)
			SceneManager.GoTo(SceneList.game, Global.Instance.seletedChapter+1);
		if(GUI.Button(rTitle, "Title\nMenu"))
			SceneManager.GoTo(SceneList.title);
		if(GUI.Button(rTheme, "Theme\nMenu"))
			SceneManager.GoTo(SceneList.themeMenu);
		if(GUI.Button(rChapter, "Chapter\nMenu"))
			SceneManager.GoTo(SceneList.chapterMenu);
	}
}

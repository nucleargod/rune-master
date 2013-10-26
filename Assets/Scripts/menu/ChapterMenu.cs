using UnityEngine;
using System.Collections;

public class ChapterMenu : MonoBehaviour {
	public ChapterSet[] chapterSets;
	public Texture lockedImg;
	public float ratio = 0.2f;
	
	private Rect itemRect; // auto X and Y
	private Rect groupArea; // auto size
	
	// Use this for initialization
	void Start () {
		itemRect = new Rect(0.0f, 0.0f, Screen.width*ratio, Screen.width*ratio);
		itemRect.center = new Vector2(Screen.width*0.5f, 0);
		groupArea.height = Screen.height;
		groupArea.width = Screen.width;
		
		// chapterSets = DataManager.GetChapterList();
		chapterSets = new ChapterSet[30];
		for(int i = 0; i < chapterSets.Length; i++)
		{
			chapterSets[i].id = i;
			chapterSets[i].name = "CP "+i.ToString();
			chapterSets[i].status = ChapterStatus.locked;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// 必須再OnGUI呼叫，當狀態非locked而且被點選時會回傳True
	bool MakeChapter (int i) {
		bool isClick;
		if (chapterSets[i].status == ChapterStatus.locked)
		{
			GUILayout.Button(lockedImg,
			GUILayout.Width(itemRect.width), 
			GUILayout.Height(itemRect.height));
			return false;
		}
		isClick = GUILayout.Button(chapterSets[i].name,
			GUILayout.Width(itemRect.width), 
			GUILayout.Height(itemRect.height));
		return isClick;
	}
	
	private Vector2 scrollViewVector = Vector2.zero;
	void OnGUI () {
		// 只是一些GUI的排版
		float tmpWidth = 0.0f;
		GUILayout.BeginArea(groupArea);
		scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, true, false);
		
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		for(int i = 0; i < chapterSets.Length; i++)
		{
			if(tmpWidth+itemRect.width > (float)Screen.width*0.95f)
			{
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.BeginHorizontal();
				tmpWidth = 0.0f;
			}
			GUILayout.FlexibleSpace();
			if(MakeChapter(i))
			{
				SceneManager.GoTo(SceneList.game, chapterSets[i].id);
			}
			tmpWidth = tmpWidth+itemRect.width;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		
		// go back button
		if(GUI.Button(new Rect(Screen.width*0.7f, Screen.height*0.9f, Screen.width*0.3f, Screen.height*0.1f),"Back"))
		{
			SceneManager.GoTo(SceneList.themeMenu);
		}
	}
}

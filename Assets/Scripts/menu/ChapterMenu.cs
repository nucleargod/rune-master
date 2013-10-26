using UnityEngine;
using System.Collections;

public enum ChapterStatus{
	locked,
	unlocked,
	aClass,
	bClass,
	cClass
}

public struct ChapterSet {
	public int id;
	public string name;
	public ChapterStatus status;
}

public class ChapterMenu : MonoBehaviour {
	public string title;
	public ChapterSet[] chapterSets;
	
	private Rect titleRect; // auto X and Y
	private Rect itemRect; // auto X and Y
	
	private Rect groupArea; // auto size
	private Rect tmpRect; // for loop
	
	// Use this for initialization
	void Start () {
		titleRect.x = 0;
		titleRect.y = 0;
		itemRect = new Rect(0.0f, 0.0f, Screen.width*0.2f, Screen.height*0.2f);
		itemRect.center = new Vector2(Screen.width*0.5f, 0);
		groupArea.height = Screen.height;
		groupArea.width = Screen.width;
		//groupArea.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
		tmpRect = itemRect;
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
		isClick = GUILayout.Button(chapterSets[i].name,
			GUILayout.Width(itemRect.width), 
			GUILayout.Height(itemRect.height));
		if (chapterSets[i].status == ChapterStatus.locked)
			return false;
		return isClick;
	}
	
	private Vector2 scrollViewVector = Vector2.zero;
	void OnGUI () {
		//GUI.Button( titleRect, title);
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
		
		if(GUI.Button(new Rect(Screen.width*0.7f, Screen.height*0.9f, Screen.width*0.3f, Screen.height*0.1f),"Back"))
		{
			Debug.Log("Click");
			SceneManager.GoTo(SceneList.themeMenu);
		}
	}
}

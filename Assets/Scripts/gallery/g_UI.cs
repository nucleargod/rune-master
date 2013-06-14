using UnityEngine;
using System.Collections;

public class g_UI : MonoBehaviour {
	
	public GlobalRecord rcd;
	
	
	public Material unknow;
	public Texture2D unknowT;
	public GUITexture wordView;
	public GUIText recordsView;
	public GUIText avgView;
	
	private GameObject wordWall;
	private int totalNum;
	private int recordNum;
	private int recordNow;
	private System.Collections.Generic.List<string> wordList;
	private wordRecord[] wordRecords;
	
	// Use this for initialization
	void Start () 
	{
		wordWall = GameObject.Find("wordWall");
		
		rcd = GameObject.Find("GlobalRecord").GetComponent<GlobalRecord>();
		wordRecords = rcd.records;
			
		wordList  = rcd.database.getWords();
		totalNum  = wordList.Count;
		recordNum = wordRecords.Length;
		recordNow = 0;
		
		//init size
		float w = Screen.width*3/5;
		wordView.pixelInset = new Rect(-w*0.5f, w, -w*0.5f, w);
		/*wordView.pixelInset.width = w;
		wordView.pixelInset.x = -w*0.5f;
		wordView.pixelInset.height = w;
		wordView.pixelInset.y = -w*0.5f;//*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isRecord() == true)
		{
			Word word = rcd.database.getWord(wordList[recordNow]);
			wordView.texture = word.image;
			//avgView = 
			
			
			wordWall.renderer.material = word.mat;
			
		}
		else
		{
			wordView.texture = unknowT;
			wordWall.renderer.material = unknow;
		}
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(0, 0, 50, 50), "LEFT"))
		{
			shiftLeft();
		}
		
		if(GUI.Button(new Rect(Screen.width-50, 0, 50, 50), "Right"))
		{
			shiftRight();
		}
	}
	
	bool isRecord()
	{
		for(int i = 0 ; i < recordNum ; i++)
		{
			if(wordRecords[i].word == wordList[recordNow])
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void shiftLeft()
	{
		recordNow--;
		if(recordNow < 0) recordNow += totalNum;
	}
	
	public void shiftRight()
	{
		recordNow++;
		recordNow = recordNow % totalNum;
	}
}

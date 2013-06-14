using UnityEngine;
using System.Collections;

public class g_UI : MonoBehaviour {
	
	public GlobalRecord rcd;
	
	
	public Material unknow;
	public Texture2D unknowT;
	public GUITexture wordView;
	public GUIText recordsView;
	public GUIText avgView;
	
	private int totalNum;
	private int recordNum;
	private int recordNow;
	private System.Collections.Generic.List<string> wordList;
	private wordRecord[] wordRecords;
	
	private bool loaded;
	private bool isdown;
	private Vector3 frontPos;
	
	// Use this for initialization
	void Start () 
	{
		rcd = GameObject.Find("GlobalRecord").GetComponent<GlobalRecord>();
		wordRecords = rcd.records;
			
		wordList  = rcd.database.getWords();
		totalNum  = wordList.Count;
		recordNum = wordRecords.Length;
		recordNow = 0;
		
		loaded = false;
		isdown = false;
		
		//init size
		float w = Screen.width*3/5;
		wordView.pixelInset = new Rect(-w*0.5f, -w*0.5f, w, w);
		print("w=" + Screen.width);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!loaded){
			if(isRecord() == true){
				Word word = rcd.database.getWord(wordList[recordNow]);
				wordView.texture = word.image;
				
				wordRecord wrcd = rcd.getRecord(word.wordName);
				avgView.text = "Proficiency " + wrcd.avgScore().ToString();
				
				wrcd = rcd.database.getOrderedRecords(word);
				recordsView.text  = wrcd.records[0].score.ToString("000.00") + " | " + wrcd.records[0].time;
				int i;
				for(i=1;i<wrcd.records.Count;i++){
					recordsView.text += "\n" + wrcd.records[i].score.ToString("000.00") + " | " + wrcd.records[i].time;
				}
				while(i < 5){
					recordsView.text += "\n----.-- | -";
					i++;
				}
			}
			else{
				wordView.texture = unknowT;
				avgView.text = "0.0";
				recordsView.text = "----.-- | -\n----.-- | -\n----.-- | -\n----.-- | -\n----.-- | -";
			}
		}
		loaded = true;
		
		if(Input.touchCount > 0){
			Touch t = Input.GetTouch(0);
			//finger cross half(0.5) screen width in 0.2 sec than change page
			if(t.deltaPosition.x/Time.deltaTime > Screen.width*0.5f / 0.2f ) shiftRight();
			else if(t.deltaPosition.x/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftLeft();
			else if(t.deltaPosition.y/Time.deltaTime < -Screen.width*0.5f / 0.2f){
				
			}
		}
		else{
			if(isdown){
				Vector2 d = Input.mousePosition-frontPos;
				if(d.x/Time.deltaTime > Screen.width*0.5f / 0.2f) shiftRight();
				else if(d.x/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftLeft();
				else{
					frontPos = Input.mousePosition;
				}
				
			}
			else{
				if(Input.GetMouseButtonDown(0)){
					frontPos = Input.mousePosition;
					isdown = true;
					print("isDown");
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(Input.multiTouchEnabled){
			
		}
		else {
			if(GUI.Button(new Rect(0, 0, 50, 50), "LEFT")){
				shiftLeft();
			}
			
			if(GUI.Button(new Rect(Screen.width-50, 0, 50, 50), "Right")){
				shiftRight();
			}
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
		loaded = false;
		recordNow--;
		if(recordNow < 0) recordNow += totalNum;
		isdown = false;
	}
	
	public void shiftRight()
	{
		loaded = false;
		recordNow++;
		recordNow = recordNow % totalNum;
		isdown = false;
	}
	
	private void shiftUp(){
		isdown = false;
	}
	
	private void shiftDown(){
		isdown = false;
	}
}

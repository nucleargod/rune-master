using UnityEngine;
using System.Collections;

public class g_UI : MonoBehaviour {
	
	public GlobalRecord rcd;
	
	public Material unknow;
	public Texture2D unknowT;
	
	public GUITexture wordView;
	public GUIText recordsView;
	public GUIText timeView;
	public GUIText avgView;
	
	private Vector3 wordViewPos;
	private Vector3 recordsViewPos;
	private Vector3 timeViewPos;
	private Vector3 avgViewPos;
	
	public float shiftSpeed;
	private Vector3 shiftRate;
	private Vector2 wordViewNW;
	
	private int totalNum;
	private int recordNum;
	private int recordNow;
	private System.Collections.Generic.List<string> wordList;
	private wordRecord[] wordRecords;
	
	private bool loaded;
	private bool isdown;
	private bool istouch;
	private Vector3 frontPos;
	
	public GUITexture LRarrowT;
	
	// Use this for initialization
	void Start () 
	{
		rcd = GameObject.Find("GlobalRecord").GetComponent<GlobalRecord>();
		wordRecords = rcd.records;
		
		wordViewPos = wordView.transform.position;
		recordsViewPos = recordsView.transform.position;
		timeViewPos = timeView.transform.position;
		avgViewPos = avgView.transform.position;
		
		shiftRate = Vector3.zero;
		
		wordList  = rcd.database.getWords();
		totalNum  = wordList.Count;
		recordNum = wordRecords.Length;
		recordNow = 0;
		
		loaded = false;
		isdown = false;
		istouch = false;
		
		//init size
		float w = Screen.width*3/5;
		wordView.pixelInset = new Rect(-w*0.5f, -w*0.5f, w, w);
		//print("w=" + Screen.width);
		wordViewNW = new Vector2(w/Screen.width, w/Screen.height);
		
		w = Screen.width/3f;
		LRarrowT.pixelInset = new Rect(-w*0.5f, -w*0.25f, w, w*0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!loaded){
			if(isRecord() == true){
				Word word = rcd.database.getWord(wordList[recordNow]);
				wordView.texture = word.image;
				
				wordRecord wrcd = rcd.getRecord(word.wordName);
				avgView.text = "精準度：" + wrcd.avgScore().ToString();
				
				wrcd = rcd.database.getOrderedRecords(word);
				recordsView.text = wrcd.records[0].score.ToString("000.00");
				timeView.text = wrcd.records[0].time.ToString("MM-dd HH:mm");
				int i;
				for(i=1;i<wrcd.records.Count;i++){
					recordsView.text += "\n" + wrcd.records[i].score.ToString("000.00");
					timeView.text    += "\n" + wrcd.records[i].time.ToString("MM-dd HH:mm");
				}
				while(i < 5){
					recordsView.text += "\n----.--";
					timeView.text    += "\n-";
					i++;
				}
			}
			else{
				wordView.texture = unknowT;
				avgView.text = "精準度：0.0";
				recordsView.text = "----.--\n----.--\n----.--\n----.--\n----.--";
				timeView.text    = "-\n-\n-\n-\n-";
			}
		}
		loaded = true;
		
		//shift
		if(shiftRate != Vector3.zero){
			Vector3 fpos = wordView.transform.position;
			wordView.transform.Translate(shiftRate*Time.deltaTime);
			recordsView.transform.Translate(shiftRate*Time.deltaTime);
			timeView.transform.Translate(shiftRate*Time.deltaTime);
			avgView.transform.Translate(shiftRate*Time.deltaTime);
			
			if(wordView.transform.position.x > 1 + wordViewNW.x*0.5){
				loaded = false;
				float delta = -wordViewNW.x-1;
				wordView.transform.Translate(delta, 0.0f, 0.0f);
				recordsView.transform.Translate(delta, 0.0f, 0.0f);
				timeView.transform.Translate(delta, 0.0f, 0.0f);
				avgView.transform.Translate(delta, 0.0f, 0.0f);
			}
			else if(wordView.transform.position.x < -wordViewNW.x*0.5f){
				loaded = false;
				float delta = wordViewNW.x+1;
				wordView.transform.Translate(delta, 0.0f, 0.0f);
				recordsView.transform.Translate(delta, 0.0f, 0.0f);
				timeView.transform.Translate(delta, 0.0f, 0.0f);;
				avgView.transform.Translate(delta, 0.0f, 0.0f);
			}
			//y direction shift
			else{
				Vector3 deltaf = fpos - wordViewPos;
				Vector3 deltab = (wordView.transform.position - wordViewPos);
				if( deltaf.x * deltab.x < 0){
					wordView.transform.position = wordViewPos;
					recordsView.transform.position = recordsViewPos;
					timeView.transform.position = timeViewPos;
					avgView.transform.position = avgViewPos;
					shiftRate = Vector3.zero;
				}
				//y?
			}
		}
		
		//input
		if(Input.multiTouchEnabled){//for multiTouch
			if(isdown){
				if(Input.touchCount < 0) isdown = false;
				else{
					Touch t = Input.GetTouch(0);
					//finger cross half(0.5) screen width in 0.2 sec than change page
					if(t.deltaPosition.x/Time.deltaTime > Screen.width*0.5f / 0.2f ) shiftRight();
					else if(t.deltaPosition.x/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftLeft();
					else if(t.deltaPosition.y/Time.deltaTime > Screen.width*0.5f / 0.2f) shiftUp();
					else if(t.deltaPosition.y/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftDown();
				}
			}
			else if(Input.touchCount > 0 && !istouch){
				isdown = true;
				istouch = true;
			}
			
			if(Input.touchCount == 0){
				istouch = false;
				isdown = false;
			}
		}
		else{//for mouse
			if(isdown){
				Vector2 d = Input.mousePosition-frontPos;
				if(d.x/Time.deltaTime > Screen.width*0.5f / 0.2f) shiftRight();
				else if(d.x/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftLeft();
				else if(d.y/Time.deltaTime > Screen.width*0.5f / 0.2f) shiftUp();
				else if(d.y/Time.deltaTime < -Screen.width*0.5f / 0.2f) shiftDown();
				else{
					frontPos = Input.mousePosition;
				}
				
				if(isdown && Input.GetMouseButtonUp(0)) isdown = false;
			}
			else{
				if(Input.GetMouseButtonDown(0)){
					frontPos = Input.mousePosition;
					isdown = true;
				}
			}
		}
	}
	
	void OnGUI()
	{
		
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
		//loaded = false;
		recordNow--;
		if(recordNow < 0) recordNow += totalNum;
		
		shiftRate = new Vector3(-shiftSpeed, 0.0f, 0.0f);
		/*wordView.transform.position += shiftRate;
		recordsView.transform.position += shiftRate;
		timeView.transform.position += shiftRate;
		avgView.transform.position += shiftRate;
		//*/
		isdown = false;
	}
	
	public void shiftRight()
	{
		//loaded = false;
		recordNow++;
		recordNow = recordNow % totalNum;
		
		shiftRate = new Vector3(shiftSpeed, 0.0f, 0.0f);
		/*wordView.transform.position += shiftRate;
		recordsView.transform.position += shiftRate;
		timeView.transform.position += shiftRate;
		avgView.transform.position += shiftRate;
		//*/
		isdown = false;
	}
	
	private void shiftUp(){
		isdown = false;
		
	}
	
	private void shiftDown(){
		isdown = false;
		Application.LoadLevel("menuScene");
	}
}

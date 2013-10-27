using UnityEngine;
using System.Collections;
using System.IO;

public class game_ui : MonoBehaviour {
	
	public Canvas canvas;
	public Word frontWord;
	public Word backWord;
	public Word wordPlayer;
	public WordDisplay wordDisplay;
	public WordDisplay backWordDisplay;
	public Word[] wordList;
	public float error;
	public float showE;
	
	public int chooseWords;
	public int length;
	
	public Object sound_O;
	public Object sound_X;
	
	private model db;
	private GlobalRecord rcd;
	
	public GameObject emitter;
	private int backDisplayPos;	//-1:stop, 0:start, >0:frame
	private float playedTimer;
	private bool toggleWaterMark;
	
	public Material defaultMat;
	public Shader blender;
	public MeshRenderer canvasRenderer;
	private Material blenderMat;
	
	public GUIStyle sbclear;
	public GUIStyle sbreturn;
	public GUIStyle sbteach;
	public GUIStyle sbscore;
	public Texture2D promptT;
	public Texture2D unpromptT;
	
	void LoadWords()
	{
		wordList = new Word[rcd.wordList.Count];
		for(int i=0; i<rcd.wordList.Count; i++){
			wordList[i] = db.getWord(rcd.wordList[i]);
		}
		
		//chooseWords = Random.Range(0,rcd.wordList.Count);
		chooseWords = Global.Instants.seletedTheme*5+Global.Instants.seletedChapter;
		backWord = wordList[chooseWords];
		
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
		
		wordPlayer = new Word();
		backWordDisplay.SetTarget(wordPlayer);
		backDisplayPos = -1;
		
		//prepare shader
		/*Material[] tmp = canvasRenderer.materials;
		//Material tmpM = new Material(Shader.Find("Tranparent/Diffuse"));
		//tmpM.color = new Color(0,0,0,
		tmp.SetValue(backWord.mat_t,1);
		tmp[1].SetColor("_Color", new Color(0,0,0,0.5f));
		canvasRenderer.materials = tmp;//*/
	}
	
	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = canvas.frontDisplay;
		backWordDisplay = canvas.backDisplay;
		showE = 0.0f;
		showError = true;
		playedTimer = 0.0f;
		toggleWaterMark = false;
		
		// get global record and database
		//GameObject o = GameObject.Find("GlobalRecord");
		//rcd = o.GetComponent<GlobalRecord>();
		rcd = DataManager.Instants.recordComponent;
		//o = GameObject.Find("database");
		//db = o.GetComponent<model>();
		db = DataManager.Instants.modelComponent;
			
		// load words
		LoadWords();
		
		check = false;
	}
	
	bool check;
	// Update is called once per frame
	void Update () 
	{
		if(frontWord != null && backWord != null)
		{
			if((Input.multiTouchEnabled && Input.touchCount == 0) || 
				Input.GetMouseButtonUp(0)) {
				error = frontWord.GetError(backWord);
			}
		}
		
		if(check == false && frontWord.finishIndex >= backWord.finishIndex)
		{
			//changeWord();
			showE = error;
			/*if(Word.Judge(showE) != "Fail")
			{
				Object snd = Instantiate(sound_O);
				Destroy(snd, 1);
			}
			else
			{
				Object snd = Instantiate(sound_X);
				Destroy(snd, 1);
			}*/
			
			check = true;
			//ClearCanvas();
		}
		
		if(playedTimer > 0.0f){
			//print("暫停：" + playedTimer);
			playedTimer -= Time.deltaTime;
			if(playedTimer <= 0.0f){
				playedTimer = 0.0f;
				wordPlayer = new Word();
				backWordDisplay.SetTarget(wordPlayer);
				backDisplayPos = -1;
			}
		}
		else if(backDisplayPos >= 0){
			int strokId = backDisplayPos / Word.pointPerStroke;
			int pointId = backDisplayPos % Word.pointPerStroke;
			//print("示範：" + strokId + "," + pointId);
			if(strokId != backWord.finishIndex){
				//示範書寫
				if(pointId == 0){wordPlayer.BeginWriting();}
				Vector3 p = (Vector3)((Stroke)backWord.strokeList[strokId]).pointList[pointId];
				emitter.transform.localPosition = p * canvas.canvasSize;
				emitter.particleSystem.Emit(1);
				wordPlayer.Writing( p );
				backDisplayPos++;
				if(pointId + 1 == Word.pointPerStroke){
					wordPlayer.EndWriting();
					//print("endWriting");
				}
			}
			else{
				emitter.transform.position = Vector3.zero;
				playedTimer = 5.0f;
			}
		}
	}
	
	void OnGUI()
	{
		float W  = Screen.width/720.0f*200.0f;
		float W2 = Screen.width/720.0f*100.0f;
		
		if(GUI.Button( new Rect(Screen.width-W, 0, W, W2), "", sbclear))
		{
			check = false;
			ClearCanvas();
		}		
		if(GUI.Button( new Rect(Screen.width-W, W2, W, W2), "", sbreturn))
		{
			SceneManager.GoTo(SceneList.chapterMenu);
			//Application.LoadLevel("menuScene");
		}
		
		ShowError(W);
		
		if(GUI.Button(new Rect(0, 0, W, W), wordList[chooseWords].image, GUIStyle.none)){
			ClearCanvas();
			// changeWord();
		}
		
		if(GUI.Button(new Rect(0, Screen.height - W, W, W2), "", sbteach)){
			backDisplayPos = 0;
		}
		
		//浮水印
		Texture2D pt = promptT;
		if(toggleWaterMark) pt = unpromptT;
		if(GUI.Button(new Rect(0, Screen.height - W2, W, W2), pt, GUIStyle.none)){
			if(toggleWaterMark){
				toggleWaterMark = false;
				Material[] tmp = canvasRenderer.materials;
				tmp.SetValue(defaultMat, 1);
				canvasRenderer.materials = tmp;
			}
			else{
				toggleWaterMark = true;
				changeWaterMark(backWord.image_t);
			}
		}
	}
	
	private bool showError;
	void ShowError(float W) 
	{
		if(!showError) return;
		
		if(Word.getScore(showE) != 500.0f)
		{
			Global.Instants.battleResult = Word.getScore(showE);
			SceneManager.GoTo(SceneList.result);
		}
		
		GUI.Box(new Rect(Screen.width-W, Screen.height-W, W, W), 
						"score " + Word.getScore(showE).ToString("0.000") + "\nRank: " + Word.Judge(showE), 
						sbscore);
	}
	
	public void ClearCanvas()
	{
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	private void changeWord(){
		chooseWords++;
		if(chooseWords >= wordList.Length) chooseWords = 0;
		backWord = wordList[chooseWords];
		if(toggleWaterMark){
			changeWaterMark(backWord.image_t);
		}
		//blenderMat.SetTexture("_ColorBuffer", backWord.image_t);
	}
	
	private void changeWaterMark(Texture2D tex){
		Material[] tmp = canvasRenderer.materials;
		Material t = new Material(Shader.Find("Transparent/Diffuse"));
		t.mainTexture = tex;
		t.color = new Color(0,0,0,0.2f);
		tmp.SetValue(t,1);
		canvasRenderer.materials = tmp;
	}
	/*
	public int[] shuffle()
	{
		int []nums = new int[length];
		for(int i = 0 ; i < length ; i++)
			nums[i] = i;
		
		for(int j = 0 ; j < 500 ; j++)	
		{
			int a = Random.Range(0, length);
			int b = Random.Range(0, length);
			int tmp = nums[a];
			nums[a] = nums[b];
			nums[b] = tmp;
		}
		
		return nums;
	}//*/
}

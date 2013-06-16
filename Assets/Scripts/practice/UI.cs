using UnityEngine;
using System.Collections;
using System.IO;

public class UI : MonoBehaviour {
	
	public Canvas canvas;
	public Word frontWord;
	public Word backWord;
	public WordDisplay wordDisplay;
	public WordDisplay backWordDisplay;
	public Word[] wordList;
	public float error;
	public float showE;
	
	//private Texture2D []img2D;
	public int chooseWords;
	public int length;
	
	public Object sound_O;
	public Object sound_X;
	
	private model db;
	private GlobalRecord rcd;
	
	private bool toggleBackDisplay;
	private bool toggleWaterMark;
	
	public Material defaultMat;
	public Shader blender;
	public MeshRenderer canvasRenderer;
	private Material blenderMat;
	
	void LoadWords()
	{
		wordList = new Word[rcd.wordList.Count];
		for(int i=0; i<rcd.wordList.Count; i++){
			wordList[i] = db.getWord(rcd.wordList[i]);
		}
		
		chooseWords = Random.Range(0,rcd.wordList.Count);
		backWord = wordList[chooseWords];
		
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
		
		//prepare shader
		blenderMat = new Material(blender);
		blenderMat.mainTexture = defaultMat.mainTexture;
		blenderMat.SetTexture("_ColorBuffer", backWord.image_t);
		blenderMat.SetPass(1);//*/
		//canvasRenderer.materials[1] = backWord.mat_t;
	}
	
	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = canvas.frontDisplay;
		backWordDisplay = canvas.backDisplay;
		showE = 0.0f;
		showError = true;
		
		// get global record and database
		GameObject o = GameObject.Find("GlobalRecord");
		rcd = o.GetComponent<GlobalRecord>();
		o = GameObject.Find("database");
		db = o.GetComponent<model>();
			
		// load words
		LoadWords();
	}
	
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
		
		if(frontWord.finishIndex >= backWord.finishIndex)
		{
			changeWord();
			showE = error;
			if(Word.Judge(showE) != "Fail")
			{
				Object snd = Instantiate(sound_O);
				Destroy(snd, 1);
			}
			else
			{
				Object snd = Instantiate(sound_X);
				Destroy(snd, 1);
			}
			
			ClearCanvas();
		}
	}
	
	void OnGUI()
	{
		float W  = Screen.width/720.0f*200.0f;
		float W2 = Screen.width/720.0f*100.0f;
		
		if(GUI.Button( new Rect(Screen.width-W, 0, W, W2), "Clear Canvas"))
		{
			ClearCanvas();
		}		
		if(GUI.Button( new Rect(Screen.width-W, W2, W, W2), "Menu"))
		{
			Application.LoadLevel("menuScene");
		}
		
		ShowError();
		
		if(GUI.Button(new Rect(0, 0, W, W), wordList[chooseWords].image, new GUIStyle())){
			ClearCanvas();
			changeWord();
		}
		toggleBackDisplay = GUI.Toggle(new Rect(0, Screen.height - W, W, W2), toggleBackDisplay, "開啟提示");
		if(!toggleBackDisplay && backWordDisplay.hasTarget()){
			backWordDisplay.SetTarget(null);
		}
		else if(toggleBackDisplay && !backWordDisplay.hasTarget()){
			print("setTarget");
			backWordDisplay.SetTarget(backWord);
		}
	}
	
	private bool showError;
	void ShowError() 
	{
		if(!showError) return;
		
		GUI.Box(new Rect(Screen.width * 0.8f , 
						 Screen.height* 0.9f , 
						 Screen.width / 5.0f , 
						 Screen.height/10.0f), "Error " + showE.ToString("0.000") + "\nRank: " + Word.Judge(showE));
	}
	
	public void ClearCanvas()
	{
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	private void changeWord(){
		chooseWords = Random.Range(0, wordList.Length);
		backWord = wordList[chooseWords];
		backWordDisplay.SetTarget(null);
		//canvasRenderer.materials[1] = backWord.mat_t;
		//canvasRenderer.Render(1);
		blenderMat.SetTexture("_ColorBuffer", backWord.image_t);
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

using UnityEngine;
using System.Collections;
using System.IO;

public class UI : MonoBehaviour {
	
	public Canvas canvas;
	public Word frontWord;
	public Word backWord;
	public WordDisplay wordDisplay;
	public WordDisplay backWordDisplay;
	public ArrayList wordList = new ArrayList();
	public float error;
	public float showE;
	private float a;
	
	private int wordIdx = 1;
	private CameraControl camCtrl;
	public bool isSelectWord;
	public bool isFight;
	
	private Texture2D []img2D;
	public int []chooseWords;
	public int length;
	
	void LoadWords()
	{
		Object []wordsInfo = Resources.LoadAll("WordsInfo");
		length = wordsInfo.Length;
		string []filename = new string[length];
		for(int i = 0 ; i < length ; i++)
			filename[i] = wordsInfo[i].name;
		
		for(int n = 0 ; n < filename.Length ; n++)
		{
			StringReader sr = new StringReader(((TextAsset)wordsInfo[n]).text);
			
			Word rWord = new Word();
			rWord.wordName = filename[n];
			
			int strokeNum = int.Parse(sr.ReadLine());
			rWord.finishIndex = strokeNum;
			
			int count = 0;
			Stroke s = new Stroke();
			while(count < strokeNum)
			{
				string line = sr.ReadLine();
				if(line == "Stroke End")
				{
					count++;
					rWord.strokeList.Add(s);
					s = new Stroke();
					continue;
				}
				
				string []split = line.Split(new char[]{' '});
				Vector3 p = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
				s.pointList.Add(p);
			}
			
			wordList.Add(rWord);
			sr.Close();
		}
		
		Object[]textures = Resources.LoadAll("Words");
		img2D = new Texture2D[textures.Length];
		
		for(int i = 0 ; i < textures.Length ; i++)
			img2D[i] = (Texture2D)textures[i];
		
		
		int []shuffleNum = new int[length];
		shuffleNum = shuffle();
		chooseWords = new int[4];
		for(int i = 0 ; i < 4 ; i++)
			chooseWords[i] = shuffleNum[i];
		
		
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = GameObject.Find("Displayer").GetComponent<WordDisplay>();
		backWordDisplay = GameObject.Find("BackDisplayer").GetComponent<WordDisplay>();
		showE = 0.0f;
			
		// load words
		LoadWords();
		
		camCtrl = GameObject.Find("Camera").GetComponent<CameraControl>();
		isSelectWord = false;
		isFight = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(frontWord != null && backWord != null)
		{
			showError = true;
			error = frontWord.GetError(backWord);
		}
		else
		{
			showError = false;
			error = a;
		}
	}
	
	public void ClearCanvas()
	{
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	void OnGUI () {
		float W  = Screen.width/720.0f*200.0f;
		float W2 = Screen.width/720.0f*100.0f;
		
		if(GUI.Button( new Rect(Screen.width-W,  0, W, W2), "New Word"))
		{
			canvas.word.wordName = "word " + (wordIdx++).ToString();
			wordList.Add(canvas.word);
			ClearCanvas();
		}
		if(GUI.Button( new Rect(Screen.width-W, W2, W, W2), "Clear Canvas"))
		{
			ClearCanvas();
		}
		
		if(!isSelectWord)
		{
			ShowError();
			
			for(int i = 0 ; i < 4 ; i++)
			{
				Word word = (Word)wordList[chooseWords[i]];
				if( GUI.Button(new Rect(0, i*W, W, W), (Texture)img2D[chooseWords[i]]))
				{
					backWord = word;
					//backWordDisplay.SetTarget(backWord);
				
					camCtrl.des = new Vector3(0, 1, -50);
					isSelectWord = true;
					isFight = false;
				}
			}
		}
	}
	
	private bool showError;
	private Vector2 scrollPosition2;
	void ShowError() 
	{
		if(!showError) return;
		/*
		GUILayout.BeginArea( new Rect(Screen.width-200, 100, 200, Screen.height - 100), "");
		scrollPosition2 = 
		GUILayout.BeginScrollView ( scrollPosition2, GUILayout.Width(200), GUILayout.Height(Screen.height - 100) );
		for(int i = 0; i < frontWord.strokeList.Count; i++)
		{
			if(i >= backWord.strokeList.Count) break;
			GUILayout.BeginHorizontal();
			GUILayout.Box(((Stroke)frontWord.strokeList[i]).GetError((Stroke)backWord.strokeList[i]).ToString("0.000"));
			GUILayout.Box(((Stroke)frontWord.strokeList[i]).GetDotError((Stroke)backWord.strokeList[i]).ToString("0.000"));
			GUILayout.Box(((Stroke)frontWord.strokeList[i]).GetSlopeError((Stroke)backWord.strokeList[i]).ToString("0.000"));
			GUILayout.EndHorizontal();
		}
		GUILayout.Box("Error "+error.ToString("0.000"));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
		*/
		GUI.Box(new Rect(Screen.width * 0.8f , 
						 Screen.height* 0.9f , 
						 Screen.width / 5.0f , 
						 Screen.height/10.0f), "Error " + showE.ToString("0.000"));
	}
	
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
	}
}

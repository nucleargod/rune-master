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
	
	private Texture2D []img2D;
	public int chooseWords;
	public int length;
	
	public Object sound_O;
	public Object sound_X;
	
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
		chooseWords = shuffleNum[0];
		Word word = (Word)wordList[chooseWords];
		backWord = word;
		
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = GameObject.Find("Displayer").GetComponent<WordDisplay>();
		backWordDisplay = GameObject.Find("BackDisplayer").GetComponent<WordDisplay>();
		showE = 0.0f;
		showError = true;
			
		// load words
		LoadWords();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(frontWord != null && backWord != null)
		{
			error = frontWord.GetError(backWord);
		}
		else
		{
			//showError = false;
			error = a;
		}
		
		if(frontWord.finishIndex >= backWord.finishIndex)
		{
			int []shuffleNum = new int[length];
			shuffleNum = shuffle();
			chooseWords = shuffleNum[0];
			Word word = (Word)wordList[chooseWords];
			backWord = word;
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
		
		GUI.Button(new Rect(0, 0, W, W), (Texture)img2D[chooseWords]);
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

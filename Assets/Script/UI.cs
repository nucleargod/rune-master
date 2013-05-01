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
	private float a;
	
	private int wordIdx = 1;
	private CameraControl camCtrl;
	public bool isSelectWord;
	public bool isFight;
	
	void LoadWords()
	{
		string []filename = {"metal",
						     "wood" ,
						     "water",
						     "fire" ,
						     "earth"};
		
		for(int n = 0 ; n < filename.Length ; n++)
		{
			StreamReader sr = new StreamReader("./Assets/resource/" + filename[n] + ".txt");
			
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
		
		canvas.word = new Word();
		frontWord = canvas.word;
		wordDisplay.SetTarget(frontWord);
	}
	
	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = GameObject.Find("Displayer").GetComponent<WordDisplay>();
		backWordDisplay = GameObject.Find("BackDisplayer").GetComponent<WordDisplay>();
		
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
	
	private Vector2 scrollPosition;
	void OnGUI () {
		if(GUI.Button( new Rect(Screen.width-100,  0, 100, 50), "New Word"))
		{
			canvas.word.wordName = "word " + (wordIdx++).ToString();
			wordList.Add(canvas.word);
			ClearCanvas();
		}
		if(GUI.Button( new Rect(Screen.width-100, 50, 100, 50), "Clear Canvas"))
		{
			ClearCanvas();
		}
		
		if(!isSelectWord)
		{
			ShowError();
			
			scrollPosition = 
			GUILayout.BeginScrollView ( scrollPosition, GUILayout.Width(100), GUILayout.Height(Screen.height) );
			foreach( Word word in wordList )
			{
				if( GUILayout.Button(word.wordName))
				{
					// save words
					/*StreamWriter sw = new StreamWriter("./Assets/resource/haha.txt");
					sw.WriteLine(word.finishIndex);
					for(int i = 0 ; i < word.strokeList.Count ; i++)
					{
						Stroke s = (Stroke)word.strokeList[i];
						for(int j = 0 ; j < s.pointList.Count ; j++)
						{
							Vector3 v = (Vector3)s.pointList[j];
							string line = "";
							line = v.x.ToString() + " " + v.y.ToString() + " " + v.z.ToString();
							sw.WriteLine(line);
						}
					
						sw.WriteLine("Stroke End");
					}
					sw.Close();*/
					
					backWord = word;
					backWordDisplay.SetTarget(backWord);
				
					camCtrl.des = new Vector3(0, 1, -50);
					isSelectWord = true;
					isFight = false;
				}
			}
			GUILayout.EndScrollView();
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
						 Screen.height/10.0f), "Error " + error.ToString("0.000"));
	}
}

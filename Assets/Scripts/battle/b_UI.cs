using UnityEngine;
using System.Collections;
using System.IO;

public class b_UI : MonoBehaviour {
	
	public Canvas canvas;
	public Word frontWord;
	public Word backWord;
	public WordDisplay wordDisplay;
	public WordDisplay backWordDisplay;
	public ArrayList wordList = new ArrayList();
	public float error;
	public float showE;
	private float a;
	
	public bool isGameOver;
	
	private b_CameraControl camCtrl;
	private b_BattleSystem battle;
	public bool isSelectWord;
	public bool isFight;
	public string mode;
	public int selectWordIdx;
	public Vector2 selectWordPos;
	public Vector2 []chooseWordPos;
	
	private Texture2D []img2D;
	public int []chooseWords;
	public int length;
	public int levelNum;
	public int levelNow;
	
	public Object snd_Pass;
	public Object snd_Fail;
	public Object pass;
	public Object Fail;
	
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
			
			sr.ReadLine();	// property
			sr.ReadLine();	// ATK
			
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
	void Start () 
	{
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = GameObject.Find("Displayer").GetComponent<WordDisplay>();
		backWordDisplay = GameObject.Find("BackDisplayer").GetComponent<WordDisplay>();
		showE = 0.0f;
			
		isGameOver = false;
			
		// load words
		LoadWords();
		
		camCtrl = GameObject.Find("Camera").GetComponent<b_CameraControl>();
		battle = GameObject.Find("Camera").GetComponent<b_BattleSystem>();
		
		chooseWordPos = new Vector2[4];
		// initial
		Initial();
		battle.monster.transform.position = new Vector3(0, 83.24539f, 0);
		
		levelNum = 5;
		levelNow = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
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
	
	void OnGUI ()
	{
		float W  = Screen.width/4.0f;
		float W2 = Screen.width/8.0f;
		
		int eHP = (int)battle.HP_enemy_now;
		if(eHP < 0) eHP = 0;
		GUI.Box(new Rect(Screen.width-W, 0, W, W2), mode);
		GUI.Box(new Rect(Screen.width/2.0f - W/2.0f, 				0, W, W2), ((int)(eHP)).ToString());
		GUI.Box(new Rect(Screen.width/2.0f - W/2.0f, Screen.height-W2, W, W2), ((int)(battle.HP_player_now)).ToString());
		
		if(isGameOver)
		{
			if(battle.HP_player_now == 0 && !Fail)
			{
				Fail = Instantiate(snd_Fail);
			}
			else
			{
				int eHP2 = (int)battle.HP_enemy_now;
				if(eHP2 < 0) eHP2 = 0;
				
				if(eHP2 == 0 && !pass)
				{
					pass = Instantiate(snd_Pass);
				}
			}
			
			if(GUI.Button(new Rect(Screen.width/4.0f, Screen.height/4.0f, Screen.width/2.0f, Screen.height/4.0f), "Munu"))
				Application.LoadLevel("menuScene");
			
			if(GUI.Button(new Rect(Screen.width/4.0f, Screen.height/2.0f, Screen.width/2.0f, Screen.height/4.0f), "Retry"))
				Application.LoadLevel("battleScene");
			
			return;
		}
		
		if(!isSelectWord)
		{
			ShowError();
			
			for(int i = 0 ; i < 4 ; i++)
			{
				Word word = (Word)wordList[chooseWords[i]];
				
				Vector2 tmp = chooseWordPos[i];
				chooseWordPos[i] = Vector2.MoveTowards(tmp, new Vector2(i*W, Screen.height/2.0f-W/2.0f), Time.deltaTime*700);
				
				if(Vector2.Distance(selectWordPos, new Vector2(i*W, Screen.height/2.0f-W/2.0f)) <= 1.0f)
					chooseWordPos[i] = new Vector2(i*W, Screen.height/2.0f-W/2.0f);
				
				if(mode == "Attack mode")
				{
					if(GUI.Button(new Rect(tmp.x, tmp.y, W, W), (Texture)img2D[chooseWords[i]]))
					{
						backWord = word;
						//backWordDisplay.SetTarget(backWord);
						
						camCtrl.des = new Vector3(0, 1, -50);
						isSelectWord = true;
						isFight = false;
						selectWordIdx = i;
						selectWordPos = new Vector2(i*W, Screen.height/2.0f-W/2.0f);
					}
				}
				else
				{
					GUI.Button(new Rect(tmp.x, tmp.y, W, W), (Texture)img2D[chooseWords[i]]);
						
					if(battle.defenseTime > 0.0f)
						battle.defenseTime -= Time.deltaTime/15.0f;
					
					if(battle.defenseTime < 0.0f)
					{
						battle.defenseTime = 0.0f;
						
						int j = Random.Range(0, 4);
						backWord = (Word)wordList[chooseWords[j]];
						camCtrl.des = new Vector3(0, 1, -50);
						isSelectWord = true;
						isFight = false;
						selectWordIdx = j;
						selectWordPos = new Vector2(j*W, Screen.height/2.0f-W/2.0f);
					}
				}
			}
		}
		else
		{
			Vector2 tmp = selectWordPos;
			selectWordPos = Vector2.MoveTowards(tmp, new Vector2(0, 0), Time.deltaTime*400);
			
			if(Vector2.Distance(selectWordPos, new Vector2(0, 0)) <= 1.0f)
				selectWordPos = new Vector2(0, 0);
			
			GUI.DrawTexture(new Rect(tmp.x, tmp.y, W, W), (Texture)img2D[chooseWords[selectWordIdx]]);
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
	
	public void Initial()
	{
		for(int i = 0 ; i < 4 ; i++)
			chooseWordPos[i] = new Vector2(Screen.width/2.0f, Screen.height*1.2f);
		
		isSelectWord = false;
		isFight = true;
		mode = "Attack mode";
		selectWordIdx = -1;
		battle.monster.transform.position = new Vector3(0, 500, 0);
	}
}

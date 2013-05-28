using UnityEngine;
using System.Collections;
using System.IO;

public class t_UI : MonoBehaviour {
	
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
	
	private t_BattleSystem battle;
	private HorizontalBar barHP;
	public int isSelectWord;
	public bool isFight;
	public string mode;
	
	public Material []wordMat;
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
		// Words txt
		// *****************
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
			
			int strokeNum = int.Parse(sr.ReadLine());	// stroke number
			rWord.finishIndex = strokeNum;
			
			rWord.property = sr.ReadLine();				// property
			rWord.ATK = float.Parse(sr.ReadLine());		// ATK
			
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
		// *****************
		
		
		// Words png
		// *****************
		Object[]textures = Resources.LoadAll("Words");
		img2D = new Texture2D[textures.Length];
		for(int i = 0 ; i < textures.Length ; i++)
			img2D[i] = (Texture2D)textures[i];
		// *****************
		
		
		// Words material
		// *****************
		Object[]materials = Resources.LoadAll("WordMaterials");
		wordMat = new Material[materials.Length];
		for(int i = 0 ; i < materials.Length ; i++)
			wordMat[i] = (Material)materials[i];
		// *****************
		
		
		int []shuffleNum = new int[length];
		shuffleNum = shuffle();
		chooseWords = new int[4];
		for(int i = 0 ; i < 4 ; i++)
			chooseWords[i] = shuffleNum[i];
		
		
		//canvas.word = new Word();
		//frontWord = canvas.word;
		//wordDisplay.SetTarget(frontWord);
	}
	
	// Use this for initialization
	void Start () 
	{
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		wordDisplay = canvas.frontDisplay;
		backWordDisplay = canvas.backDisplay;
		showE = 0.0f;
			
		isGameOver = false;
			
		// load words
		LoadWords();
		
		battle = GameObject.Find("Camera").GetComponent<t_BattleSystem>();
		barHP  = GameObject.Find("Camera").GetComponent<HorizontalBar>();
		
		// initial
		Initial();
		
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
		
		if(isSelectWord == 2)
		{
			isFight = false;
		}
		if(mode == "Attack mode")
		{/*
			if(GUI.Button(new Rect(tmp.x, tmp.y, W, W), (Texture)img2D[chooseWords[i]]))
			{
				backWord = word;
				//backWordDisplay.SetTarget(backWord);
				
				isSelectWord++;
				
				isFight = false;
				selectWordIdx = i;
			}*/
		}
		else
		{
			// attack player
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
		barHP.DrawBarViewer(new Vector3(Screen.width/2.0f, Screen.height*0.995f, 0.0f)
							, battle.HP_enemy_now, battle.HP_enemy_max);
		
		barHP.DrawBarViewer(new Vector3(Screen.width/2.0f, Screen.height/3.0f*1.815f, 0.0f)
							, battle.HP_player_now, battle.HP_player_max);
		
		/*if(isGameOver)
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
		}*/
		
		
		ShowError();
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
		isSelectWord = 0;
		isFight = true;
		mode = "Attack mode";
	}
}
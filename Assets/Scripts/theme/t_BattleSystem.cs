using UnityEngine;
using System.Collections;

public class t_BattleSystem : MonoBehaviour {
	
	private t_UI ui;
	private float bulletTime;
	private int bulletNum;
	private bool isRecover;
	public GameObject monster;
	
	private int []remainWordIdx;
	
	public Word firstWord;
	public Word secondWord;
	
	public Object Sound_O;
	public Object Sound_X;
	public Object Bullet;
	
	public string enemy_property;
	public int HP_player_max;
	public int HP_enemy_max;
	public float HP_player_now;
	public float HP_enemy_now;
	public int HP_player;
	public int HP_enemy;
	public float ATK_player;
	public float ATK_enemy;
	
	// Use this for initialization
	void Start ()
	{
		ui = GameObject.Find("Global").GetComponent<t_UI>();
		monster = GameObject.Find("MonsterWall");
		
		remainWordIdx = new int[4];
		bulletTime = 0.0f;
		isRecover = false;
		
		enemy_property = "fire";
		HP_player_max = 100;
		HP_enemy_max  = 1000;
		HP_player_now = 0;
		HP_enemy_now  = 0;
		ATK_player = 100;
		ATK_enemy  = 45;
		HP_initial();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(bulletTime > 0.0f) bulletTime -= Time.deltaTime;
		
		//HP of enemy
		if(Mathf.Abs(HP_enemy_now - HP_enemy) >= 1.0f)
		{
			if(HP_enemy_now > HP_enemy)
				HP_enemy_now -= Time.deltaTime*500;
			else
				HP_enemy_now += Time.deltaTime*500;
			
			if(Mathf.Abs(HP_enemy_now - HP_enemy) < 1.0f)
				HP_enemy_now = HP_enemy;
		}		
		// HP of player
		if(Mathf.Abs(HP_player_now - HP_player) >= 1.0f)
		{
			if(HP_player_now > HP_player)
				HP_player_now -= Time.deltaTime*500;
			else
				HP_player_now += Time.deltaTime*500;
			
			if(Mathf.Abs(HP_player_now - HP_player) < 1.0f)
				HP_player_now = HP_player;
		}
		
		if(ui.isGameOver) return;
		
		if(ui.isSelectWord == 2)
		{
			if(ui.frontWord.finishIndex >= ui.backWord.finishIndex)
			{
				ui.isSelectWord = 0;
				bulletNum = ui.frontWord.finishIndex;
				bulletTime = 0.15f;
			}
		}
		else
		{
			if(ui.isFight == false)
			{
				if(bulletNum > 0 && bulletTime < 0.0f)
				{
					Fight();
					
					bulletTime = 0.15f;
					bulletNum--;
				}
				
				if(bulletNum == 0)
				{
					ui.isFight = true;
					ui.ClearCanvas();
					
					for(int i = 0 ; i < 4 ; i++)
					{
						remainWordIdx[i] = -1;
						t_Card card = GameObject.Find("Card" + (i+1).ToString()).GetComponent<t_Card>();
						
						if(card.isSelect)
						{
							card.anim.clip = card.anim.GetClip("RotationToNegative");
							card.anim.Play();
						}
						else
							remainWordIdx[i] = ui.chooseWords[i];
					}
					
					if(isRecover)
					{
						HP_player += (int)ui.backWord.ATK;
						if(HP_player > HP_player_max) HP_player = HP_player_max;
					}
				}
			}
		}
		
		// Game over?
		if(HP_enemy == 0)
		{
			HP_enemy = -1;
			HP_enemy_max += 50;
			ui.levelNow += 1;
			ui.Initial();
		}
		
		if(HP_player <= 0 || ui.levelNow > ui.levelNum)
		{
			// game over
			ui.isGameOver = true;
		}
	}
	
	void Fight()
	{
		ui.showE = ui.error;
		
		// Successful
		if(Word.Judge(ui.showE) != "Fail")
		{
			GameObject snd = (GameObject) Instantiate(Sound_O);
			Destroy(snd, 2);
			
			Vector3 pos = (Vector3)(((Stroke)(ui.frontWord.strokeList[ui.frontWord.finishIndex - bulletNum])).pointList[Random.Range(0, 99)]);
			pos -= Vector3.Normalize(pos - ui.gameObject.transform.position)*25.0f;
			
			GameObject bullet = (GameObject)Instantiate(Bullet, pos, Quaternion.identity);
			bullet.GetComponent<t_Bullet>().attackObj = monster;
			
			if(firstWord.property == secondWord.property)
			{
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK / ui.backWord.finishIndex * 2.0f;
				isRecover = true;
			}
			else
			if( (firstWord.property == "water" && secondWord.property == "wood" ) ||
			    (firstWord.property == "wood"  && secondWord.property == "fire" ) ||
			    (firstWord.property == "fire"  && secondWord.property == "earth") ||
			    (firstWord.property == "earth" && secondWord.property == "metal") ||
			    (firstWord.property == "metal" && secondWord.property == "water") )
			{
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK / ui.backWord.finishIndex;
				
				isRecover = true;
			}
			else 
			{
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK / ui.backWord.finishIndex;
				isRecover = false;
			}
				
			
			/*if(HP_enemy <= 0)
				HP_enemy = 0;
			else
			{
				ui.mode = "Defense mode";
			}*/
			
			/*
			else
			{
				switch(Word.Judge(ui.error))
				{
					case "S": HP_player -= (int)(ATK_enemy * 0.5f + Random.Range(-10.0f, 0.0f)); break;
					case "A": HP_player -= (int)(ATK_enemy * 0.7f + Random.Range(-10.0f, 0.0f)); break;
					case "B": HP_player -= (int)(ATK_enemy * 0.8f + Random.Range(-10.0f, 0.0f)); break;
					case "C": HP_player -= (int)(ATK_enemy * 0.9f + Random.Range(-10.0f, 0.0f)); break;
				}
				
				if(HP_player <= 0)
					HP_player = 0;
				else
				{
					ui.mode = "Attack mode";
				}
				
			}*/
		}
		/*
		// Failed
		else
		{
			GameObject snd = (GameObject) Instantiate(Sound_X);
			Destroy(snd, 2);
			
			if(ui.mode == "Attack mode")
			{
				ui.mode = "Defense mode";
			}
			else
			{
				HP_player -= (int)(ATK_enemy * 1.0f + Random.Range(-10.0f, 0.0f));	
				
				if(HP_player <= 0)
					HP_player = 0;
				else
				{
					ui.mode = "Attack mode";
				}
			}
		}*/
	}
	
	void HP_initial()
	{
		HP_player = HP_player_max;
		HP_enemy  = HP_enemy_max;
	}
	
	public void reShuffle()
	{
		int []shuffleNum = new int[ui.length];
		shuffleNum = ui.shuffle();
		ui.chooseWords = new int[4];
		for(int i = 0 ; i < 4 ; i++)
		{
			ui.chooseWords[i] = shuffleNum[i];
			if(remainWordIdx[i] != -1)
				ui.chooseWords[i] = remainWordIdx[i];
		}
	}
}

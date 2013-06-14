using UnityEngine;
using System.Collections;

public class t_BattleSystem : MonoBehaviour {
	
	private t_UI ui;
	private float bulletTime;
	private int bulletNum;
	private bool isRecover;
	public GameObject monster;
	public float monsterCreateTime;
	private const float CreateTime = 3.0f;
	
	private int []remainWordIdx;
	
	public Word firstWord;
	public Word secondWord;
	
	public Object HP_Up;
	public Object HP_Down;
	public Object Sound_O;
	public Object Sound_X;
	public Object Bullet;
	public Object fireBall;
	public Object sparkle;
	public GameObject sparkle_monster;
	
	public string enemy_property;
	public int HP_player_max;
	public int HP_enemy_max;
	public float HP_player_now;
	public float HP_enemy_now;
	public int HP_player;
	public int HP_enemy;
	public float ATK_player;
	public float ATK_enemy;
	
	public bool isFireBall;
	public GameObject []fireBalls;
	public int fireBallNum;
	
	public Material mat_fire_monster;
	public Material mat_earth_monster;
	public Material mat_metal_monster;
	public Material mat_water_monster;
	public Material mat_wood_monster;
	public Material mat_black_monster;
	
	
	// Use this for initialization
	void Start ()
	{
		ui = GameObject.Find("Global").GetComponent<t_UI>();
		monster = GameObject.Find("MonsterWall");
		monster.renderer.material = mat_black_monster;
		monsterCreateTime = CreateTime;
		//sparkle_monster = (GameObject)Instantiate(sparkle);
		//sparkle_monster.transform.position = new Vector3(7.677217f, 76.90249f, 88.65485f);
		
		remainWordIdx = new int[4];
		bulletTime = 0.0f;
		isFireBall = false;
		isRecover = false;
		
		HP_player_max = 1000;
		HP_enemy_max  = 100;
		HP_player_now = 0;
		HP_enemy_now  = 0;
		ATK_player = 100;
		ATK_enemy  = 350;
	}
	
	private int lastCount;
	// Update is called once per frame
	void Update ()
	{
		if(bulletTime > 0.0f) bulletTime -= Time.deltaTime;
		
		//HP of enemy
		if(Mathf.Abs(HP_enemy_now - HP_enemy) >= 5.0f)
		{
			if(HP_enemy_now > HP_enemy)
			{
				HP_enemy_now -= Time.deltaTime*HP_enemy_max/2.0f;
				GameObject snd = (GameObject)Instantiate(HP_Down);
				Destroy(snd, 0.3f);
			}
			else
			{
				HP_enemy_now += Time.deltaTime*HP_enemy_max/2.0f;
				GameObject snd = (GameObject)Instantiate(HP_Up);
				Destroy(snd, 0.3f);
			}
			
			if(Mathf.Abs(HP_enemy_now - HP_enemy) < 5.0f)
				HP_enemy_now = HP_enemy;
		}		
		// HP of player
		if(Mathf.Abs(HP_player_now - HP_player) >= 5.0f)
		{
			if(HP_player_now > HP_player)
			{
				HP_player_now -= Time.deltaTime*HP_player_max/2.0f;
			}
			else
			{
				HP_player_now += Time.deltaTime*HP_player_max/2.0f;
			}
			
			if(Mathf.Abs(HP_player_now - HP_player) < 5.0f)
				HP_player_now = HP_player;
		}
		
		if(monsterCreateTime > 0.0f) monsterCreateTime -= Time.deltaTime;
		
		if(monsterCreateTime > 0.0f) return;
		if(monsterCreateTime < 0.0f) 
		{
			monsterCreateTime = 0.0f;
			HP_initial();
		}
		
		if(ui.isGameOver) return;
		
		if(ui.isSelectWord == 2)
		{
			if(isFireBall == false)
			{
				lastCount = 0;
				isFireBall = true;
				fireBallNum = ui.backWord.finishIndex;
				fireBalls = new GameObject[fireBallNum];
				for(int i = 0 ; i < fireBallNum ; i++)
				{
					fireBalls[i] = (GameObject)Instantiate(fireBall);
					
					if(i == 0) fireBalls[i].transform.position = new Vector3(-0.2083941f, 35.57512f, 5.812185f);
					else if(i % 2 != 0) fireBalls[i].transform.position = fireBalls[0].transform.position - 
																	 	  new Vector3((i+1)/2.0f*3.0f, 0,0);
					else fireBalls[i].transform.position = fireBalls[0].transform.position + 
														   new Vector3(i/2.0f*3.0f, 0,0);
				}
			}
			
			if(ui.frontWord.finishIndex > lastCount)
			{
				lastCount = ui.frontWord.finishIndex;
				Destroy(fireBalls[ui.backWord.finishIndex - lastCount]);
			}
			
			if(ui.frontWord.finishIndex >= ui.backWord.finishIndex)
			{
				ui.isSelectWord = 0;
				isFireBall = false;
				bulletNum = ui.frontWord.finishIndex;
				bulletTime = 0.15f;
				
				// store record to database
				bool isFind = false;
				foreach(wordRecord word in ui.rcd.wordRecord)
				{
					if(word.word == firstWord.wordName)
					{
						isFind = true;
						word.addRecord(ui.error, System.DateTime.Now.ToString());
						print(word.records.Count);
					}
				}
				// this is a new word record
				if(isFind == false)
				{
					ui.db.addRecord(new writeRecord(firstWord.wordName, ui.error, System.DateTime.Now.ToString()));
					ui.rcd.wordRecord = ui.db.getRecords();
				}
				
				
				/*if(ui.db.isTerm(firstWord, secondWord))
				{
					print("is Term!!");
				}
				else
				{
					print("no Term!!");
				}*/
				//print(firstWord.wordName + " " + secondWord.wordName);
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
						//remainWordIdx[i] = -1;
						t_Card card = GameObject.Find("Card" + (i+1).ToString()).GetComponent<t_Card>();
						
						//if(card.isSelect)
						//{
							card.anim.clip = card.anim.GetClip("RotationToNegative");
							card.anim.Play();
						//}
						//else
						//	remainWordIdx[i] = ui.chooseWords[i];
					}
					
					if(isRecover)
					{
						HP_player += (int)(ui.backWord.ATK * 30.0f);
						if(HP_player > HP_player_max) HP_player = HP_player_max;
						
						isRecover = false;
					}
					
					if(HP_enemy > 0)
					{
						GameObject bullet = (GameObject)Instantiate(Bullet, monster.transform.position - new Vector3(0, 0, 10), Quaternion.identity);
						bullet.GetComponent<t_Bullet>().attackObj = GameObject.Find("Canvas");
						bullet.GetComponent<t_Bullet>().ATK = ATK_enemy;
						bullet.GetComponent<TrailRenderer>().time = 0.1f;
						bullet.GetComponent<TrailRenderer>().startWidth = 3.0f;
						bullet.GetComponent<TrailRenderer>().endWidth   = 3.0f;
					}
					
				}
			}
		}
		
		// Game over?
		if(HP_enemy == 0 && bulletNum == 0)
		{
			monsterCreateTime = CreateTime;
			monster.renderer.material = mat_black_monster;
			HP_enemy = -1;
			HP_enemy_max += 100;
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
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK * 2.0f;
				isRecover = false;
			}
			else
			if( (firstWord.property == ui.water && secondWord.property == ui.wood ) ||
			    (firstWord.property == ui.wood  && secondWord.property == ui.fire ) ||
			    (firstWord.property == ui.fire  && secondWord.property == ui.earth) ||
			    (firstWord.property == ui.earth && secondWord.property == ui.metal) ||
			    (firstWord.property == ui.metal && secondWord.property == ui.water) )
			{
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK / 2.0f;
				
				isRecover = true;
			}
			else 
			{
				bullet.GetComponent<t_Bullet>().ATK = ui.backWord.ATK;
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
		if(ui.levelNow > ui.levelNum) return;
		
		HP_player = HP_player_max;
		HP_enemy  = HP_enemy_max;
		
		if(ui.levelNow == 1)
		{
			monster.renderer.material = mat_fire_monster;
			enemy_property = ui.fire;
		}
		else
		if(ui.levelNow == 2)
		{
			monster.renderer.material = mat_earth_monster;
			enemy_property = ui.earth;
		}
		else
		if(ui.levelNow == 3)
		{
			monster.renderer.material = mat_water_monster;
			enemy_property = ui.water;
		}
		else
		if(ui.levelNow == 4)
		{
			monster.renderer.material = mat_wood_monster;
			enemy_property = ui.wood;
		}
		else
		if(ui.levelNow == 5)
		{
			monster.renderer.material = mat_metal_monster;
			enemy_property = ui.metal;
		}
		else
		{
		}
		
		Destroy(sparkle_monster);
	}
	
	public void reShuffle()
	{
		int []shuffleNum = new int[ui.length];
		shuffleNum = ui.shuffle();
		ui.chooseWords = new int[4];
		for(int i = 0 ; i < 4 ; i++)
		{
			ui.chooseWords[i] = shuffleNum[i];
			//if(remainWordIdx[i] != -1)
			//	ui.chooseWords[i] = remainWordIdx[i];
		}
	}
}

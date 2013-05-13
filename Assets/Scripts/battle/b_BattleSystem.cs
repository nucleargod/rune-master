using UnityEngine;
using System.Collections;

public class b_BattleSystem : MonoBehaviour {
	
	private b_UI ui;
	private b_CameraControl camCtrl;
	private float explosionTime;
	public GameObject monster;
	public Vector3 monsterDes;
	
	public Object explosion;
	public Object Sound_O;
	public Object Sound_X;
	
	public float defenseTime;
	
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
		ui = GameObject.Find("Global").GetComponent<b_UI>();
		camCtrl = GameObject.Find("Camera").GetComponent<b_CameraControl>();
		monster = GameObject.Find("monster01");
		monsterDes = new Vector3(0, 83.24539f, 0);
		
		explosionTime = 0.0f;
		defenseTime = 0.0f;
		
		HP_player_max = 100;
		HP_enemy_max  = 100;
		ATK_player = 100;
		ATK_enemy  = 50;
		HP_initial();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(explosionTime > 0.0f) explosionTime -= Time.deltaTime;
		else if(explosionTime < 0.0f) 
		{
			explosionTime = 0.0f;
			if(ui.mode == "Defense mode") Instantiate(explosion, new Vector3(0, 91, 0), Quaternion.identity);
			if(ui.mode == "Attack mode") Instantiate(explosion, new Vector3(0, 80, -40), Quaternion.identity);
		}
		
		if(ui.isSelectWord)
		{
			if(ui.frontWord.finishIndex >= ui.backWord.finishIndex)
			{
				camCtrl.des = new Vector3(0, 100, -50);
				ui.isSelectWord = false;
				
				int []shuffleNum = new int[ui.length];
				shuffleNum = ui.shuffle();
				ui.chooseWords = new int[4];
				for(int i = 0 ; i < 4 ; i++)
				{
					ui.chooseWords[i] = shuffleNum[i];
					ui.chooseWordPos[i] = new Vector2(Screen.width/2.0f, Screen.height*1.2f);
				}
			}
		}
		else
		{
			if(ui.isFight == false)
			{
				ui.isFight = true;
				Fight();
				
				ui.ClearCanvas();
			}
		}
		
		// monster destination
		if(Vector3.Distance(monsterDes, monster.transform.position) >= 5.0f)
		{
			monster.transform.position = Vector3.MoveTowards(monster.transform.position, monsterDes, Time.deltaTime*500.0f);
			if(Vector3.Distance(monsterDes, monster.transform.position) < 5.0f)
			{
				monster.transform.position = monsterDes;
				HP_initial();
			}
		}
		
		// HP of player
		if(Mathf.Abs(HP_player_now - HP_player) >= 1.0f)
		{
			if(HP_player_now > HP_player)
				HP_player_now -= Time.deltaTime*100;
			else
				HP_player_now += Time.deltaTime*100;
			
			if(Mathf.Abs(HP_player_now - HP_player) < 1.0f)
				HP_player_now = HP_player;
		}
		
		//HP of enemy
		if(Mathf.Abs(HP_enemy_now - HP_enemy) >= 1.0f)
		{
			if(HP_enemy_now > HP_enemy)
				HP_enemy_now -= Time.deltaTime*100;
			else
				HP_enemy_now += Time.deltaTime*100;
			
			if(Mathf.Abs(HP_enemy_now - HP_enemy) < 1.0f)
				HP_enemy_now = HP_enemy;
		}
		
		// Game over?
		if(HP_enemy == 0 && explosionTime == 0.0f)
		{
			HP_enemy = -1;
			HP_enemy_max += 50;
			ui.levelNow += 1;
			ui.Initial();
		}
		if(HP_player <= 0)
		{
			// game over
			ui.isGameOver = true;
		}
	}
	
	void Fight()
	{
		// Successful
		if(Word.Judge(ui.error) != "Fail")
		{
			GameObject snd = (GameObject) Instantiate(Sound_O);
			Destroy(snd, 2);
			explosionTime = 0.6f;
			
			if(ui.mode == "Attack mode")
			{
				switch(Word.Judge(ui.error))
				{
					case "S": HP_enemy -= (int)(ATK_player * 1.0f + Random.Range(-10.0f, 10.0f)); break;
					case "A": HP_enemy -= (int)(ATK_player * 0.8f + Random.Range(-10.0f, 10.0f)); break;
					case "B": HP_enemy -= (int)(ATK_player * 0.5f + Random.Range(-10.0f, 10.0f)); break;
					case "C": HP_enemy -= (int)(ATK_player * 0.3f + Random.Range(-10.0f, 10.0f)); break;
				}
				
				if(HP_enemy <= 0)
					HP_enemy = 0;
				else
				{
					ui.mode = "Defense mode";
					defenseTime = 2.0f;
				}
			}
			else
			{
				switch(Word.Judge(ui.error))
				{
					case "S": HP_player -= (int)(ATK_enemy * 0.5f + Random.Range(-5.0f, 0.0f)); break;
					case "A": HP_player -= (int)(ATK_enemy * 0.7f + Random.Range(-5.0f, 0.0f)); break;
					case "B": HP_player -= (int)(ATK_enemy * 0.8f + Random.Range(-5.0f, 0.0f)); break;
					case "C": HP_player -= (int)(ATK_enemy * 0.9f + Random.Range(-5.0f, 0.0f)); break;
				}
				
				if(HP_player <= 0)
					HP_player = 0;
				else
				{
					ui.mode = "Attack mode";
				}
			}
		}
		// Failed
		else
		{
			GameObject snd = (GameObject) Instantiate(Sound_X);
			Destroy(snd, 2);
			
			if(ui.mode == "Attack mode")
			{
				ui.mode = "Defense mode";
				defenseTime = 2.0f;
			}
			else
			{
				HP_player -= (int)(ATK_enemy * 1.0f + Random.Range(-5.0f, 0.0f));
				ui.mode = "Attack mode";
				
				explosionTime = 0.6f;
			}
		}
		
		ui.showE = ui.error;
	}
	
	void HP_initial()
	{
		HP_player_now = HP_player = HP_player_max;
		HP_enemy_now  = HP_enemy  = HP_enemy_max;
	}
}

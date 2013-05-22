using UnityEngine;
using System.Collections;

public class t_Bullet : MonoBehaviour {
	
	private t_UI ui;
	private t_BattleSystem battle;
	private float speed;
	
	public GameObject attackObj;
	public GameObject explosion;
	public GameObject ATK_Num;
	public float ATK;
	
	// Use this for initialization
	void Start ()
	{
		ui = GameObject.Find("Global").GetComponent<t_UI>();
		battle = GameObject.Find("Camera").GetComponent<t_BattleSystem>();
		
		speed = 500.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float dt = Time.deltaTime;
		
		if(Vector3.Distance(gameObject.transform.position, attackObj.transform.position) > 2.0f)
		{
			Vector3 dir = attackObj.transform.position - gameObject.transform.position;
			dir = Vector3.Normalize(dir);
			
			gameObject.transform.position += dir * dt * speed;
		}
		else 
		{		
			Animation anim = attackObj.GetComponent<Animation>();
			if(attackObj.transform.name == "MonsterWall")
				anim.clip = anim.GetClip("MonsterShake");
			anim.Play();
			
			//Instantiate(explosion);
			
			float atkRange = ATK + Random.Range(-10.0f, 10.0f);
			if(atkRange < 0.0f) atkRange = 0.0f;
			
			if( (ui.backWord.property == "water" && battle.enemy_property == "fire" ) ||
			    (ui.backWord.property == "wood"  && battle.enemy_property == "earth") ||
			    (ui.backWord.property == "fire"  && battle.enemy_property == "metal") ||
			    (ui.backWord.property == "earth" && battle.enemy_property == "water") ||
			    (ui.backWord.property == "metal" && battle.enemy_property == "wood" ) )
			{
				atkRange *= 1.5f;
			}
			
			if( (ui.backWord.property == "water" && battle.enemy_property == "earth") ||
			    (ui.backWord.property == "wood"  && battle.enemy_property == "metal") ||
			    (ui.backWord.property == "fire"  && battle.enemy_property == "water") ||
			    (ui.backWord.property == "earth" && battle.enemy_property == "wood" ) ||
			    (ui.backWord.property == "metal" && battle.enemy_property == "fire" ) )
			{
				atkRange *= 0.5f;
			}
			
			GameObject obj = (GameObject)Instantiate(ATK_Num);
			obj.GetComponent<t_ATK>().toShow = ((int)atkRange).ToString();
			
			battle.HP_enemy -= (int)atkRange;
			if(battle.HP_enemy < 0) battle.HP_enemy = 0;
			
			Destroy(gameObject);
		}
	}
}

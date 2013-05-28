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
		
		if(Vector3.Distance(gameObject.transform.position, attackObj.transform.position) > 5.0f)
		{
			Vector3 dir = attackObj.transform.position - gameObject.transform.position;
			dir = Vector3.Normalize(dir);
			
			if(attackObj.transform.name == "MonsterWall")
				gameObject.transform.position += dir * dt * speed;
			else
				gameObject.transform.position += dir * dt * speed / 4.0f;
		}
		else 
		{	
			// attack monster
			if(attackObj.transform.name == "MonsterWall")
			{
				Animation anim = attackObj.GetComponent<Animation>();
				anim.clip = anim.GetClip("MonsterShake");
				anim.Play();
				
				//Instantiate(explosion);
				
				float atkRange = ATK + Random.Range(-10.0f, 10.0f);
				if(atkRange < 0.0f) atkRange = 0.0f;
				
				if( (ui.backWord.property == ui.water && battle.enemy_property == ui.fire ) ||
					(ui.backWord.property == ui.wood  && battle.enemy_property == ui.earth) ||
					(ui.backWord.property == ui.fire  && battle.enemy_property == ui.metal) ||
					(ui.backWord.property == ui.earth && battle.enemy_property == ui.water) ||
					(ui.backWord.property == ui.metal && battle.enemy_property == ui.wood ) )
				{
					atkRange *= 1.5f;
				}
				
				if( (ui.backWord.property == ui.water && battle.enemy_property == ui.earth) ||
					(ui.backWord.property == ui.wood  && battle.enemy_property == ui.metal) ||
					(ui.backWord.property == ui.fire  && battle.enemy_property == ui.water) ||
					(ui.backWord.property == ui.earth && battle.enemy_property == ui.wood ) ||
					(ui.backWord.property == ui.metal && battle.enemy_property == ui.fire ) )
				{
					atkRange *= 0.5f;
				}
				
				GameObject obj = (GameObject)Instantiate(ATK_Num);
				obj.GetComponent<t_ATK>().toShow = ((int)atkRange).ToString();
				
				battle.HP_enemy -= (int)atkRange;
				if(battle.HP_enemy < 0) battle.HP_enemy = 0;
			}
			
			// attack player
			if(attackObj.transform.name == "Canvas")
			{
				Animation anim = GameObject.Find("Camera").GetComponent<Animation>();
				anim.clip = anim.GetClip("CameraShake");
				anim.Play();
				
				Instantiate(explosion);
				
				float atkRange = ATK + Random.Range(-10.0f, 10.0f);
				if(atkRange < 0.0f) atkRange = 0.0f;
				
				battle.HP_player -= (int)atkRange;
				if(battle.HP_player < 0) battle.HP_player = 0;
			}
			
			Destroy(gameObject);
		}
	}
}

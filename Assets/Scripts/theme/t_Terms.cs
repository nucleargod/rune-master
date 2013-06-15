using UnityEngine;
using System.Collections;

public class t_Terms : MonoBehaviour {
	
	public GameObject firstWord;
	public GameObject secondWord;
	public Object ATK_Num;
	
	private t_BattleSystem battle;
	private GameObject monster;
	private GameObject camera;
	private float speed;
	
	// Use this for initialization
	void Start () 
	{
		monster = GameObject.Find("MonsterWall");
		camera 	= GameObject.Find("Camera");
		battle  = camera.GetComponent<t_BattleSystem>();
		
		firstWord.renderer.material  = battle.firstWord.mat_t;
		secondWord.renderer.material = battle.secondWord.mat_t;
		
		gameObject.transform.position = camera.transform.position;
		speed = 450.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 direction = monster.transform.position - gameObject.transform.position;
		direction = Vector3.Normalize(direction);
		gameObject.transform.position += direction * speed * Time.deltaTime;
		
		if(Vector3.Distance(monster.transform.position, gameObject.transform.position) < 3.0f)
		{
			GameObject obj = (GameObject)Instantiate(ATK_Num);
			int tatk = ((int)battle.firstWord.ATK + (int)battle.secondWord.ATK)*2;
			obj.GetComponent<t_ATK>().toShow = tatk.ToString();
			
			battle.HP_enemy -= tatk;
			if(battle.HP_enemy < 0) battle.HP_enemy = 0;
			
			Destroy(gameObject);
		}
	}
}

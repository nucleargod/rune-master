using UnityEngine;
using System.Collections;

public class t_Card : MonoBehaviour {
	
	private t_UI ui;
	private t_BattleSystem battle;
	private int cardIdx;
	public GameObject positive;
	public GameObject negative;
	public Animation anim;
	public bool isSelect;
	
	public Object mat_metal;
	public Object mat_wood;
	public Object mat_water;
	public Object mat_fire;
	public Object mat_earth;
	
	// Use this for initialization
	void Start () 
	{
		ui = GameObject.Find("Global").GetComponent<t_UI>();
		battle = GameObject.Find("Camera").GetComponent<t_BattleSystem>();
		
		string name = gameObject.transform.name;
		cardIdx = int.Parse( name.Remove(0, 4) );
		
		positive = GameObject.Find("Card-positive-" + cardIdx.ToString());
		negative = GameObject.Find("Card-negative-" + cardIdx.ToString());
		
		anim = gameObject.GetComponent<Animation>();
		anim.clip = anim.GetClip("CreateCard" + cardIdx.ToString());
		anim.Play();
		
		isSelect = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		positive.renderer.material = ui.wordList[ui.chooseWords[cardIdx-1]].mat;
		
		Word word = (Word)ui.wordList[ui.chooseWords[cardIdx-1]];
		if(word.property == ui.metal)
			negative.renderer.material = (Material)mat_metal;
		if(word.property == ui.wood)
			negative.renderer.material = (Material)mat_wood;
		if(word.property == ui.water)
			negative.renderer.material = (Material)mat_water;
		if(word.property == ui.fire)
			negative.renderer.material = (Material)mat_fire;
		if(word.property == ui.earth)
			negative.renderer.material = (Material)mat_earth;
		
		if(anim.clip.name == "RotationToNegative" && anim.isPlaying == false)
		{
			anim.clip = anim.GetClip("CreateCard" + cardIdx.ToString());
			anim.Play();
			
			this.isSelect = false;
			battle.reShuffle();
		}
	}
	
	void OnMouseDown()
	{
		if(ui.isSelectWord < 2 && this.isSelect == false && anim.isPlaying == false)
		{
			if(ui.isSelectWord == 0)
			{
				Word word = (Word)ui.wordList[ui.chooseWords[cardIdx-1]];
				battle.firstWord = ui.backWord = word;
			}
			if(ui.isSelectWord == 1)
			{
				Word word = (Word)ui.wordList[ui.chooseWords[cardIdx-1]];
				battle.secondWord = word;
				
				ui.ClearCanvas();
			}
			
			this.isSelect = true;
			ui.isSelectWord++;
			
			anim.clip = anim.GetClip("RotationToPositive");
			anim.Play();
		}
	}
}

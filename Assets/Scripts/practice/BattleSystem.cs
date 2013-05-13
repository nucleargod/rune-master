using UnityEngine;
using System.Collections;

public class BattleSystem : MonoBehaviour {
	
	private UI ui;
	private CameraControl camCtrl;
	private float explosionTime;
	
	public Object explosion;
	public Object Sound_O;
	public Object Sound_X;
	
	// Use this for initialization
	void Start ()
	{
		ui = GameObject.Find("Global").GetComponent<UI>();
		camCtrl = GameObject.Find("Camera").GetComponent<CameraControl>();
		
		explosionTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(explosionTime > 0.0f) explosionTime -= Time.deltaTime;
		else if(explosionTime < 0.0f) 
		{
			explosionTime = 0.0f;
			Instantiate(explosion, new Vector3(0, 91, 0), Quaternion.identity);
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
					ui.chooseWords[i] = shuffleNum[i];
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
	}
	
	void Fight()
	{
		if(ui.error < 150.0f)
		{
			GameObject snd = (GameObject) Instantiate(Sound_O);
			Destroy(snd, 2);
			
			explosionTime = 2.0f;
		}
		else
		{
			GameObject snd = (GameObject) Instantiate(Sound_X);
			Destroy(snd, 2);
		}
		
		ui.showE = ui.error;
	}
}

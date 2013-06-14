using UnityEngine;
using System.Collections;

public class m_exit : MonoBehaviour {
	
	private m_UI ui;
	private float loadTime;
	private bool isLoading;
	private Vector3 oPos;
	
	public Object sound;
	
	// Use this for initialization
	void Start () 
	{
		ui = GameObject.Find("Camera").GetComponent<m_UI>();
		loadTime  = 0.0f;
		isLoading = false;
		oPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(loadTime > 0.0f)
		{
			loadTime -= Time.deltaTime;
			
			if(loadTime < 0.0f)
			{
				loadTime = 0.0f;
				Application.Quit();
			}
		}
	}
	
	void OnMouseDown()
	{
		if(ui.glstate != "GLowBlance") return;
		
		if(isLoading == false)
		{
			loadTime = 1.0f;
			isLoading = true;
		}
	}
	
	void OnMouseEnter()
	{
		if(ui.glstate != "GLowBlance") return;
		
		gameObject.transform.position = oPos - new Vector3(1, 0, 0);
		GameObject snd = (GameObject)Instantiate(sound);
		Destroy(snd, 0.9f);
	}
	
	void OnMouseExit()
	{
		if(ui.glstate != "GLowBlance") return;
		
		gameObject.transform.position = oPos;
	}
}

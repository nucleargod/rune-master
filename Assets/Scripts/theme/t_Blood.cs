using UnityEngine;
using System.Collections;

public class t_Blood : MonoBehaviour {
	
	private GUITexture mat_blood;
	private Material   mat_bloodWall;
	private float rate;
	
	// Use this for initialization
	void Start () 
	{
		rate = 0.5f;
		
		mat_blood = GameObject.Find("Blood").GetComponent<GUITexture>();
		mat_blood.color = new Color(mat_blood.color.r,
									mat_blood.color.g,
									mat_blood.color.b,
									rate);
		
		mat_bloodWall = GameObject.Find("BloodWall").GetComponent<Renderer>().material;
		mat_bloodWall.color = new Color(mat_bloodWall.color.r,
										mat_bloodWall.color.g,
										mat_bloodWall.color.b,
										rate);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(rate > 0.0f)
			rate -= Time.deltaTime/5.0f;
		if(rate < 0.0f)
			rate = 0.0f;
		
		mat_blood.color = new Color(mat_blood.color.r,
			mat_blood.color.g,
			mat_blood.color.b,
			rate);
		
		mat_bloodWall.color = new Color(mat_bloodWall.color.r,
			mat_bloodWall.color.g,
			mat_bloodWall.color.b,
			rate);
		
		if(rate == 0.0f)
			Destroy(gameObject);
	}
}

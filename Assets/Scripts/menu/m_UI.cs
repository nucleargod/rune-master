using UnityEngine;
using System.Collections;

public class m_UI : MonoBehaviour {
	
	private GlowEffect gleffect;
	public string glstate;
	
	private GameObject button1;
	private GameObject button2;
	private GameObject button3;
	private GameObject button4;
	
	// Use this for initialization
	void Start () 
	{
		gleffect = GameObject.Find("Camera").GetComponent<GlowEffect>();
		gleffect.glowIntensity = 3.5f;
		glstate = "GlowUp";
		// GlowUp
		// GlowDown
		// GlowBlance
		
		button1 = GameObject.Find("BUTTON_practice"); button1.SetActive(false);
		button2 = GameObject.Find("BUTTON_battle");   button2.SetActive(false);
		button3 = GameObject.Find("BUTTON_special");  button3.SetActive(false);
		button4 = GameObject.Find("BUTTON_exit");     button4.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(glstate == "GlowDown")
		{
			gleffect.glowIntensity -= Time.deltaTime*0.6f;
			if(gleffect.glowIntensity < 3.0f)
			{
				glstate = "GLowBlance";
			}
		}
		
		if(glstate == "GlowUp")
		{
			gleffect.glowIntensity += Time.deltaTime*0.6f;
			if(gleffect.glowIntensity > 5.0f)
			{
				glstate = "GlowDown";
				
				button1.SetActive(true);
				button2.SetActive(true);
				button3.SetActive(true);
				button4.SetActive(true);
			}
		}
	}
}

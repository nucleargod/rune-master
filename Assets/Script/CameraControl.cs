using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public Vector3 des;
	
	private GameObject camObj;
	
	// Use this for initialization
	void Start ()
	{
		camObj = GameObject.Find("Camera");
		des = camObj.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(camObj.transform.position == des) return;
		
		
		float T = Time.deltaTime;
		
		if(Vector3.Distance(camObj.transform.position, des) < 5.0f)
		{
			camObj.transform.position = des;
		}
		else
		{
			Vector3 dir = des - camObj.transform.position;
			dir = Vector3.Normalize(dir);
			
			camObj.transform.position += dir * T * 500.0f;
		}
	}
}

using UnityEngine;
using System.Collections;

public class t_showDead : MonoBehaviour {
	
	private Vector3 startPos;
	private Vector3 destination;
	private float speed;
	private Vector3 direction;
	
	// Use this for initialization
	void Start () 
	{
		startPos    = new Vector3(1.32252e-05f , 33.09502f, 101.2787f );
		destination = new Vector3(5.071854e-07f, 33.095f  , -44.19848f);
		direction = Vector3.Normalize(destination - startPos);
		speed = 100.0f;
		
		gameObject.transform.position = startPos;
		hhhh = false;
	}
	
	bool hhhh;
	// Update is called once per frame
	void Update () 
	{
		if(hhhh) return;
		if(destination != transform.position && 
			Vector3.Angle(destination - transform.position, direction) < 1.0f)
		{
			gameObject.transform.position += Time.deltaTime * speed * direction;
		}
		else{gameObject.transform.position = destination; hhhh = true;}
	}
	
	public void setTexture(Texture2D tex){
		GameObject o = transform.GetChild(0).gameObject;
		MeshRenderer mr = o.GetComponent<MeshRenderer>();
		mr.material.mainTexture = tex;
	}
}

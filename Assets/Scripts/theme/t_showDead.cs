using UnityEngine;
using System.Collections;

public class t_showDead : MonoBehaviour {
	
	private Vector3 startPos;
	private Vector3 destination;
	private float speed;
	
	// Use this for initialization
	void Start () 
	{
		startPos    = new Vector3(1.32252e-05f , 33.09502f, 101.2787f );
		destination = new Vector3(5.071854e-07f, 33.095f  , -44.19848f);
		speed = 100.0f;
		
		gameObject.transform.position = startPos;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(gameObject.transform.position, destination) > 3.0f)
		{
			Vector3 direction = Vector3.Normalize(destination - gameObject.transform.position);
			gameObject.transform.position += Time.deltaTime * speed * direction;
		}
		else gameObject.transform.position = destination;
	}
}

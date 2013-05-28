using UnityEngine;
using System.Collections;

public class TouchEffect : MonoBehaviour {
	
	public RaycastHit theHit;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		particleSystem.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) 
			&& Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out theHit))
		{
			transform.position = theHit.point;
			particleSystem.enableEmission = true;
		}
		if(Input.GetMouseButton(0) 
			&& Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out theHit))
		{
			transform.position = theHit.point;
		}
		else
		{
			
		}
		if(Input.GetMouseButtonUp(0))
		{
			particleSystem.enableEmission = false;
		}
	}
}

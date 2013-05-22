using UnityEngine;
using System.Collections;

public class Canvas : MonoBehaviour {
	
	public Word word;
	
	// Use this for initialization
	void Start () {
		word = new Word();
		GameObject.Find("Displayer").GetComponent<WordDisplay>().SetTarget(word);
	}
	
	void OnMouseDown()
	{
		word.BeginWriting();
	}
	
	private Vector3 tmpPos;
	private RaycastHit theHit;
	void OnMouseDrag()
	{
		if(tmpPos == Input.mousePosition) return;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out theHit))
			word.Writing(theHit.point);
		tmpPos = Input.mousePosition;
	}
	
	void OnMouseUp()
	{
		word.EndWriting();
	}
}

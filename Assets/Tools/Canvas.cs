using UnityEngine;
using System.Collections;

public class Canvas : MonoBehaviour {
	
	public Word word;
	public float canvasSize;
	public WordDisplay frontDisplay;
	public WordDisplay backDisplay;
	
	// Use this for initialization
	void Start () {
		word = new Word();
		frontDisplay = GetComponents<WordDisplay>()[0];
		backDisplay  = GetComponents<WordDisplay>()[1];
		frontDisplay.SetTarget(word);
		//GameObject.Find("Displayer").GetComponent<WordDisplay>().SetTarget(word);
		canvasSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
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
		{
			theHit.point = transform.worldToLocalMatrix.MultiplyPoint(theHit.point);
			theHit.point /= canvasSize;
			word.Writing(theHit.point);
		}
		tmpPos = Input.mousePosition;
	}
	
	void OnMouseUp()
	{
		word.EndWriting();
	}
}

using UnityEngine;
using System.Collections;

public class Canvas : MonoBehaviour {
	
	public Word word;
	private float canvasSize;
	
	// Use this for initialization
	void Start () {
		word = new Word();
		GameObject.Find("Displayer").GetComponent<WordDisplay>().SetTarget(word);
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
			//theHit.point = transform.worldToLocalMatrix.MultiplyPoint(theHit.point)/canvasSize;
			word.Writing(theHit.point);
			//Debug.Log(theHit.point.ToString());
		}
		tmpPos = Input.mousePosition;
	}
	
	void OnMouseUp()
	{
		word.EndWriting();
	}
}

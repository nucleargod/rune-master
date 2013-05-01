using UnityEngine;
using System.Collections;

public class StrokeDisplay : MonoBehaviour {
	
	public LineRenderer lineRenderer;
	
	private Vector3 offset;
	private Stroke targetStroke;
	private int tmpCount;
	// Use this for initialization
	void Start () {
		lineRenderer.enabled = false;
		offset = Vector3.back*0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetStroke == null) return;
		if(targetStroke.pointList.Count < 2) lineRenderer.enabled = false;
		else lineRenderer.enabled = true;
		
		if(tmpCount != targetStroke.pointList.Count)
		{
			tmpCount = targetStroke.pointList.Count;
			SetRenderer();
		}
	}
	
	public void SetTarget(Stroke stroke)
	{
		targetStroke = stroke;
		if(stroke.pointList.Count==0) return;
		SetRenderer();
	}
	
	void SetRenderer()
	{
		lineRenderer.SetVertexCount(tmpCount);
		for(int i = 0; i < tmpCount; i++)
		{
			lineRenderer.SetPosition(i, (Vector3)targetStroke.pointList[i]+offset);
		}
	}
}

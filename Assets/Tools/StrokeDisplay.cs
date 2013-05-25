using UnityEngine;
using System.Collections;

public class StrokeDisplay : MonoBehaviour {
	
	public DisplayStyle style;
	public DisplayAnimation animation;
	public GameObject effect;
	
	public float stepLength;
	public Vector3[] DrawPointList;
	public int DPLSize;
	private Vector3 offset;
	public Stroke targetStroke;
	// Use this for initialization
	void Start () {
		offset = Vector3.back;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetStroke == null) return;
		
		
		if(animation != DisplayAnimation.Default)
			DoAnimation();
		DoStyle();
	}
	
	private float styDelay = 0.0f;
	private int targetIndex = 0;
	private int lastIndex = 0;
	public void DoStyle()
	{
		switch(style)
		{
		case DisplayStyle.Default:
			if(styDelay < 0)
			{
				for(int i = 0; i < DPLSize; i++)
				{
					effect.transform.position = DrawPointList[i];
					effect.particleSystem.Emit(1);
				}
				styDelay = effect.particleSystem.startLifetime/10.0f;
			}else styDelay -= Time.deltaTime; 
			break;
			
		case DisplayStyle.Flicker:
			if(styDelay < 0)
			{
				for(int i = 0; i < DPLSize; i++)
				{
					effect.transform.position = DrawPointList[i];
					effect.particleSystem.Emit(1);
				}
				styDelay = effect.particleSystem.startLifetime;
			}else styDelay -= Time.deltaTime; 
			break;
			
		case DisplayStyle.Loop:
			targetIndex = (targetIndex+(int)(Time.deltaTime*100.0f))%DPLSize;
			if(lastIndex > targetIndex) lastIndex = 0;
			for(int i = lastIndex; i < targetIndex; i++)
			{
				effect.transform.position = DrawPointList[i];
				effect.particleSystem.Emit(1);
			}
			lastIndex = targetIndex;
			break;
			
		case DisplayStyle.RLoop:
			targetIndex = (targetIndex-(int)(Time.deltaTime*100.0f))%DPLSize;
			if(targetIndex<0) targetIndex += DPLSize;
			if(lastIndex < targetIndex) lastIndex = DPLSize-1;
			for(int i = lastIndex; i > targetIndex; i--)
			{
				effect.transform.position = DrawPointList[i];
				effect.particleSystem.Emit(1);
			}
			lastIndex = targetIndex;
			break;
			
		default:
			style = DisplayStyle.Default;
			break;
		}
	}
	
	private float aniDelay;
	public void DoAnimation()
	{
		switch(animation)
		{
		case DisplayAnimation.Default:
			break;
			
		case DisplayAnimation.Focus:
			aniDelay += Time.deltaTime;
			for(int i = 0; i < DPLSize; i++)
			{
				DrawPointList[i] = Vector3.MoveTowards(DrawPointList[i], DrawPointList[DPLSize/2], Time.deltaTime*10);
			}
			if(aniDelay >= 1.0f)
			{
				aniDelay = 0.0f;
				animation = DisplayAnimation.Default;
			}
			break;
			
		default:
			animation = DisplayAnimation.Default;
			break;
		}
	}
	
	public void SetTarget(Stroke stroke)
	{
		targetStroke = stroke;
		MakeDrawPoint(0.01f, stroke.pointList);
	}
	
	private void MakeDrawPoint(float stepLength, ArrayList pointList)
	{
		if(pointList.Count == 0)
		{
			DrawPointList = null;
			return;
		}
		if(pointList.Count == 1)
		{
			DrawPointList = new Vector3[1];
			DrawPointList[0] = (Vector3)pointList[0];
			return;
		}
		float canvasSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
		float totalLength = 0.0f;
		for(int i = 0; i < pointList.Count-1; i++)
		{
			totalLength += Vector3.Distance((Vector3)pointList[i], (Vector3)pointList[i+1]);
		}
		DPLSize = (int)(totalLength/stepLength);
		DrawPointList = new Vector3[DPLSize];
		Vector3 curPos = (Vector3)pointList[0];
		Vector3 prePos = (Vector3)pointList[0];
		int curPointIndex = 0;
		float tmpLength = 0.0f;
		float curLength = Vector3.Distance(DrawPointList[0], DrawPointList[1]);
		float preLength = 0.0f;
		
		DrawPointList[0] = curPos;
		for(int i = 1; i < DPLSize; i++)
		{
			tmpLength = stepLength*i;
			while(curLength < tmpLength)
			{
				prePos = curPos;
				preLength = curLength;
				curPointIndex++;
				curPos = (Vector3)pointList[curPointIndex];
				curLength += Vector3.Distance(curPos, prePos);
			}
			
			DrawPointList[i] = Vector3.Lerp(prePos, curPos, (curLength-tmpLength)/(curLength-preLength));
			DrawPointList[i] = transform.localToWorldMatrix.MultiplyPoint(DrawPointList[i]*canvasSize);
		}
	}
}

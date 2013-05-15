using UnityEngine;
using System.Collections;

public class Stroke {
	
	public ArrayList pointList;
	
	// Use this for initialization
	public Stroke () {
		pointList = new ArrayList();
	}

	public float GetError( Stroke stroke )
	{
		int size = this.pointList.Count;
		float error = 0.0f;
		for(int i = 0; i < size; i++)
		{
			error += Vector3.Distance((Vector3)stroke.pointList[i], (Vector3)this.pointList[i]);
		}
		return error/size;
	}
	
	public float GetDotError ( Stroke stroke )
	{
		int size = this.pointList.Count;
		float error = 0.0f;
		float tmp;
		for(int i = 0; i < size-1; i++)
		{
			tmp = Vector3.Dot(
				( (Vector3)stroke.pointList[i]-(Vector3)stroke.pointList[i+1] ).normalized, 
				( (Vector3)this.pointList[i]-(Vector3)this.pointList[i+1] ).normalized
				);
			if(tmp < 0) tmp = tmp*-2.0f;
			else tmp = 1-tmp;
			error += tmp;
		}
		return error/size;
	}
	
	public float GetSlopeError ( Stroke stroke )
	{
		int size = this.pointList.Count;
		float error = 0.0f;
		for(int i = 0; i < size-1; i++)
		{
			error += Vector3.Angle(
				( (Vector3)stroke.pointList[i]-(Vector3)stroke.pointList[i+1] ),
				( (Vector3)this.pointList[i]-(Vector3)this.pointList[i+1] )
				);
		}
		return error/size;
	}
	
	public float GetMixError ( Stroke stroke )
	{
		return Mathf.Pow(GetError(stroke), 2.0f) + Mathf.Pow(GetSlopeError(stroke), 1.5f);
	}
	
	public int AddPoint(Vector3 worldPoint)
	{
		pointList.Add(worldPoint);
		return pointList.Count;
	}
	
	public void Clear()
	{
		pointList.Clear();
	}
	
	public void Resample(int size)
	{
		if(pointList.Count==0) return;
		if(pointList.Count==1) 
		{
			for(int i = 0; i < 99; i++)
				pointList.Add(pointList[0]);
			return;
		}
		ArrayList newList = new ArrayList();
		float totalLength = 0.0f;
		float stepLength = 0.0f;
		for(int i = 0; i < pointList.Count-1; i++)
		{
			totalLength += ((Vector3)pointList[i]-(Vector3)pointList[i+1]).magnitude;
		}
		stepLength = totalLength/size;
		
		int rightPointIndex = 0;
		float leftLength = 0.0f;
		float rightLength = 0.0f;
		float tmpLength = 0.0f;
		newList.Add(pointList[0]);
		for(int i = 1; i < size; i++)
		{
			tmpLength = stepLength*i;
			while(rightLength < tmpLength)
			{
				leftLength = rightLength;
				rightLength += ((Vector3)pointList[rightPointIndex]-(Vector3)pointList[rightPointIndex+1]).magnitude;
				rightPointIndex++;
			}
			newList.Add ( 
				Vector3.Lerp (
					(Vector3)pointList[rightPointIndex-1],
					(Vector3)pointList[rightPointIndex],
					(tmpLength-leftLength)/(rightLength-leftLength)
				)
			);
		}
		
		pointList = newList;
	}
}

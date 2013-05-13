using UnityEngine;
using System.Collections;

public class Word {
	
	public ArrayList strokeList;
	public int finishIndex = 0;
	public string wordName;
	
	private Stroke curStroke;
	public Word () {
		strokeList = new ArrayList();
	}
	
	public float GetError( Word word )
	{
		int size;
		if(word.strokeList.Count < this.strokeList.Count)
			size = word.strokeList.Count;
		else 
			size = this.strokeList.Count;
		
		float error = 0.0f;
		for(int i = 0; i < size; i++)
		{
			error += ((Stroke)this.strokeList[i]).GetMixError((Stroke)(word.strokeList[i]));
		}
		return error/size;
	}
	
	public void BeginWriting()
	{
		curStroke = new Stroke();
		strokeList.Add(curStroke);
	}
	
	public void Writing(Vector3 worldPoint)
	{
		curStroke.AddPoint(worldPoint);
	}
	
	public void EndWriting()
	{
		curStroke.Resample(100);
		finishIndex++;
	}
	
	public static string Judge(float error)
	{
		if(error <=  50) return "S";
		if(error <=  80) return "A";
		if(error <= 120) return "B";
		if(error <= 150) return "C";
		
		return "Fail";
	}
	
}

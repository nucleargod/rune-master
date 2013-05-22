using UnityEngine;
using System.Collections;

public class Word {
	
	public ArrayList strokeList;
	public int finishIndex = 0;
	public string wordName;
	public const int pointPerStroke = 100;
	public Texture2D image;
	public string buso;
	
	private Stroke curStroke;
	public Word () {
		strokeList = new ArrayList();
	}
	
	/*constructor for initial from database*/
	public Word (string name, string strokes, int stroke_count, string rbuso) {
		strokeList = new ArrayList();
		
		wordName = name;
		buso = rbuso;
		if(strokes == null) {
			Debug.Log("null strokes");
			//throw new UnityException("null strokes");
		}
		else{
			System.IO.StringReader stokeReader = new System.IO.StringReader(strokes);
			for(int i=0;i<stroke_count;i++){
				Stroke t_stroke = new Stroke();
				for(int j=0;j<pointPerStroke;j++){
					string line=stokeReader.ReadLine();
					string []split = line.Split(new char[]{' '});
					Vector3 p = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
					t_stroke.AddPoint(p);
				}
				strokeList.Add(t_stroke);
			}
		}
		finishIndex = stroke_count;
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
		curStroke.Resample(pointPerStroke);
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
	
	public string writeStroke(){
		string d = "";
		for(int i=0; i < finishIndex; i++){
			Stroke t = (Stroke)(strokeList[i]);
			for(int j=0;j<pointPerStroke;j++){
				Vector3 p = (Vector3)(t.pointList[j]);
				d += p.x.ToString() + " " + p.y.ToString() + " " + p.z.ToString() + "\n";
			}
		}
		return d;
	}
	
	public bool loadImage(string path){
		image = (Texture2D)(Resources.Load(path));
		return image != null;
	}
}
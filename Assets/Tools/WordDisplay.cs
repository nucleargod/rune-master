using UnityEngine;
using System.Collections;

public class WordDisplay : MonoBehaviour {
	
	public GameObject strokeDisplayer;
	public Color color;
	
	private StrokeDisplay[] strokeDisplayList;
	private Word targetWord;
	private int curSize = 0;
	private int maxSize = 100;
	// Use this for initialization
	void Start () {
		strokeDisplayList = new StrokeDisplay[100];
		for(int i = 0; i < maxSize; i++)
		{
			
			GameObject GO = (GameObject)Instantiate(strokeDisplayer);
			GO.transform.parent = this.transform;
			strokeDisplayList[i] = GO.GetComponent<StrokeDisplay>();
			strokeDisplayList[i].lineRenderer.SetColors(color, color);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(targetWord == null) return;
		if( curSize > maxSize ) return;
		
		if( curSize < targetWord.strokeList.Count)
		{
			strokeDisplayList[curSize].SetTarget((Stroke)targetWord.strokeList[curSize]);
			curSize++;
		}
	}
	
	public void SetTarget( Word word )
	{
		targetWord = word;
		Stroke S = new Stroke();
		if(word == null)
		{
			for(int i = 0; i < maxSize; i++)
				strokeDisplayList[i].SetTarget( S );
			return;
		}

		curSize = word.strokeList.Count;
		for(int i = 0; i < maxSize; i++)
		{
			if( i < targetWord.strokeList.Count )
				strokeDisplayList[i].SetTarget( (Stroke)targetWord.strokeList[i] );
			else
				strokeDisplayList[i].SetTarget( S );
		}
	}
}

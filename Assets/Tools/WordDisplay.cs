using UnityEngine;
using System.Collections;

public enum DisplayStyle {
	Default,
	Loop,
	RLoop,
	Flicker
}

public enum DisplayAnimation {
	Default, // do nothing
	Focus
}

public enum DrawRank {
	S = 0,
	A = 1,
	B = 2,
	C = 3,
	D = 4,
	E = 5
}

public class WordDisplay : MonoBehaviour {
	
	public Color color;
	public StrokeDisplay[] strokeDisplayList;
	public GameObject effect;

	private Word targetWord = null;
	
	private int curSize = 0;
	private int maxSize = 100;
	private int tmpCount;
	private Vector3 offset;
	private Stroke DrawingStroke;
	// Use this for initialization
	void Start () {
		offset = Vector3.back;
		effect = (GameObject)Instantiate(effect);
		strokeDisplayList = new StrokeDisplay[100];
		for(int i = 0; i < maxSize; i++)
		{
			strokeDisplayList[i] = this.gameObject.AddComponent<StrokeDisplay>();	
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( targetWord == null ) return;
		if( curSize > maxSize ) return;
		
		
		if( curSize < targetWord.finishIndex)
		{
			strokeDisplayList[curSize].SetTarget((Stroke)targetWord.strokeList[curSize]);
			strokeDisplayList[curSize].enabled = true;
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
			strokeDisplayList[i].effect = effect;
			if( i < targetWord.strokeList.Count )
			{
				strokeDisplayList[i].SetTarget( (Stroke)targetWord.strokeList[i] );
				strokeDisplayList[i].enabled = true;
			}
			else
			{
				strokeDisplayList[i].enabled = false;
			}
		}
	}
	
	public bool hasTarget(){ return targetWord != null;}
}

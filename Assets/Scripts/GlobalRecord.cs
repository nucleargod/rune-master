using UnityEngine;
using System.Collections;

public class GlobalRecord : MonoBehaviour {
	
	public model database;
	public System.Collections.Generic.List<string> wordList;
	
	private wordRecord[] _wordRecord = null;
	
	public wordRecord[] records{
		get {
			if(_wordRecord == null) _wordRecord = database.getRecords();
			return _wordRecord;}
	}
	
	// Use this for initialization
	void Start () 
	{
		database = GameObject.Find("database").GetComponent<model>();
		wordList = database.getWords();
		
		DontDestroyOnLoad(gameObject);
		Application.LoadLevel("menuScene");
	}
	
	public void addRecord(string w, float score){
		//database.addRecord(r);
		
		if(_wordRecord == null){
			database.addRecord(new writeRecord(w,score,database.getTime()));
		}
		else{
			foreach(wordRecord word in _wordRecord)
			{
				if(word.word == w)
				{
					word.addRecord(score, database.getTime());
					print(word.records.Count);
					return;
				}
			}
			wordRecord wr = new wordRecord(w);
			wr.addRecord(score, database.getTime());
			database.addRecord(wr.records[0]);
			
			wordRecord[] nRecords = new wordRecord[_wordRecord.Length+1];
			_wordRecord.CopyTo(nRecords, 0);
			nRecords[_wordRecord.Length] = wr;
			_wordRecord = nRecords;
		}
	}
	
	public wordRecord getRecord(string w){
		foreach(wordRecord i in _wordRecord){
			if(w == i.word) return i;
		}
		return null;
	}
}

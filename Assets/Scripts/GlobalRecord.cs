using UnityEngine;
using System.Collections;

public class GlobalRecord : MonoBehaviour {
	
	public model database;
	public wordRecord[] wordRecord;
	public System.Collections.Generic.List<string> wordList;
	
	// Use this for initialization
	void Start () 
	{
		database = GameObject.Find("database").GetComponent<model>();
		wordRecord = database.getRecords();
		wordList = database.getWords();
		
		DontDestroyOnLoad(gameObject);
		Application.LoadLevel("menuScene");
	}
}

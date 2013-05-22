using UnityEngine;
using System.Collections;

public class model : MonoBehaviour {
	private string description;
	public bool toggle;
	private dbAccess db;

	// Use this for initialization
	void Start () {
		Debug.Log("starting SQLiteLoad app");
		
		// Retrieve next word from database
		description = "something went wrong with the database";
		db = GetComponent<dbAccess>();
		
		if(db != null){
			db.OpenDB("word.db");
			System.Data.IDataReader reader = db.BasicQuery("SELECT * FROM words");
			//System.Collections.Generic.List<string> reader = db.getWords();
			
			if( reader != null)
			{
				description = "";
				string water = "\u6C34";
				//string disaster = "\u707D";
				Word d = db.getWord(water);
				if(d != null) description = "it works!";
				else description = water;
				/*
				while(reader.Read()){
					description += reader.GetString(0) + " ";
					description += reader.GetString(1) + " ";
					description += reader.GetInt32(2) + " ";
					string t = (string)reader.GetValue(3);
					if(t == null) description +="NULL ";
					else description += t + " ";
					//description += reader.GetValue(3) + " ";
					description += reader.GetValue(4) + " ";
					description += reader.GetValue(5) + " ";
					description += reader.GetValue(6) + " ";
					description += reader.GetValue(7) + " ";
					description += reader.GetValue(8) + " ";
					/*for(int i=0; i<reader.FieldCount;i++){
						description += reader.GetString(i) + "\t";
					}//*/
				//	description += "\n";
				//}
			}
		}
		else {
			toggle = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnGUI () {
		//toggle = GUI.Toggle(new Rect(Screen.width - 5 - 100, Screen.height*2/3 - 5 - 100, 100, 100),toggle,"description");
		if(toggle){
			toggle = GUI.Toggle(new Rect(Screen.width - 5 - 100, Screen.height*2/3 - 5 - 100, 100, 100),toggle,"description");
			// create the gui text of the description
			GUI.Box (new Rect (5 ,Screen.height*2/3 - 5, Screen.width - 10, Screen.height/3), description);
			if(GUI.skin.customStyles.Length > 0)
	        	GUI.skin.customStyles[0].wordWrap = true;
		}
	}
	
	public void updateWord(Word w){
		if(db != null){
			bool d = db.updateWord(w);
			if(d == false){
				description = db.errMsg;
				toggle = true;
			}
		}
	}
	
	public void insertWord(Word w){
		if(db != null){
			bool d = db.insertWord(w);
			if(d == false){
				description = db.errMsg;
				toggle = true;
			}
		}
	}
	
	public Word getWord(string w){
		if(db != null){
			Word d = db.getWord(w);
			if(d == null){
				description = db.errMsg;
				toggle = true;
			}
			return d;
		}
		else return null;
	}
	
	public System.Collections.Generic.List<string> getWords(){
		if(db != null){
			System.Collections.Generic.List<string> d = db.getWords();
			if(d == null){
				description = db.errMsg;
				toggle = true;
			}
			return d;
		}
		else return null;
	}
	
	void OnDestroy(){
		if(db != null){
			db.CloseDB();
		}
	}
}

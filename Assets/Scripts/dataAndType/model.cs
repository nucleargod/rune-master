using UnityEngine;
using System.Collections;

public class model : MonoBehaviour {
	private string description;
	public bool toggle;
	private dbAccess db;
	
	//cache
	//[SerializeField]
	private themeRecord[] c_themes = null;

	// Use this for initialization
	void Start () {
		Debug.Log("starting SQLiteLoad app");
		
		// Retrieve next word from database
		description = "something went wrong with the database";
		db = new dbAccess();
		
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
		DontDestroyOnLoad(gameObject);
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
	
	/*update the data of word w, developer only*/
	public void updateWord(Word w){
		if(db != null){
			bool d = db.updateWord(w);
			if(d == false){
				description = db.errMsg;
				toggle = true;
			}
		}
	}
	
	/*insert word w into database. DON'T use this function*/
	public void insertWord(Word w){
		if(db != null){
			bool d = db.insertWord(w);
			if(d == false){
				description = db.errMsg;
				toggle = true;
			}
		}
	}
	
	/*get word data of word w from database*/
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
	
	/*get word data of word w from database*/
	public Word getWord(char w){
		return getWord(w.ToString());
	}
	
	/*get all words from database*/
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
	
	/*get all string words from database*/
	public Word[] getWords(string r){
		if(db != null){
			char[] str = r.ToCharArray();
			Word[] d = db.getWords(str);
			if(d == null){
				description = db.errMsg;
				toggle = true;
			}
			else{
				for(int i=0; i<str.Length; i++){
					for(int j=i+1; j<str.Length; j++){
						if(str[i].ToString() == d[j].wordName){
							Word w = d[i];
							d[i] = d[j];
							d[j] = w;
						}
					}
				}
			}
			return d;
		}
		else return null;
	}
	
	/*insert a write record into database*/
	public void addRecord(writeRecord r){
		if(db != null && r != null){
			if(db.InsertInto("history", r.toStrings()) == 0){
				description = db.errMsg;
				toggle = true;
			}
		}
	}
	
	/*get all write records*/
	public wordRecord[] getRecords(){
		if(db != null){
			string[] words = db.getRecordWords();
			if(words == null){
				description = db.errMsg;
				toggle = true;
				return null;
			}
			wordRecord[] records = new wordRecord[words.Length];
			for(int i=0;i<words.Length;i++){
				records[i] = db.getWordRecords(words[i]);
			}
			return records;
		}
		else return null;
	}
	
	//get all write records of input word
	public wordRecord getWordRecords(string w){
		if(db != null){
			wordRecord wrecord = db.getWordRecords(w);
			if(wrecord == null){
				description = db.errMsg;
				toggle = true;
				return null;
			}
			return wrecord;
		}
		else return null;
	}
	
	public bool isTerm(Word a, Word b){
		if(db != null){
			return db.isTerm(a, b);
		}
		return false;
	}
	
	public System.DateTime getTime(){
		System.DateTime time = System.DateTime.Now;
		return time;
	}
	
	//get 5 latest write records of input word order by time
	public wordRecord getOrderedRecords(Word r){
		if(db!=null){
			wordRecord records = db.getOrderedRecords(r.wordName, 5);
			if(records == null){
				description = db.errMsg;
				toggle = true;
			}
			return records;
		}
		return null;
	}
	
	//get all themes
	public themeRecord[] getThemes(){
		if(c_themes != null) return c_themes;
		else if(db!=null){
			c_themes = db.getThemes(0);
			if(c_themes == null){
				description = db.errMsg;
				toggle = true;
			}
			return c_themes;
		}
		return null;
	}
	
	//get the theme of given id
	public themeRecord getTheme(int id){
		if(c_themes == null) getThemes();
		
		if(c_themes != null && c_themes.Length > id) return c_themes[id];
		else return null;
	}
	
	//update given theme's score
	public bool updateTheme(int themeId, float score){
		if(c_themes == null) getThemes();
		
		if(c_themes != null && c_themes.Length > themeId) 
			c_themes[themeId].score = score;
		else return false;
		
		if(db != null){
			bool d=db.updateTheme(c_themes[themeId]);
			if(!d){
				description = db.errMsg;
				toggle = true;
			}
			return d;
		}
		else{
			description = "null db";
			toggle = true;
			return false;
		}
	}
	
	//update given theme's score
	public bool updateTheme(themeRecord theme){
		return updateTheme(theme.id, theme.score);
	}
	
	//get all Chapters of given theme
	public chapterRecord[] getCapters(int themeId){
		if(c_themes == null) getThemes();
		
		if(c_themes != null && c_themes.Length > themeId){
			print("c_themes.Length > themeId");
			if(c_themes[themeId].chapters == null || c_themes[themeId].chapters.Length < 1){
				print("c_themes[themeId].chapters.Length < 1");
				if(db!=null){
					print("query");
					c_themes[themeId].chapters = db.getChapters(themeId);
					if(c_themes[themeId].chapters == null){
						description = db.errMsg;
						toggle = true;
					}
				}
				else {
					description = "null db";
					toggle = true;
				}
			}
			return c_themes[themeId].chapters;
		}
		else return null;
	}
	
	//update given chapter's score
	public bool updateCapter(int number, int themeId, float score){
		if(c_themes == null) getThemes();
		
		chapterRecord[] chapters;
		if(c_themes != null && c_themes.Length > themeId){
			chapters = c_themes[themeId].chapters;
			if(chapters == null) chapters = getCapters(themeId);
			
			if(chapters != null && chapters.Length > number) 
				chapters[number].score = score;
		}
		else return false;
		
		if(db != null){
			bool d=db.updateChapter(chapters[number]);
			if(!d){
				description = db.errMsg;
				toggle = true;
			}
			return d;
		}
		else{
			description = "null db";
			toggle = true;
			return false;
		}
	}
	
	//update given chapter's score
	public bool updateCapter(chapterRecord chapter){
		return updateCapter(chapter.id, chapter.themeId, chapter.score);
	}
	
	void OnDestroy(){
		if(db != null){
			db.CloseDB();
		}
	}
	
}

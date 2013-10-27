using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Text;
using Mono.Data.SqliteClient;

public class dbAccess {
	private string connection;
	private IDbConnection dbcon;
	private IDbCommand dbcmd;
	private IDataReader reader;
	private StringBuilder builder;
	
	public const int dbVersion = 1;
	
	public string errMsg = "";
	
	public void OpenDB(string p)
	{
		Debug.Log("Call to OpenDB:" + p);
		// check if file exists in Application.persistentDataPath
		string filepath = Application.persistentDataPath + "/" + p;
		
		if(!File.Exists(filepath))
		{
			Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
			                 Application.dataPath + "!/assets/" + p);
			// if it doesn't ->
			// open StreamingAssets directory and load the db -> 
			WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
			while(!loadDB.isDone) {}
			// then save to Application.persistentDataPath
			File.WriteAllBytes(filepath, loadDB.bytes);
		}
		
		//open db connection
		connection = "URI=file:" + filepath;
		Debug.Log("Stablishing connection to: " + connection);
		dbcon = new SqliteConnection(connection);
		dbcon.Open();
		
		bool dbavalible = true;
		
		try{
			BasicQuery("SELECT ver FROM version");
			if(reader.Read()){
				Debug.Log("read");
				if(reader.GetInt32(0) != dbVersion){
					dbavalible = false;
				}
			}
			else dbavalible = false;
		}
		catch(Exception e){
			Debug.Log(e);
			dbavalible = false;
		}
		
		if(!dbavalible){
			Debug.LogWarning("File \"" + filepath + "\" does not avaliable. Attempting to recreate from \"" +
			                 Application.dataPath + "!/assets/" + p);
			CloseDB();
			// if it doesn't ->
			// open StreamingAssets directory and load the db -> 
			WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);
			while(!loadDB.isDone) {}
			// then save to Application.persistentDataPath
			File.WriteAllBytes(filepath, loadDB.bytes);
			
			//open db connection
			connection = "URI=file:" + filepath;
			Debug.Log("Stablishing connection to: " + connection);
			dbcon = new SqliteConnection(connection);
			dbcon.Open();
		}
	}
	
	public void CloseDB(){
		// clean everything up
		if(reader != null){
			reader.Close(); 
  	 		reader = null;
		}
		if(dbcmd != null){
   			dbcmd.Dispose();
   			dbcmd = null;
		}
   		dbcon.Close();
   		dbcon = null;
	}
	
	public IDataReader BasicQuery(string query){ // run a basic Sqlite query
		dbcmd = dbcon.CreateCommand(); // create empty command
		dbcmd.CommandText = query; // fill the command
		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		return reader; // return the reader
	
	}
	
	
	public bool CreateTable(string name,string[] col, string[] colType){ // Create a table, name, column array, column type array
		string query;
		query  = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
		for(var i=1; i< col.Length; i++){
			query += ", " + col[i] + " " + colType[i];
		}
		query += ")";
		try{
			dbcmd = dbcon.CreateCommand(); // create empty command
			dbcmd.CommandText = query; // fill the command
			reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		}
		catch(Exception e){
			errMsg = e.ToString();
			Debug.Log(e);
			return false;
		}
		return true;
	}
	
	public int InsertIntoSingle(string tableName, string colName , string value ){ // single insert
		string query;
		query = "INSERT INTO " + tableName + "(" + colName + ") " + "VALUES ('" + value + "')";
		try
		{
			dbcmd = dbcon.CreateCommand(); // create empty command
			dbcmd.CommandText = query; // fill the command
			reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		}
		catch(Exception e){
			errMsg = e.ToString();
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public int InsertIntoSpecific(string tableName, string[] col, string[] values){ // Specific insert with col and values
		string query;
		query = "INSERT INTO " + tableName + "(" + col[0];
		for(int i=1; i< col.Length; i++){
			query += ", " + col[i];
		}
		query += ") VALUES ('" + values[0];
		for(int i=1; i< col.Length; i++){
			query += "', '" + values[i];
		}
		query += "')";
		Debug.Log(query);
		try
		{
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			errMsg = e.ToString();
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public int InsertInto(string tableName , string[] values ){ // basic Insert with just values
		string query;
		query = "INSERT INTO " + tableName + " VALUES ('" + values[0];
		for(int i=1; i< values.Length; i++){
			query += "', '" + values[i];
		}
		query += "')";
		try
		{
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		}
		catch(Exception e){
			errMsg = e.ToString();
			Debug.Log(e);
			return 0;
		}
		return 1;
	}
	
	public IDataReader SingleSelectWhere(string tableName , string itemToSelect,string wCol,string wPar, string wValue){ // Selects a single Item
		string query;
		query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue;	
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query;
		reader = dbcmd.ExecuteReader();
		return reader;
		//string[,] readArray = new string[reader, reader.FieldCount];
		/*string[] row = new string[reader.FieldCount];
		ArrayList readArray = new ArrayList();
		while(reader.Read()){
			int j=0;
			while(j < reader.FieldCount)
			{
				row[j] = reader.GetString(j);
				j++;
			}
			readArray.Add(row);
		}
		return readArray; // return matches*/
	}
	
	public string Dquery(string query){ // An evil back door
		try{
			dbcmd = dbcon.CreateCommand(); // create empty command
			dbcmd.CommandText = query; // fill the command
			reader = dbcmd.ExecuteReader(); // execute command which returns a reader
		}
		catch(Exception e){
			errMsg = e.ToString();
			Debug.Log(e);
			return null;
		}
		//string[] row = new string[reader.FieldCount];
		string d = "";
		while(reader.Read()){
			int j=0;
			while(j < reader.FieldCount)
			{
				d += reader.GetString(j) + "\t";
				j++;
			}
			d += "\n";
		}
		return d;
	}
	
	public System.Collections.Generic.List<string> getWords(){
		string query = "SELECT word FROM words";	
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		System.Collections.Generic.List<string> d = new System.Collections.Generic.List<string>();
		while(reader.Read()){
			if(reader.FieldCount == 0){
				Debug.Log("null Field");
				errMsg = "null Field";
				break;
			}
			else{
				d.Add(reader.GetString(0));
			}
		}
		return d;
	}
	
	public Word getWord(string r){
		string query = "SELECT * FROM words WHERE word = '" + r + "'";
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		Word d = null;
		if(reader.Read()){
			if(reader.FieldCount != 9){
				errMsg = "FieldCount error!";
				Debug.Log("FieldCount error!");
			}
			else{
				string name = reader.GetString(0);
				int stroke_count = (int)reader.GetValue(2);
				string imgPath = (string)reader.GetValue(5);
				string strokes = (string)reader.GetValue(6);
				string buso    = (string)reader.GetValue(8);
				try{
					d = new Word(name, strokes, stroke_count, buso);
				}catch (Exception e){
					Debug.Log(e);
					errMsg = e.ToString();
				}
				if(imgPath != null) d.loadImage(imgPath);
			}
		}
		return d;
	}
	
	public bool insertWord(Word w){
		string query = "INSERT INTO words(word, stroke_count, stroke, buso) VALUES ";
		query += "(" + w.wordName + ", " + w.finishIndex + ", " + w.writeStroke() + ", ";
		if(w.buso != null) query += w.buso + ")";
		else query += w.wordName + ")";
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return false;
		}
		return true;
	}
	
	public bool updateWord(Word w){
		string query = "UPDATE words SET stroke_count='" + w.finishIndex.ToString();
		query += "', stroke='" + w.writeStroke() + "' WHERE word='" + w.wordName + "'";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return false;
		}
		return true;
	}
	
	public bool isTerm(Word a, Word b){
		if( a==null || b==null){
			errMsg = "null Word";
			Debug.Log(errMsg);
			return false;
		}
		
		string query = "SELECT * FROM terms WHERE firstWord='" + a.wordName + "'";
		query += "AND secondWord='" + b.wordName + "'";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return false;
		}
		return reader.Read();
	}
	
	public bool isTerm(string a, string b){
		if( a==null || b==null){
			errMsg = "null Word";
			Debug.Log(errMsg);
			return false;
		}
		
		string query = "SELECT * FROM terms WHERE firstWord='" + a + "'";
		query += "AND secondWord='" + b + "'";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return false;
		}
		return reader.Read();
	}
	
	public string[] getRecordWords(){
		string query = "SELECT DISTINCT word FROM history GROUP BY word";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		System.Collections.Generic.List<string> words = new System.Collections.Generic.List<string>();
		
		while(reader.Read()){
			string w = reader.GetValue(0) as string;
			words.Add(w);
		}
		return words.ToArray();
	}
	
	public wordRecord getWordRecords(string r){
		if(r == null) return null;
		
		string query = "SELECT * FROM history WHERE word='" + r + "'";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		wordRecord record = new wordRecord(r);
		
		while(reader.Read()){
			float score = reader.GetFloat(1);
			System.DateTime time = (System.DateTime)reader.GetValue(2);
			record.addRecord(score,time);
		}
		return record;
	}
	
	public string getTime(){
		string query = "SELECT datetime('now', 'localtime');";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		if(reader.Read()){
			string time = reader.GetValue(0) as string;
			return time;
		}
		return null;
	}
	
	public wordRecord getOrderedRecords(string r, int limit){
		if(r == null) return null;
		
		string query = "SELECT * FROM history WHERE word='" + r + "' ORDER BY time DESC";
		if(limit != 0) query += " LIMIT " + limit;
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		wordRecord record = new wordRecord(r);
		
		while(reader.Read()){
			float score = reader.GetFloat(1);
			System.DateTime time = (System.DateTime)reader.GetValue(2);
			record.addRecord(score,time);
		}
		return record;
	}
	
	public themeRecord[] getThemes(int limit){
		string query = "SELECT * FROM themes ORDER BY id";
		if(limit > 0) query += " LIMIT " + limit;
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		System.Collections.Generic.List<themeRecord> themes = new System.Collections.Generic.List<themeRecord>();
		while(reader.Read()){
			int    id      = reader.GetInt32(0);
			string name    = reader.GetString(1);
			string imgPath = reader.GetString(2);
			float  score   = reader.GetFloat(3);
			themeRecord _theme = new themeRecord(id, name, score, imgPath); 
			themes.Add(_theme);
		}
		
		return themes.ToArray();
	}
	
	public chapterRecord[] getChapters(int themeId){
		string query = "SELECT * FROM enemies WHERE themeId = " + themeId + " ORDER BY id";
		
		try {
			dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = query;
			reader = dbcmd.ExecuteReader();
		} catch(Exception e){
			Debug.Log(e);
			errMsg = e.ToString();
			return null;
		}
		
		System.Collections.Generic.List<chapterRecord> chapters = new System.Collections.Generic.List<chapterRecord>();
		while(reader.Read()){
			int    id      = reader.GetInt32(0);
			string name    = reader.GetString(2);
			string imgPath = reader.GetString(3);
			float  score   = reader.GetFloat(4);
			chapterRecord _chapter = new chapterRecord(id, themeId, name, score, imgPath); 
			chapters.Add(_chapter);
		}
		
		return chapters.ToArray();
	}
}
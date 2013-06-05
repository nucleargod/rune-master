using UnityEngine;
using System.Collections;

public class writeRecord {
	public string word;
	public float score;
	public string time;
	
	public writeRecord(string rword, float rscore, string rtime){
		word = rword;
		score = rscore;
		time = rtime;
	}
	
	public string[] toStrings(){
		string[] d = new string[3];
		d[0] = word;
		d[1] = score.ToString();
		d[2] = time;
		return d;
	}
}

using UnityEngine;
using System.Collections;

public class wordRecord {
	public string word;
	public System.Collections.Generic.List<writeRecord> records;
	
	public wordRecord(string r){
		word = r;
		records = new System.Collections.Generic.List<writeRecord>();
	}
	
	public void addRecord(float score, string time){
		writeRecord s = new writeRecord(word,score,time);
		records.Add(s);
	}
	
	/*get average score of this word. O(n)*/
	public float avgScore(){
		float avg = 0;
		for(int i=0;i<records.Count;i++){
			avg += records[i].score;
		}
		avg /= records.Count;
		return avg;
	}
}

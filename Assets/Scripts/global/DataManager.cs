using UnityEngine;
using System.Collections;

// data struct
public struct ChapterSet {
	public int id;
	public string name;
	public ChapterStatus status;
}

public struct ThemeSet {
	public int id;
	public string name;
	public Texture texture;
}

public class DataManager : MonoBehaviour {
	public static DataManager Instants = null;
	
	// Use this for initialization
	void Start () {
		if(Instants == null) {
			Instants = this;
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
		}
		else {
			Debug.Log("DataManager is already exists");
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

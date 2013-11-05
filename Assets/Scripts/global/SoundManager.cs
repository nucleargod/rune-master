using UnityEngine;
using System.Collections;

// 音樂管理員

public class SoundManager : MonoBehaviour {
	public static SoundManager Instance = null;
	
	// Use this for initialization
	void Start () {
		if (Instance == null)
		{
			Instance = this;
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Debug.Log("SoundManager is already exists");
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

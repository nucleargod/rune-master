using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
	public static Global Instants = null;
	// Use this for initialization
	void Start () {
		if (Instants == null)
		{
			Instants = this;
			DontDestroyOnLoad(this.gameObject);
			this.enabled = false;
		}
		else
		{
			Debug.Log("Global is already exists");
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

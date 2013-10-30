using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public GameObject cub;
	// Use this for initialization
	void Start () {
		Quaternion q = Quaternion.Euler(0.0f, 0.0f, -45.0f);
		for(int i = 0; i < 1000; i++)
		{
			for(int j = 0; j < 10; j++)
			{
				if (i==0)
					Instantiate(cub, new Vector3(i*0.5f, 0, j), q);
				else
					Instantiate(cub, new Vector3(i*0.5f, 0, j), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

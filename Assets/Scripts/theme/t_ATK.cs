using UnityEngine;
using System.Collections;

public class t_ATK : MonoBehaviour {
	
	public string toShow = "hello";
	private float scale;
	
	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<TextMesh>().text = toShow;
		gameObject.rigidbody.AddForce(new Vector3(Random.Range(-3, 3), 5, 0), ForceMode.Impulse);
		
		scale = 0.1f;
		
		Destroy(gameObject, 1.2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		scale += Time.deltaTime*0.5f;
		if(scale > 1.0f) scale = 1.0f;
		gameObject.transform.localScale = new Vector3(scale, scale, 1.0f);
	}
}

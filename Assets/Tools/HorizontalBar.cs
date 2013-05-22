using UnityEngine;
using System.Collections;

public class HorizontalBar : MonoBehaviour {
	
	public Rect area = new Rect(0, 0, 100, 5);
	public Texture2D backImage;
	public Texture2D frontImage;
	
	void Start()
	{
		area.width  = Screen.width  * area.width;
		area.height = Screen.height * area.height;
		area.x  = Screen.width  * area.x;
		area.y = Screen.height * area.y;
	}
	
	private Rect targetRect;
	public void DrawBaWorld(Vector3 worldPos, float curValue, float maxValue)
	{
		if(curValue > maxValue) curValue = maxValue;
		else if(curValue < 0) curValue = 0;
		Vector3 ScrPos = Camera.main.WorldToScreenPoint(worldPos);
		if(ScrPos.z < 0) return;
		targetRect.x = ScrPos.x + area.x;
		targetRect.y = Screen.height - (ScrPos.y + area.y);
		targetRect.width = area.width;
		targetRect.height = area.height;
		GUI.DrawTexture(targetRect, backImage);
		targetRect.width = targetRect.width*(curValue/maxValue);
		GUI.DrawTexture(targetRect, frontImage);
	}
	
	public void DrawBarViewer(Vector3 ScrPos, float curValue, float maxValue)
	{
		if(curValue > maxValue) curValue = maxValue;
		else if(curValue < 0) curValue = 0;
		//Vector3 ScrPos = Camera.main.WorldToScreenPoint(worldPos);
		if(ScrPos.z < 0) return;
		targetRect.x = ScrPos.x + area.x;
		targetRect.y = Screen.height - (ScrPos.y + area.y);
		targetRect.width = area.width;
		targetRect.height = area.height;
		GUI.DrawTexture(targetRect, backImage);
		targetRect.width = targetRect.width*(curValue/maxValue);
		GUI.DrawTexture(targetRect, frontImage);
	}
}

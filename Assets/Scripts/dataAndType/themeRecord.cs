using UnityEngine;
using System.Collections;

[System.Serializable]
public class themeRecord {
	//-1: no such record
	public int				id = -1;
	public readonly string	name;
	private Texture			image;
	public Texture 			img{ get { return image;}}
	public float			score = 0.0f;
	public ThemeStatus		status = ThemeStatus.locked;
	//public chapterRecord[]	chapters
	
	public enum ThemeStatus{
		locked,
		unlocked,
	}
	
	public themeRecord(){
		
	}
	
	public themeRecord(int _id, string _name, float _score, string _imgPath = null){
		id    = _id;
		name  = _name;
		score = _score;
		if(_imgPath != null) loadImage(_imgPath);
	}
	
	public void unlock(){
		status = ThemeStatus.unlocked;
	}
	
	public bool loadImage(string path){
		image = (Texture2D)(Resources.Load(path));
		return image != null;
	}
}

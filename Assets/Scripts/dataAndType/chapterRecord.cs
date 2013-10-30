using UnityEngine;
using System.Collections;

[System.Serializable]
public class chapterRecord {
	//-1: no such chapter
	public readonly int		id = -1;
	//-1: no such theme
	public readonly int		themeId = -1;
	public readonly string	name;
	private Texture			image;
	public Texture 			img{ get { return image;}}
	public float			score;
	public ChapterStatus	status = ChapterStatus.unlocked;
	
	public enum ChapterStatus{
		locked,
		unlocked,
	}
	
	public chapterRecord(){
		
	}
	
	public chapterRecord(int _id, int _themeId, string _name, float _score, string _imgPath = null){
		id      = _id;
		themeId = _themeId;
		name    = _name;
		score   = _score;
		if(_imgPath != null) loadImage(_imgPath);
	}
	
	public void unlock(){
		status = ChapterStatus.unlocked;
	}
	
	public bool loadImage(string path){
		image = (Texture2D)(Resources.Load(path));
		return image != null;
	}
}

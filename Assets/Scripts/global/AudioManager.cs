using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 音樂管理員

public class AudioManager : MonoBehaviour {
	public static AudioManager Instance = null;
	
	private bool isVolumeChanged = false; // 音量是否被改動
	[SerializeField]
	private float mainVolume =	1.0f; // 主音量
	[SerializeField]
	private float musicVolume =	1.0f; // 音樂音量
	[SerializeField]
	private float soundVolume =	1.0f; // 音效音量
	//-------------------------------------------------------------------------------
	public class AudioUnit {
		public AudioSource audioSource;
		public float originVolume;
		
		public AudioUnit (AudioSource _audioSource, float _volume = 1.0f) {
			this.audioSource = _audioSource;
			originVolume = _volume;
		}
	}
	//-------------------------------------------------------------------------------
	public Dictionary<string, AudioUnit> audioList = new Dictionary<string, AudioUnit>(); // 所有音樂資料
	// Use this for initialization
	void Start () {
		if (Instance == null)
		{
			Instance = this;
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
			AudioManager.LoadAllAudio();
			this.enabled = true;
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
	
	void OnGUI () {
		foreach(KeyValuePair<string, AudioUnit> kvp in audioList) {
			AudioUnit audioUnit = kvp.Value;
			if (GUILayout.Button(audioUnit.audioSource.clip.name)) {
				audioUnit.audioSource.Play();
			}
		}
		
	}
	
	static void LoadAllAudio() {
		// 將所有聲音檔讀入
		foreach (Object obj in Resources.LoadAll("Sounds/")) {
			AudioClip audioClip = (AudioClip)obj;
			GameObject go = new GameObject();
			go.name = audioClip.name;
			go.transform.parent = Instance.transform;
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			
			Instance.audioList.Add(audioClip.name, new AudioUnit(audioSource));
		}
	}
	
	static void Play(string _name, bool _loop = false) {
		AudioUnit audioUnit = Instance.audioList[_name];
		if(audioUnit != null) {
			audioUnit.audioSource.loop = _loop;
			audioUnit.audioSource.Play();
		}
	}
}

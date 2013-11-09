// #define _TEST_AUDIOMANAGER_
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
		
		public AudioUnit (AudioClip _audioClip, float _volume = 1.0f) {
			GameObject go = new GameObject();
			go.name = _audioClip.name;
			go.transform.parent = Instance.transform;
			audioSource = go.AddComponent<AudioSource>();
			audioSource.clip = _audioClip;
			originVolume = _volume;
		}
		
		public void Play(bool _loop = false){audioSource.loop = _loop; audioSource.Play();}
		public void Pause(){audioSource.Pause();}
		public void Stop(){audioSource.Stop();}
	}
	//-------------------------------------------------------------------------------
	public Dictionary<string, AudioUnit> audioList = new Dictionary<string, AudioUnit>(); // 所有讀入記憶體的音樂資料
	
	// Use this for initialization
	void Start () {
		if (Instance == null)
		{
			Instance = this;
			this.enabled = false;
			DontDestroyOnLoad(this.gameObject);
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
#if _TEST_AUDIOMANAGER_
	bool ox = true;
	void OnGUI () {
		foreach(KeyValuePair<string, AudioUnit> kvp in audioList) {
			AudioUnit audioUnit = kvp.Value;
			if (GUILayout.Button(audioUnit.audioSource.clip.name)) {
				audioUnit.audioSource.Play();
			}
		}
		if ( ox && GUILayout.Button("LoadAllAudio")) {
			AudioManager.LoadAllAudio();
			ox = !ox;
		}
		if (!ox && GUILayout.Button("UnloadAllAudio")) {
			AudioManager.UnloadAllAudio();
			ox = !ox;
		}
	}
#endif // _TEST_AUDIOMANAGER_
	// 尚未讀取才會去讀取
	// 建議在初始化場景時就先讀取
	// 並留下AudioUnit
	public static void LoadSound(string _name, out AudioUnit _audioUnit) {
		// 檢查是否已經讀取
		if (Instance.audioList.ContainsKey(_name)) {
			// 已經存在則直接回傳
			_audioUnit = Instance.audioList[_name];
		}
		else {
			// 不存在則試著產生
			AudioClip audioClip = (AudioClip)Resources.Load("Sounds/"+_name);
			if (audioClip != null) {
				// 有該資源則產生
				_audioUnit = new AudioUnit(audioClip);
				Instance.audioList.Add(_name, _audioUnit);
			}
			else {
				// 沒有該資源則return null
				_audioUnit = null;
			}
		}
	}
	
	public static void Play(string _name, bool _loop = false) {
		if(Instance.audioList.ContainsKey(_name)) {
			// 存在則撥放
			Instance.audioList[_name].Play(_loop);
		}
		else {
			// 不存在則讀取
			AudioUnit audioUnit = null;
			LoadSound(_name, out audioUnit);
			if(audioUnit != null) {
				// 讀取成功則撥放
				audioUnit.Play(_loop);
			}
		}
	}
	
	public static void LoadAllAudio() {
		// 將所有聲音檔讀入
		foreach (Object obj in Resources.LoadAll("Sounds/")) {
			AudioClip audioClip = (AudioClip)obj;
			Instance.audioList.Add(audioClip.name, new AudioUnit(audioClip));
		}
	}
	
	public static void UnloadAllAudio() {
		foreach(KeyValuePair<string, AudioUnit> kvp in Instance.audioList) {
			AudioUnit audioUnit = kvp.Value;
			audioUnit.Stop();
			Resources.UnloadAsset(audioUnit.audioSource.clip);
			Destroy(audioUnit.audioSource.gameObject);
		}
		Instance.audioList.Clear();
	}
}

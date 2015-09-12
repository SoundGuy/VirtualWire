using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientGUI : MonoBehaviour {

	public static ClientGUI Instance;

	public Image Blinker;

	public Text commandText;

#if UNITY_EDITOR || UNITY_STANDALONE
	public AudioClip[] ClipsPC;
#endif
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
	public AudioClip[] ClipsMobile;
#endif

	public AudioSource[] Sounds;

	private int bpm;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.131f;
	bool onOff;
	bool colorOnOff;

	void Awake () {
		Instance=this;
		StartBPM();

		for(int i=0; i<Sounds.Length; i++)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			Sounds[i].clip = ClipsPC[i];
#else
			Sounds[i].clip = ClipsMobile[i];
#endif
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBPM();
	}

	void StartBPM()
	{
		onOff = false;
		started = false;
		bpm = 128;
		LastBlink = 0;//Time.time;
		MSDiff = (Utils.GetMS (bpm));

		Blinker.color = new Color(1, 1, 1, 0.5f);

		if(SyncrotronSyncData.Instance != null && SyncrotronSyncData.Instance.BPM!=0)
		{
			SetBPM(SyncrotronSyncData.Instance.BPM);
		}
	}

	public void SetBPM(int value)
	{
		bpm = value;
		MSDiff = (Utils.GetMS (bpm));
		RestartBPM();
	}

	void RestartBPM()
	{
		started = true;
		onOff = false;
		LastBlink = 0;
		Blinker.color = new Color(1, 1, 1, 0.5f);
	}

	void UpdateBPM()
	{
		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					LastBlinkStart = Time.time;
					Blinker.color = new Color(1, 1, 1, 1);
					onOff = true;
					if(colorOnOff)
					{
						colorOnOff = false;
						Sounds[1].Play();
					}else{
						colorOnOff = true;
						Sounds[2].Play();
					}
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					Blinker.color = new Color(1, 1, 1, 0.5f);
					onOff = false;
				}
			}
		}
	}

	public static void UpdateCommandText(string text)
	{
		if(Instance != null)
		{
			Instance.commandText.text=text;
		}
	}

	public static void LoadClientScene()
	{
		Application.LoadLevelAdditive("ClientScene");
	}
}

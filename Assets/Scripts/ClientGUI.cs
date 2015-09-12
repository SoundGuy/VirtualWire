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
	int CurrentBeat;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.131f;
	bool onOff;
	bool colorOnOff;

	int [] ColorPattern;

	void Awake () {
		Instance=this;
		StartBPM();
		ColorPattern = new int[8];
		CurrentBeat = 0;

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
					CurrentBeat++;
					if (CurrentBeat ==8) {
						CurrentBeat =0;
					}
					switch (ColorPattern[CurrentBeat]) {
					case 0:
						Sounds[36].Play();
						break;
					case 1:
						Sounds[11].Play();
						Blinker.color = new Color(1, 0, 0, 1);
						break;
					case 2:
						Sounds[9].Play();
						Blinker.color = new Color(0, 1, 0, 1);
						break;
					case 3:
						Sounds[8].Play();
						Blinker.color = new Color(0, 0, 1, 1);
						break;
					}

					/*

					if(colorOnOff)
					{
						colorOnOff = false;
						Sounds[1].Play();
					}else{
						colorOnOff = true;
						Sounds[2].Play();
					}*/
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

		if (text [0] == 'p') { // Pattern
			for (int i=0;i<8;i++) {
				switch (text[i+1]) {
				case  'K': {
					Instance.ColorPattern[i] =0;
					break;
				}
				case  'R': {
					Instance.ColorPattern[i] =1;
					break;
				}
				case  'G': {
					Instance.ColorPattern[i] =2;
					break;
				}
				case  'B': {
					Instance.ColorPattern[i] =3;
					break;
				}
				}
			}

		}
	}

	public static void LoadClientScene()
	{
		Application.LoadLevelAdditive("ClientScene");
	}
}

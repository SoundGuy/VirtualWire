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
	public float rhythm=1f;
	int CurrentBeat;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.131f;
	bool onOff;
	bool colorOnOff;

	string[] ColorPatternLetter;

	private int currentPattern;


	public void UpdateMSDiff()
	{
		MSDiff = (Utils.GetMS (bpm, rhythm));
	}

	void Awake () {
		Instance=this;
		StartBPM();
		ColorPatternLetter = new string[8];
		CurrentBeat = 0;
		currentPattern = 0;

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
		rhythm=1f;
		LastBlink = 0;//Time.time;
		UpdateMSDiff();

		Blinker.color = new Color(1, 1, 1, 0.5f);

		if(SyncrotronSyncData.Instance != null && SyncrotronSyncData.Instance.BPM!=0)
		{
			SetBPM(SyncrotronSyncData.Instance.BPM);
		}
	}

	public void SetBPM(int value)
	{
		bpm = value;
		UpdateMSDiff();
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

					switch(ColorPatternLetter[CurrentBeat])
					{
					case "K":
						Sounds[36].Play();
						Blinker.color = new Color(0, 0, 0, 1);
						break;
					case "B":
						Sounds[1+(7*currentPattern)].Play();
						Blinker.color = new Color(0, 0, 1, 1);
						break;
					case "G":
						Sounds[2+(7*currentPattern)].Play();
						Blinker.color = new Color(0, 1, 0, 1);
						break;
					case "C":
						Sounds[3+(7*currentPattern)].Play();
						Blinker.color = new Color(0, 1, 1, 1);
						break;
					case "R":
						Sounds[4+(7*currentPattern)].Play();
						Blinker.color = new Color(1, 0, 0, 1);
						break;
					case "P":
						Sounds[5+(7*currentPattern)].Play();
						Blinker.color = new Color(1, 0, 1, 1);
						break;
					case "Y":
						Sounds[6+(7*currentPattern)].Play();
						Blinker.color = new Color(1, 1, 0, 1);
						break;
					case "W":
						Sounds[7+(7*currentPattern)].Play();
						Blinker.color = new Color(1, 1, 1, 1);
						break;
					}
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					if(currentPattern != 0)
					{
						Blinker.color = new Color(1, 1, 1, 0.5f);
					}
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
				Instance.ColorPatternLetter[i] = ""+text[i+1];
			}
			if(text.Length>9)
			{
				Instance.CurrentBeat = int.Parse(""+text[9]);
				Instance.currentPattern = int.Parse(""+text[10]);
			}
		}
		
		if (text == "r1")
		{
			Instance.rhythm = 1f;
			Instance.UpdateMSDiff();
		}
		if (text == "r2")
		{
			Instance.rhythm = 2f;
			Instance.UpdateMSDiff();
		}
		if (text == "r3")
		{
			Instance.rhythm = 4f;
			Instance.UpdateMSDiff();
		}
		if (text == "r4")
		{
			Instance.rhythm = 8f;
			Instance.UpdateMSDiff();
		}
	}

	public static void LoadClientScene()
	{
		Application.LoadLevelAdditive("ClientScene");
	}
}

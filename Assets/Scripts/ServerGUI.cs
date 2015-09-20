using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerGUI : MonoBehaviour {

	public static ServerGUI Instance;

	// Manual stuff
	public GroupToggleRGB[] GroupToggleRGBs;
	public RectTransform ManualMarker;
	private int currentToggleGroupRGB = 0;

	// Patterns stuff
	public Toggle[] TogglePatterns;
	private int currentPattern;

	// Rhythm stuff
	public Toggle[] ToggleRhythm;

	float rhythm=1;


	// BPM stuff

#if UNITY_EDITOR || UNITY_STANDALONE
	public AudioClip[] ClipsPC;
#endif
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
	public AudioClip[] ClipsMobile;
#endif

	public AudioSource[] Sounds;

	public Text BPMInput;
	public Image Blinker;
	public Sprite BlinkOn;
	public Sprite BlinkOff;
	public Slider SliderBPM;

	int bpm;
	int clickCount;
	float [] clickTimes;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.131f;
	bool onOff;

	void Awake () {
		Instance=this;
		ResetGUI();

		for(int i=0; i<Sounds.Length; i++)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			Sounds[i].clip = ClipsPC[i];
#else
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Sounds[i].clip = ClipsMobile[i];
#endif
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBPM();
		if (Input.GetKey ("m")) {
			Debug.Log("Pressed Manual");
			PressSetManual();
		}

#if !UNITY_EDITOR && !UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
#endif
	}

	public void ResetGUI()
	{
		foreach(GroupToggleRGB GToggleRGB in GroupToggleRGBs)
		{
			GToggleRGB.ToggleR.isOn = false;
			GToggleRGB.ToggleG.isOn = false;
			GToggleRGB.ToggleB.isOn = false;
		}
		foreach(Toggle togglePattern in TogglePatterns)
		{
			togglePattern.isOn = false;
		}
		TogglePatterns[0].isOn = true;
		currentPattern = 0;
		ToggleRhythm[0].isOn = true;
		for(int i=1;i<ToggleRhythm.Length;i++)
		{
			ToggleRhythm[i].isOn = false;
		}
		rhythm = 1f;

		StartBPM();
	}

	void StartBPM()
	{
		onOff = false;
		started = false;
		bpm = 128;
		rhythm=1f;
		SliderBPM.value = bpm;
		LastBlink = -10;//Time.time;
		MSDiff = (Utils.GetMS (bpm, rhythm));
		Blinker.sprite = BlinkOff;
		BPMInput.text = bpm.ToString();
		
		clickCount = 0;
		clickTimes = new float[4];
	}

	public void PressSetManual() {
		string command = "p";
		for (int i=0;i<8;i++) {

			command +=GroupToggleRGBs[i].GetColorLetter();


		}

		command += currentToggleGroupRGB;
		command += currentPattern;
		Debug.Log (command);

		SyncrotronSyncData.SendCommandToClient(command);
	}

	void UpdateBPM()
	{
		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					Blinker.sprite = BlinkOn;
					LastBlinkStart = Time.time;
					onOff = true;

					currentToggleGroupRGB++;
					if(currentToggleGroupRGB >= GroupToggleRGBs.Length)
					{
						currentToggleGroupRGB = 0;
					}

					ManualMarker.localPosition= new Vector3(GroupToggleRGBs[currentToggleGroupRGB].transform.localPosition.x, ManualMarker.localPosition.y, ManualMarker.localPosition.z);
					switch(GroupToggleRGBs[currentToggleGroupRGB].GetColorLetter())
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
						Blinker.sprite = BlinkOff;
						Blinker.color = new Color(1, 1, 1, 1);
					}
					onOff = false;
				}
			}
		} 
	}

	public void SetBpmFromSlider()
	{
		BPMInput.text = ""+(int)SliderBPM.value;
	}

	public void IncreaseBPM()
	{
		BPMInput.text = "" + (int.Parse( BPMInput.text) + 1 );
		SliderBPM.value = int.Parse( BPMInput.text);
	}

	public void DecreaseBPM()
	{
		BPMInput.text = "" + (int.Parse( BPMInput.text) -1 );
		SliderBPM.value = int.Parse( BPMInput.text);
	}

	public void PressGO() {
		started = true;
		LastBlink = -10;
		onOff = false;

		bpm = int.Parse( BPMInput.text);
		SliderBPM.value = bpm;
		MSDiff = (Utils.GetMS (bpm, rhythm));

		
		SyncrotronSyncData.UpdateBPM(bpm);
		
		clickCount = 0;

	}

	public void PressBPM()
	{
		/*
		if (clickCount != 0) {
			Debug.Log ("diff" + (Time.time - clickTimes[clickCount]) );
			if (Time.time - clickTimes [clickCount] > 2) {
				clickCount = 0;
			}
		}*/

		clickTimes[clickCount++] = Time.time;

		if (clickCount == 4) {
			started = true;
			//	float [] diffs = new float[4];
			float tot=0;
			for (int i=0;i<3;i++) {
				
				//		diffs[i] = clickTimes[i +1] - clickTimes[i];
				tot += clickTimes[i +1] - clickTimes[i];
			}
			float avg = tot /3;
			Debug.Log( "avg =" + avg);
			onOff = false;
			LastBlink = -10;
			bpm = (int)Utils.GetBMP (avg);
			SliderBPM.value = bpm;
			MSDiff = (Utils.GetMS (bpm, rhythm));
			BPMInput.text= bpm.ToString();

			SyncrotronSyncData.UpdateBPM(bpm);

			clickCount = 0;
		}
	}

	public void ClickRhythm14()
	{
		rhythm = 1f;
		ToggleRhythm[0].isOn = true;
		ToggleRhythm[1].isOn = false;
		ToggleRhythm[2].isOn = false;
		ToggleRhythm[3].isOn = false;
		MSDiff = (Utils.GetMS (bpm, rhythm));
		SyncrotronSyncData.SendCommandToClient("r1");
	}
	
	public void ClickRhythm18()
	{
		rhythm = 2f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = true;
		ToggleRhythm[2].isOn = false;
		ToggleRhythm[3].isOn = false;
		MSDiff = (Utils.GetMS (bpm, rhythm));
		SyncrotronSyncData.SendCommandToClient("r2");
	}
	
	public void ClickRhythm116()
	{
		rhythm = 4f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = false;
		ToggleRhythm[2].isOn = true;
		ToggleRhythm[3].isOn = false;
		MSDiff = (Utils.GetMS (bpm, rhythm));
		SyncrotronSyncData.SendCommandToClient("r3");
	}
	
	public void ClickRhythm132()
	{
		rhythm = 8f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = false;
		ToggleRhythm[2].isOn = false;
		ToggleRhythm[3].isOn = true;
		MSDiff = (Utils.GetMS (bpm, rhythm));
		SyncrotronSyncData.SendCommandToClient("r4");
	}

	public void ClickPattern(int patternNumber)
	{
		foreach(Toggle togglePattern in TogglePatterns)
		{
			togglePattern.isOn = false;
		}
		TogglePatterns[patternNumber].isOn = true;
		currentPattern = patternNumber;
	}

	public void ClickReset()
	{
		ResetGUI();
	}

	public static void LoadServerScene()
	{
		Application.LoadLevelAdditive("ServerScene");
	}
}

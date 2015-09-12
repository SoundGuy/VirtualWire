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

	// Rhythm stuff
	public Toggle[] ToggleRhythm;

	float rhythm;


	// BPM stuff

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
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBPM();
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
		ToggleRhythm[0].isOn=true;
		for(int i=1;i<ToggleRhythm.Length;i++)
		{
			ToggleRhythm[i].isOn=false;
		}
		rhythm = 1f;

		StartBPM();
	}

	void StartBPM()
	{
		onOff = false;
		started = false;
		bpm = 128;
		SliderBPM.value = bpm;
		LastBlink = 0;//Time.time;
		MSDiff = (Utils.GetMS (bpm));
		Blinker.sprite = BlinkOff;
		BPMInput.text = bpm.ToString();
		
		clickCount = 0;
		clickTimes = new float[4];
	}

	void UpdateBPM()
	{
		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					Blinker.sprite = BlinkOn;
					LastBlinkStart = Time.time;
					onOff = true;

					ManualMarker.localPosition= new Vector3(GroupToggleRGBs[currentToggleGroupRGB].transform.localPosition.x, ManualMarker.localPosition.y, ManualMarker.localPosition.z);
					currentToggleGroupRGB++;
					if(currentToggleGroupRGB >= GroupToggleRGBs.Length)
					{
						currentToggleGroupRGB = 0;
					}
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					Blinker.sprite = BlinkOff;
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
		LastBlink = Time.time;

		bpm = int.Parse( BPMInput.text);
		SliderBPM.value = bpm;
		MSDiff = (Utils.GetMS (bpm));

		
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
			
			LastBlink = clickTimes[0];
			bpm = (int)Utils.GetBMP (avg);
			SliderBPM.value = bpm;
			MSDiff = (Utils.GetMS (bpm));
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
	}
	
	public void ClickRhythm18()
	{
		rhythm = 0.5f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = true;
		ToggleRhythm[2].isOn = false;
		ToggleRhythm[3].isOn = false;
	}
	
	public void ClickRhythm116()
	{
		rhythm = 0.25f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = false;
		ToggleRhythm[2].isOn = true;
		ToggleRhythm[3].isOn = false;
	}
	
	public void ClickRhythm132()
	{
		rhythm = 0.125f;
		ToggleRhythm[0].isOn = false;
		ToggleRhythm[1].isOn = false;
		ToggleRhythm[2].isOn = false;
		ToggleRhythm[3].isOn = true;
	}

	public static void LoadServerScene()
	{
		Application.LoadLevelAdditive("ServerScene");
	}
}

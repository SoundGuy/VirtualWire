﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerGUI : MonoBehaviour {

	public static ServerGUI Instance;


	// BPM stuff

	public InputField BPMInput;
	public Image Blinker;
	int bpm;
	int clickCount;
	float [] clickTimes;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.100f;
	bool onOff;

	void Awake () {
		Instance=this;
		StartBPM();
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
		MSDiff = (GetMS (bpm));
		Blinker.color = Color.black;
		BPMInput.text = bpm.ToString();
		
		clickCount = 0;
		clickTimes = new float[4];
	}

	void UpdateBPM()
	{
		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					Blinker.color = Color.white;
					LastBlinkStart = Time.time;
					onOff = true;
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					Blinker.color = Color.black;
					onOff = false;
				}
			}
		} 
	}

	float GetMS(int bpm) {
		return 60000f / bpm /1000f;
	}

	float GetBMP(float sec) {
		return 60f/sec;
	}

	public void PressBPM()
	{
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
			bpm = (int)GetBMP (avg);
			MSDiff = (GetMS (bpm));
			BPMInput.text= bpm.ToString();

			SyncrotronSyncData.UpdateBPM(bpm);

			clickCount = 0;
		}
	}

	public static void LoadServerScene()
	{
		Application.LoadLevelAdditive("ServerScene");
	}
}

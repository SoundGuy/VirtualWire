using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientGUI : MonoBehaviour {

	public static ClientGUI Instance;

	public Text commandText;

	public AudioSource sound1;
	public AudioSource sound2;

	private int bpm;
	bool started;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.080f;
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
		MSDiff = (Utils.GetMS (bpm));

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
	}

	void UpdateBPM()
	{
		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					LastBlinkStart = Time.time;
					sound1.Play ();
					onOff = true;
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					sound2.Play ();
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

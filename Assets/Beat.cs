using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Beat : MonoBehaviour {


	int bpm;

	public Text debug;	
	public Text lastMS;	
	public Text LenMS;
	public InputField BPMInput;
	float MSDiff;
	float LastBlink;
	float LastBlinkStart;
	float BlinkLength = 0.100f;
	public Image Blinker;
	bool onOff;
	public AudioSource sound1;
	public AudioSource sound2;
	int clickCount;
	float [] clickTimes;

	bool started;



	float GetMS(int bpm) {
		return 60000f / bpm /1000f;
	}

	float GetBMP(float sec) {
		return 60f/sec;
	}
	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
		if (bpm.ToString() != BPMInput.text) {
			Debug.Log("new BPM");
		}
		LenMS.text = MSDiff.ToString();

		debug.text = "time = " + Time.time + "last " + LastBlink + " last + diff" + (LastBlink + MSDiff) ;

		if (started) {
			if (onOff == false) {
				if (Time.time > LastBlink + MSDiff) {
					Blinker.color = Color.white;
					LastBlinkStart = Time.time;
					sound1.Play ();
					onOff = true;
				}
			} else {
				if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					Blinker.color = Color.black;
					sound2.Play ();
					onOff = false;


							
				}
			}
		}   

	
	}


	public void pressBPM() {



		clickTimes[clickCount++] = Time.time;
		sound1.Play ();

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
		}
	}
}

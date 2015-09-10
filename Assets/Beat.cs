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



	float GetMS(int bpm) {
		return 60000f / bpm /1000f;
	}

	// Use this for initialization
	void Start () {
		onOff = false;
		bpm = 128;
		LastBlink = 0;//Time.time;
		MSDiff = (GetMS (bpm));
		Blinker.color = Color.black;
		BPMInput.text = bpm.ToString();

		clickCount = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (bpm.ToString() != BPMInput.text) {
			Debug.Log("new BPM");
		}
		LenMS.text = MSDiff.ToString();
		float now = Time.time;

		debug.text = "time = " + Time.time + "last " + LastBlink + " last + diff" + (LastBlink + MSDiff) ;


		if (onOff == false) {
			if (Time.time > LastBlink + MSDiff) {
				Blinker.color = Color.white;
				LastBlinkStart = Time.time;
				sound1.Play();
				onOff = true;
			}
		} else {
			if (Time.time > LastBlink + MSDiff + BlinkLength) {
					LastBlink = LastBlinkStart;
					Blinker.color = Color.black;
				sound2.Play();
				onOff = false;


							
			}
		}
		       

	
	}


	void pressBPM() {

		
	}
}

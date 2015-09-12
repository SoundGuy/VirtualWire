using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamAudioFilter : MonoBehaviour {

	public string CurrentMessage;
	public AudioClip clipTest;

	public bool ignore = false;
	public int count = 0;

	public List<float> allbytes = new List<float>();

	void Start()
	{
		//clipTest.
		//SavWav.Save("test",clipTest);
	}

	//public float[] remainData;

	void OnAudioFilterRead(float[] data, int channels) {
		string dataS="OnAudioFilterRead "+data.Length+" ";
		if(data[0] != 0 && !ignore)
		{
			allbytes.AddRange(data);
			for (var i = 1; i < data.Length; i+=2)
			{
				//data[i] = ;//remainData[i];
				dataS+=data[i]+" , ";
				data[i] = 0;
			}
			//ignore=true;
			count++;
			if(count==2)
			{
				ignore=true;
			}
		}else{
			for (var i = 0; i < data.Length; i++)
			{
				//data[i] = ;//remainData[i];
				dataS+=data[i]+" , ";
				data[i] = 0;
			}
		}
		Debug.LogError(count + "C " + dataS);
	}
}

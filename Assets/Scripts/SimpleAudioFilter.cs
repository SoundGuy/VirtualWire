using UnityEngine;
using System.Collections;

public class SimpleAudioFilter : MonoBehaviour {

	private System.Random random;

	void Awake()
	{
		random = new System.Random();
	}

	void OnAudioFilterRead(float[] data, int channels) {
		for (var i = 0; i < data.Length; i++)
		{
			data[i] = (float)GetRandomNumber(-1, 1);		
		}
	}

	double GetRandomNumber(double minimum, double maximum)
	{ 
		return random.NextDouble() * (maximum - minimum) + minimum;
	}
}

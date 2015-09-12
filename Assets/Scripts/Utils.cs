using UnityEngine;
using System;
using System.Collections;

public class Utils {

	public static char CharFromInt(int number)
	{
		return (char)number;
	}

	public static int IntFromChar(char c)
	{
		return (int)Char.GetNumericValue(c);
	}

	public static float GetMS(int bpm, float rhythm=1f)
	{
		return 60000f / (bpm / rhythm) /1000f;
	}
	
	public static float GetBMP(float sec)
	{
		return 60f/sec;
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GroupToggleRGB : MonoBehaviour {

	public Toggle ToggleR;
	public Toggle ToggleG;
	public Toggle ToggleB;

	public string GetColorLetter()
	{
		string col = "K";
		if(ToggleR.isOn)
		{
			col = "R";
			if(ToggleG.isOn)
			{
				col = "Y";
				if(ToggleB.isOn)
				{
					col = "W";
				}
			}
			else if(ToggleB.isOn)
			{
				col = "P";
			}
		}
		else if(ToggleG.isOn)
		{
			col = "G";
			if(ToggleB.isOn)
			{
				col = "C";
			}
		}
		else if(ToggleB.isOn)
		{
			col = "B";
		}
		return col;
	}
}

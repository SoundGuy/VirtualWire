using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClientGUI : MonoBehaviour {

	public static ClientGUI Instance;

	public Text commandText;

	void Awake () {
		Instance=this;
	}
	
	// Update is called once per frame
	void Update () {
	
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

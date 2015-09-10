using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerGUI : MonoBehaviour {

	public static ServerGUI Instance;

	void Awake () {
		Instance=this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void LoadServerScene()
	{
		Application.LoadLevelAdditive("ServerScene");
	}
}

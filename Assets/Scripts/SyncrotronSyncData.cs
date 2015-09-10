using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[NetworkSettings(channel=1,sendInterval=0.05f)]
public class SyncrotronSyncData : NetworkBehaviour {

	public static SyncrotronSyncData Instance;

	public string currentCommand;

	void Awake () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(isServer)
		{
			if(Input.GetKeyDown(KeyCode.P))
			{
				RpcSendCommandToClient("pupik"+Time.time);
			}
		}
	}

	[ClientRpc]
	public void RpcSendCommandToClient(string command)
	{
		currentCommand = command;
		ClientGUI.UpdateCommandText(command);
	}


}

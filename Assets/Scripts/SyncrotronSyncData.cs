using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


[NetworkSettings(channel=1,sendInterval=0.03333334f)]
public class SyncrotronSyncData : NetworkBehaviour {

	public static SyncrotronSyncData Instance;

	public string currentCommand;

	[SyncVar(hook="SetClientBPM")]
	public int BPM;

	void Awake () {
		Instance = this;
	}

	void OnEnable () {
		if(BPM != 0)
		{
			SetClientBPM(BPM);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isServer)
		{
			/*if(Input.GetKeyDown(KeyCode.P))
			{
				RpcSendCommandToClient("pupik"+Time.time);
			}*/
		}
	}

	[ClientRpc]
	public void RpcSendCommandToClient(string command)
	{
		currentCommand = command;
		ClientGUI.UpdateCommandText(command);
	}

	public static void SendCommandToClient(string command)
	{
		if(Instance)
		{
			Instance.RpcSendCommandToClient(command);
		}
	}

	public static void UpdateBPM(int bpm)
	{
		if(Instance)
		{
			Instance.BPM=bpm;
			Instance.RpcSendCommandToClient("BPM"+bpm);
		}
	}

	private void SetClientBPM(int value)
	{
		if(ClientGUI.Instance != null)
		{
			ClientGUI.Instance.SetBPM(value);
		}
	}


}

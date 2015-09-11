using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class SyncrotronNetworkDiscovery : NetworkDiscovery {

	public NetworkManager manager;

	void Awake()
	{
		manager = GetComponent<NetworkManager>();
	}

	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
		var items = data.Split(':');
		if (items.Length == 3 && items[0] == "NetworkManager")
		{
			var items2 = fromAddress.Split(':');
			if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
			{
				manager.networkAddress = items2[items2.Length-1];
				manager.networkPort = Convert.ToInt32(items[2]);
				manager.StartClient();
			}
			
			StopBroadcast();
		}
	}
}

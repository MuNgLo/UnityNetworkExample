using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChattChannel : MonoBehaviour {

	public List<string> chattLog = new List<string>();
	// Use this for initialization
	void Start () {
		name = "ChattChannel";
		transform.parent = GameObject.Find ("_GAMECORE").transform;
		chattLog.Add ("ServerChatt initialized...");
	}
	
	/// <summary>
	/// Send a chatmessage over the network
	/// It has to be marked as an [RPC] and the gameobject has to have a NetworkView attached. The NetworkView can have state sync. Off. As long as it is there you can send RPC's through it.
	/// </summary>
	/// <param name="msg">Textmessage</param>
	[RPC]
	void say (string msg) {
		chattLog.Add (msg); // Add the message to the local List<>. If the RPC was sent buffered it would make this a mess for those connecting midgame. Now we only save new messages after we connected.
		Debug.Log (msg);
	}
	
	public string getLastChatt(int rows=5) {
		string text = "";
		int lastindex = chattLog.Count - 1;
		int index = lastindex - (rows - 1);
		if (index < 0) {
			index = 0;
		}
		//Debug.Log ("getLastChatt  " + index.ToString () + " to " + lastindex.ToString ());
		if (lastindex == 0) {
			return chattLog[0];
		}
		while (index <= lastindex) {
			text = text + "\n" + chattLog[index];
			index++;
		}
		return text;
	}
}

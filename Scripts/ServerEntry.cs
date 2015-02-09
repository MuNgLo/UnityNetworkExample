using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerEntry : MonoBehaviour {
	public int serverID = 0;
	private GameObject serverName;
	private GameObject playerCount;
	private GameObject joinButton;
	private Image background;
	void Awake() {
		background = GetComponent<Image> ();
		serverName = transform.FindChild ("Servername").gameObject;
		playerCount = transform.FindChild ("Playercount").gameObject;
		joinButton = transform.FindChild ("JoinBtn").gameObject;
		serverName.SetActive(false);
		playerCount.SetActive(false);
		joinButton.SetActive(false);
		background.enabled = false;
	}

	void Update () {
		if (MasterServer.PollHostList ().Length > serverID) {
						Show ();
				} else {
						Hide ();
				}
	}
	void Show() {
		UpdateInfo ();
		serverName.SetActive(true);
		playerCount.SetActive(true);
		joinButton.SetActive(true);
		background.enabled = true;
	}
	void Hide(){
		serverName.SetActive(false);
		playerCount.SetActive(false);
		joinButton.SetActive(false);
		background.enabled = false;
	}
	void UpdateInfo() {
		HostData serverInfo = MasterServer.PollHostList () [0];
		serverName.GetComponent<Text>().text = serverInfo.gameName;
		playerCount.GetComponent<Text> ().text = serverInfo.connectedPlayers + "/" + serverInfo.playerLimit;
	}
}

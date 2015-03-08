using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
	#region Game variables
		private string gameKey = "UNE-UnityNetworkExample-v1.1"; // this is what identifies the game on the masterserver. Asking it for servers with this key returns all servers for this game.
		public string hostName = "UNE-000"; // The servername used when registering on the masterserver
		public string hostComment = "Unity Network example server"; // Server comment that is sent to masterserver when registering the server.
		public int hostPort = 27030; // Which port the server you host will run on. This is the port needed to be NATed(forwarded) for maximum connectability. This should be possible to set for the game. It makes it easier to host for those that have some ports already NATed.
		public int maxPlayers = 10; // The number of maximum players. This minus one gives the connections that should be used when initializing the server.
		public bool useNat = true; // Indicates if servers should use the NAT punchthrough functionality of any facilitator. If your masterserver isn't running a facilitator I guess this should be turned off.
		public bool useDefaultMasterServer = true; // this flag controls the if section in Awake(). Allows to quickly switch between your own masterserver or the default one.
		public string masterServerIP = "";
		public int masterServerPort = 0;
		public string natFacilitatorIP = "";
		public int natFacilitatorPort = 0;
		public GameObject spawnLocation; // An object that indicates where players will spawn.
		public GameObject playerAvatarPrefab; // the prefab that players will spawn as.
		//[HideInInspector] // Hides the variable on the next line from the editor inspector. TODO Hide this again
		public GameObject currentPlayerAvatar; // Saves a reference to the playeravatar the player is using.
		public UIAPI ui; // Should be setup in the editor to the gameobject containing the UIAPI script. It is my way of decopeling UI from the rest of the code. Should be no problems using any UI solution through the UIAPI.
		public int maxFPS = 60;
		public bool invertMouse = false;
		[HideInInspector]
		public GameObject
				avatar;
	#endregion
	
		void Awake ()
		{
				Application.targetFrameRate = maxFPS; // This tells the game what FPS to render at. If vsync is enabled it will be ignored.
		if (playerAvatarPrefab == null) { // checks if playerAvatar is not set in Editor and if so loads the default one.
			playerAvatarPrefab = (GameObject)Resources.Load ("PlayerAvatar");
				}
				#region Masterserver settings
				// This is what you use to point the game to your own masterserver.
				// The masterserver software is available through Unity somewhere. Google away!! :D
				if (!useDefaultMasterServer) {
						MasterServer.ipAddress = masterServerIP;
						MasterServer.port = masterServerPort;
						Network.natFacilitatorIP = natFacilitatorIP;
						Network.natFacilitatorPort = natFacilitatorPort;
				}
				#endregion
		}

	#region Spawn related methods
		/// <summary>
		/// This instantiate an avatar for a player if the player don't already have one
		/// </summary>
		/// <param name="player">NetworkPlayer</param>
		public void SpawnIntoGame (NetworkPlayer player)
		{
				// make sure only intended player spawns in
				if (player != Network.player) {
						return;
				}
		//playerAvatarPrefab.transform.FindChild ("Syncer").gameObject.GetComponent<NetSync> ().SetOwner (Network.player.ToString ());
		avatar = (GameObject)Network.Instantiate (playerAvatarPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation, 0);
		}
		/// <summary>
		/// Spawn the specified player into the game. If it is a client it will relay the spawnrequest to server where it will send out the info to all connected.
		/// </summary>
		/// <param name="player">Player.</param>
		[RPC]
		public void Spawn (NetworkPlayer player)
		{
				
				if (!GameObject.Find ("_PLAYERS").transform.FindChild ("Player-" + player.ToString ())) { // Check if we have an avatar to spawn in on and if not spawn one on the nwtwork
						SpawnIntoGame (player);
				}
				if (!Network.isServer) { // If Spawn() wasn't called on server we relay the request to the server
						GetComponent<NetworkView>().RPC ("Spawn", RPCMode.Server, player);
						return;
				}
				if (player.ToString () != "0") { // Make sure we only call doSpawn over RPC if it isn't the server that is spawning
						GetComponent<NetworkView>().RPC ("doSpawn", player); // We only send the doSpawn RPC to the player that is going to spawn
				} else {
						doSpawn ();
				}
		}
		/// <summary>
		/// This is where we take the player Avatar and move it to the spawnlocation.
		/// </summary>
		[RPC]
		public void doSpawn ()
		{
				currentPlayerAvatar.transform.position = spawnLocation.transform.position;
				currentPlayerAvatar.transform.rotation = spawnLocation.transform.rotation;
		}
	#endregion
	
	
	
	#region Server setup, join, disconnect and initialization
		/// <summary>
		/// Request a host list from the master server.
		/// The list is available through MasterServer.PollHostList when it has arrived.
		/// </summary>
		public void GetServers ()
		{
				MasterServer.RequestHostList (gameKey);
		}
		/// <summary>
		/// Start up the server. Initializes the network and when it succeds it calls OnServerInitialized.
		/// </summary>
		public void HostServer ()
		{
				Network.InitializeServer ((maxPlayers - 1), hostPort, useNat);
		}
		/// <summary>
		/// Joins the server where ID is the place in the list. 0 being the first server.
		/// </summary>
		public void JoinServer (int ID)
		{
				Network.Connect (MasterServer.PollHostList () [ID]);
		}
		/// <summary>
		/// Close all open connections and shuts down the network interface.
		/// </summary>
		public void Disconnect ()
		{
				Network.Disconnect ();
				if (GameObject.Find ("Player-0")) {
						Destroy (GameObject.Find ("Player-0"));
				}
		}
	#endregion

	#region Server callback responses
		/// <summary>
		/// Called on the server whenever a Network.InitializeServer was invoked and has completed.
		/// </summary>
		void OnServerInitialized ()
		{
				Debug.Log ("OnServerInitialized()");
				if (hostName == "UNE-000") { // Check if hostName is default and if so randomize the number
						hostName = "UNE-" + Random.Range (100, 999).ToString ();
						GameObject.Find ("UI").transform.FindChild ("ConnectedMenu").FindChild ("ServerNameLabel").GetComponent<Text> ().text = "Server: " + hostName;
				}
				MasterServer.RegisterHost (gameKey, hostName, hostComment); // Register our server on the masterserver so other can find us and connect
		}
		/// <summary>
		/// Called on the server whenever a new player has successfully connected.
		/// </summary>
		void OnPlayerConnected (NetworkPlayer player)
		{
				Debug.Log ("OnPlayerConnected()::Player connected.");
		}
		/// <summary>
		/// Called on the server whenever a player is disconnected from the server. Here we clean up all buffered RPC's and network instatiated objects from the diconnecting player.
		/// </summary>
		void OnPlayerDisconnected (NetworkPlayer player)
		{
				Network.RemoveRPCs (player); // Removes all buffered RPC calls the player made. Like spawning the playerAvatar. That one is buffered so all that connect after also spawn it but that is not needed after the creator has left
				Network.DestroyPlayerObjects (player); // Removes all network.Instantiated objects the disconnecting player have made.
				Debug.Log ("OnPlayerDisconnected( playerID: " + player.ToString () + " )");
		}
	#endregion

	#region Client callback responses
		void OnConnectedToServer ()
		{
				Debug.Log ("Connected to server");
		}

		void OnDisconnectedFromServer (NetworkDisconnection info)
		{
				Debug.Log ("Disconnected from server: " + info);
		}
	#endregion

	#region MasterServerEvents
		/// <summary>
		/// Called on clients or servers when reporting events from the MasterServer.
		/// Like, for example, when a host list has been received or host registration succeeded.
		/// </summary>
		void OnMasterServerEvent (MasterServerEvent msEvent)
		{
				if (msEvent == MasterServerEvent.RegistrationSucceeded) {
						Debug.Log ("Masterserver Registration done");
				} else if (msEvent == MasterServerEvent.RegistrationFailedGameType) {
						Debug.Log ("Failed Masterserver Registration ( Gametype )");
				} else if (msEvent == MasterServerEvent.RegistrationFailedGameName) {
						Debug.Log ("Failed Masterserver Registration ( Gamename )");
				} else if (msEvent == MasterServerEvent.RegistrationFailedNoServer) {
						Debug.Log ("No Masterserver could be found");
				} else if (msEvent == MasterServerEvent.HostListReceived) {
						Debug.Log ("Masterserver List Recieved (" + MasterServer.PollHostList ().Length.ToString () + ")");
				} else {
						Debug.LogWarning ("OnMasterServerEvent():: Match failed! this should never happen!");
				}
		}
	#endregion

	#region Application level stuff
		public void Exit ()
		{
				Application.Quit ();
		}
	#endregion
	
}

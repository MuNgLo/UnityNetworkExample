using UnityEngine;
using System.Collections;

public class UIAPI : MonoBehaviour {
	private Game game; // This automatically sets it self up on Awake(). Keep it private to avoid game.game.game :D
	void Awake() {
			game = GameObject.Find ("_GAMECORE").GetComponent<Game> ();
		}
	public void StartServer() {
			game.HostServer ();
		}
	public void GetServers() {
			game.GetServers ();
		}
	public void JoinServer(int ID) {
			game.JoinServer (ID);
		}
	public void Disconnect() {
			game.Disconnect ();
		}
	public void Exit() {
			game.Exit ();
		}
	public void Spawn() {
		game.Spawn (Network.player);
	}
}

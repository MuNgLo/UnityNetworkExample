using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAPI : MonoBehaviour {
	private Game game; // This automatically sets it self up on Awake(). Keep it private to avoid game.game.game :D
	private InputField chattField;
	void Awake() {
			game = GameObject.Find ("_GAMECORE").GetComponent<Game> ();
			
			// A bit long but GameObject.Find cant locate inactive objects so we go down the hierarchy through transform.findchild
			chattField = GameObject.Find ("UI").transform.FindChild("ConnectedMenu").FindChild("ChatArea").FindChild("ChattInput").GetComponent<InputField> ();
			chattField.gameObject.SetActive (false);
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

	public void ChattStartInput() {
		game.gameObject.GetComponent<ToggleCursor> ().ShowCursor ();
		chattField.gameObject.SetActive (true);
		chattField.Select ();
		chattField.ActivateInputField ();
	}
	public void ChattSend() {
		if (chattField.text != string.Empty) {
			game.say (chattField.text);
		}

		// Empty the inputfield after chat is sent
		chattField.text = string.Empty;

		// Hide the inputfield and cursor when chat is sent
		chattField.gameObject.SetActive (false);
		game.gameObject.GetComponent<ToggleCursor> ().SetCursor (false);
	}
	public void ChattDeselected() {
		chattField.text = string.Empty;
		chattField.gameObject.SetActive (false);
	}
}

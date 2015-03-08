using UnityEngine;
using System.Collections;

public class NetSync : MonoBehaviour {
	private int owner = -1;
	public GameObject cam; // this is the player first person camera. Assign it in the editor inspector.

	 void Update() {
		if (("Player-" + owner.ToString()) != transform.parent.gameObject.name) { // Make sure the gameObject name is reflecting the value of owner variable.
							transform.parent.gameObject.name = "Player-" + owner.ToString ();
				}
		}
	void OnEnable() {
				if (GetComponent<NetworkView>().isMine) {
						owner = int.Parse(Network.player.ToString ());
						transform.parent.gameObject.GetComponent<CharacterController> ().enabled = true;
						transform.parent.gameObject.GetComponent<CharacterMotor> ().enabled = true;
						transform.parent.gameObject.GetComponent<FPSInputController> ().enabled = true;
						cam.GetComponent<Camera> ().enabled = true;
						cam.GetComponent<AudioListener> ().enabled = true;
						cam.transform.FindChild ("Eyes").gameObject.SetActive (false);
				} else {
						cam.GetComponent<Camera> ().enabled = false;
						cam.GetComponent<AudioListener> ().enabled = false;
				}
		}
	void Awake() {
				transform.parent.SetParent (GameObject.Find ("_PLAYERS").transform);
				if (GetComponent<NetworkView>().isMine) {
								GameObject.Find ("_GAMECORE").GetComponent<Game> ().currentPlayerAvatar = this.transform.parent.gameObject;
						}
		}

	/// <summary>
	/// Used to customize synchronization of variables in a script watched by a network view.
	/// It is automatically determined if the variables being serialized should be sent or received, see example below
	/// for a better description. This depends on who owns the object, i.e. the owner sends and all others receive.
	/// </summary>
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		if (stream.isWriting) {
			int ownerC = owner;
			stream.Serialize(ref ownerC);
		} else {
			int ownerZ = owner;
			stream.Serialize(ref ownerZ);
			owner = ownerZ;
		}
	}

}

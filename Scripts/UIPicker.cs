using UnityEngine;
using System.Collections;

public class UIPicker : MonoBehaviour {
	public GameObject mainMenu; 		// In the inspector assign to this the canvas that will be shown when having no network connection
	public GameObject connectedMenu;	// same but the canvas to be shown when having a network connection
	
	// Update is called once per frame
	void Update () {
		if (Network.isClient | Network.isServer) {
			mainMenu.SetActive(false);
			connectedMenu.SetActive(true);
				} else {
			mainMenu.SetActive(true);
			connectedMenu.SetActive(false);
				}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChattFeed : MonoBehaviour {
	public Text chattText;
	private string oldchatt;
	private string newchatt;
	private float lastChange, timeToFade=5f;

	void Start () {
		InvokeRepeating ("updateHUD", 1.0f, 0.2f); // Wait 1 second then start calling UpdateHUD with 0.2 second intervall
	}

	void updateHUD () {
		if (!Network.isClient && !Network.isServer) {
			return;
				}
		GameObject chattChannel = GameObject.Find ("ChattChannel");
		if (chattChannel = null) {
						return;
				}
		newchatt = GameObject.Find ("ChattChannel").GetComponent<ChattChannel> ().getLastChatt ();
		if (oldchatt == newchatt) {
			if(Time.time > lastChange + timeToFade) {
				chattText.gameObject.SetActive(false);
			}
			return;
		}
		lastChange = Time.time;
		chattText.text = newchatt;
		oldchatt = newchatt;
		chattText.gameObject.SetActive (true);
	}
}

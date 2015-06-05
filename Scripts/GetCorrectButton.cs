using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetCorrectButton : MonoBehaviour {
	public Text toggleHelpText;
	public ToggleCursor toggleScript;

	void OnEnable () {
		toggleHelpText.text = toggleScript.toggleCursorKey.ToString () + " " + toggleHelpText.text;
	}
}

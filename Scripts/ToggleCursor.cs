using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ToggleCursor : MonoBehaviour
{
	private bool showCursor = false;
	public KeyCode toggleCursorKey;
	
	void Update ()
	{
		if (Input.GetKeyDown (toggleCursorKey)) {
			toggleCursor ();
		}
	}
	
	private void toggleCursor ()
	{
		if (showCursor) {
			showCursor = false;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			showCursor = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		//Debug.Log ("toggleCursor CALLED!");
		if(!showCursor) {
		EventSystem.current.SetSelectedGameObject (null, null); // This removes the focus from the Spawn button so we don't trigger it when we press space.
		}
	}
}

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
				showCursor = !showCursor;
				Screen.showCursor = showCursor;
				Screen.lockCursor = !showCursor;
				if (!showCursor) {
						EventSystem.current.SetSelectedGameObject (null, null); // This removes the focus from the Spawn button so we don't trigger it when we press space.
				}
		}
}

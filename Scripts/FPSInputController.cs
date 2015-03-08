using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSInputController : MonoBehaviour
{
	private CharacterMotor motor;
	private Game game;
	public Camera firstPersonCam;			// the target Camera for vertical view changes in firstperson
	public bool bUseRaw = true;				// Use raw mouseinputfunction or not
	public float mouseSensitivity = 1.0f, ySensMultiplier = 1.0f, yMinLimit = -85F, yMaxLimit = 85F;
	private float x = 0.0f;					// Storing camera angle
	private float y = 0.0f;					// Storing camera angle

	// Use this for initialization
	void Awake()
	{
		motor = GetComponent<CharacterMotor>();
		game = GameObject.Find ("_GAMECORE").GetComponent<Game> ();
		if (!firstPersonCam) {
			firstPersonCam = (Camera)this.transform.FindChild ("PlayerView").GetComponent<Camera>();
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!Network.isClient && !Network.isServer) { return; }
		if(!GetComponent<NetworkView>().isMine) { return; }
		if(Cursor.visible) { 
			motor.inputMoveDirection = Vector3.zero; // Zero out the inputvector if the cursor is not hidden
			return;
		}



		// Get the input vector from kayboard or analog stick
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		
		if (directionVector != Vector3.zero)
		{
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1.0f, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		
		// Apply the direction to the CharacterMotor
		motor.inputMoveDirection = transform.rotation * directionVector;
		motor.inputJump = Input.GetButton("Jump");


		// Get MouseInput
		float dx;
		float dy;
		if (bUseRaw) {
			dx = Input.GetAxisRaw ("Mouse X");
			dy = Input.GetAxisRaw ("Mouse Y");
		} else {
			dx = Input.GetAxis ("Mouse X");
			dy = Input.GetAxis ("Mouse Y");
		}
		if(game.invertMouse) {
			dy = dy * -1;
		}
		x += dx * mouseSensitivity;												// Mouse horizontal input
		y -= dy * (mouseSensitivity * ySensMultiplier);							// Mouse vertical input multiplier needs to be halfed for symmetry
		
		// Apply changes to camera
		y = Mathf.Clamp (y, yMinLimit, yMaxLimit);							// Clamping the max/min vertical angle
		firstPersonCam.transform.localEulerAngles = new Vector3 (y, 0, 0);	// Set the Cameras Y pitch
		motor.transform.rotation = Quaternion.Euler (0, x, 0);				// Set players avatar rotation
	}
}
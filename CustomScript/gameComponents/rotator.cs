// ID Tag 3
using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {
	
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout gameHolder;	// Holds all buttons (for easy access and movement)
	
	// Turn Signal
	UIButton turn;
	
	float rotX = 0;
	float rotY = 0;
	float rotZ = 0;
	float prevX;
	float prevY;
	float prevZ;
	
	int stateTurn = 0;
	
	// Use this for initialization
	void Start () {
		turn = UIButton.create(buttons,"turn.png","turn.png",300,300);
		
		// Make sure all components are hidden at start
		turn.hidden = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Disable Rotator
	public void disable() {
		turn.hidden = true;
	}
	
	// Enable Rotator
	public void enable() {
		//Random Position
		float randX = Random.value/3;
		float randY = Random.value/3;
		turn.positionFromTopLeft(randX+0.25f,randY+0.25f);
		
		// Visibility toggle
		turn.hidden = false;
		
		// Toggle stateTurn and other vars
		// Prev States
		//prevX = Input.acceleration.x;
		//prevY = Input.acceleration.y;
		//prevZ = Input.acceleration.z;
		// Rot states
		rotX = 0;
		rotY = 0;
		rotZ = 0;
	}
	
	public int forceTurn() {		// Swipe stuff
		
		//if(stateTurn > 0) {
			rotX += (Input.acceleration.x - prevX > 0.05)?Mathf.Abs(Input.acceleration.x - prevX):Mathf.Abs(Input.acceleration.x - prevX)/5;
			rotY += (Input.acceleration.y - prevY > 0.05)?Mathf.Abs(Input.acceleration.y - prevY):Mathf.Abs(Input.acceleration.y - prevY)/5;
			rotZ += (Input.acceleration.z - prevZ > 0.05)?Mathf.Abs(Input.acceleration.z - prevZ):Mathf.Abs(Input.acceleration.z - prevZ)/5;	
			/*// crossing -1 and 1
			if(Input.acceleration.x > 0.5f && prevX < -0.5f || Input.acceleration.x < -0.5f && prevX > 0.5f) {
				rotX += (Input.acceleration.x > 0.5f)?(-2 - Input.acceleration.x + prevX):(2 + Input.acceleration.x - prevX);
			}
			if(Input.acceleration.y > 0.5f && prevY < -0.5f || Input.acceleration.y < -0.5f && prevY > 0.5f) {
				rotY += (Input.acceleration.y > 0.5f)?(-2 - Input.acceleration.y + prevY):(2 + Input.acceleration.y - prevY);
			}
			if(Input.acceleration.z > 0.5f && prevZ < -0.5f || Input.acceleration.z < -0.5f && prevZ > 0.5f) {
				rotZ += (Input.acceleration.z > 0.5f)?(-2 - Input.acceleration.z + prevX):(2 + Input.acceleration.z - prevZ);
			}*/
		//} //else
			//swipeProgress.value = 0;
		//Debug.Log("rotZ: " + rotZ);
		//Debug.Log("z   : " + Input.acceleration.z);
		
		// Set current condition as false
		prevX = Input.acceleration.x;
		prevY = Input.acceleration.y;
		prevZ = Input.acceleration.z;
		
		// End Case
		if(rotX > 2 || rotY > 2 || rotZ > 2) {
			disable();
			return 0;
		}
		return 3;
	}
}

// ID Tag 2
using UnityEngine;
using System.Collections;

public class buttoner : MonoBehaviour {
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout gameHolder;	// Holds all buttons (for easy access and movement)
	
	UIButton mainButton;
	bool buttonPressed = false;
	
	// Use this for initialization
	void Start () {
		// Create and Hide button
		mainButton = UIButton.create(buttons,"tapUp.png","tapDown.png",0,0);
		mainButton.hidden = true;

		mainButton.onTouchUp += sender => { buttonPressed = true; };		// Toggle when button fully pressed
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Disable Button
	public void disable() {
		mainButton.hidden = true;
	}
	
	// Enable Button
	public void enable() {
		// Button has not been pressed
		buttonPressed = false;
		
		//Random Position
		float randX = Random.value/3;
		float randY = Random.value/3;
		mainButton.positionFromTopLeft(randX+0.25f,randY+0.25f);
		
		// Visibility toggle
		mainButton.hidden = false;
	}
	
	// Button Pressed
	public int forcePress() {
		if(buttonPressed) {
			buttonPressed = false;
			disable();
			return 0;
		}
		return 2;
	}
}

// ID Tag 1
using UnityEngine;
using System.Collections;

public class swiper : MonoBehaviour {
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout gameHolder;	// Holds all buttons (for easy access and movement)
	
	UIButton swipeButton;
	UIProgressBar swipeProgress;
	
	int stateSwipe;
	
	// Use this for initialization
	void Start () {
		// Swipe Detector
		swipeButton = UIButton.create(buttons,"swipeBase.png","swipeBase.png",300,300);
		swipeProgress = UIProgressBar.create("swipe1.png",300,300);
		swipeProgress.resizeTextureOnChange = false;				// Reveal the bar, don't resize texture
		//swipeProgress.clipped = true;
		swipeButton.onTouchDown += sender => { if(Input.mousePosition.x < swipeButton.localPosition.x + 100) {stateSwipe = 1;} };		// Detecting swipes
		swipeButton.onTouchUp += sender => { stateSwipe = 0; };
		
		// Deactivation
		swipeProgress.hidden = true;
		swipeButton.hidden = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public int forceSwipe() {		// Swipe stuff
		if(stateSwipe == 1) {
			swipeProgress.value = (Input.mousePosition.x - swipeButton.localPosition.x)/400;
		} else
			swipeProgress.value = 0;
		
		if(swipeProgress.value > 0.5f) {
			disable();
			return 0;
		}
		else return 1;
	}
	
	// Disable Swiper
	public void disable() {
		swipeButton.hidden = true;
		swipeProgress.hidden = true;
	}
	
	// Enable Swiper
	public void enable() {
		//Random Position
		float randX = Random.value/2;
		float randY = Random.value/2;
		swipeButton.positionFromTopLeft(randX+0.23f,randY+0.25f);
		swipeProgress.positionFromTopLeft(randX+0.155f,randY+0.25f);
		
		// Visibility toggle
		swipeButton.hidden = false;
		swipeProgress.hidden = false;
	}
	
	
}

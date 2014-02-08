using UnityEngine;
using System.Collections;

public class highScores : MonoBehaviour {
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout scoreHolder;	// Holds all buttons (for easy access and movement)
	
	bool back = false;		// True -> Goto Main					False -> Do nothing
	public bool activate = false;	// True -> Goto High Scores				False -> Do nothing		Unused(May delete)
	
	// Movement Variables
	int slideMenu = 0;			// 0 - No Slide									Yes I know about the positionTo
	float targetX = 0.00f;		// 1 - Slide Left	-1 - Slide Right			Command -> I just like to keep reference
	float targetY = 0.80f;		// 2 - Slide Up		-2 - Slide Down
	float velX = 0.02f;			// Velocity of slide X
	float velY = 0.02f;			// Velocity of slide Y
	float currentX = 0.00f;		// Current Location X
	float currentY = 0.80f;		// Current Location Y
	
	string[] highScoresName = new string[20];
	
	void Start () {
		// Declaration & Begin
		scoreHolder = new UIVerticalLayout(10);	// 10 px spaces
		scoreHolder.beginUpdates();							// Begin the updates (touchable)
		
		// Position & Size
		scoreHolder.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;	// Center all
		scoreHolder.positionFromLeft(0.80f,0.80f);			// Position Left (Moved up & Right to center buttons)
		
		// Declare Buttons
		var backButton = UIButton.create(buttons,"backUp.png","backDown.png",0,0);
		
		// Button Actions
		backButton.onTouchUp += sender => { goBack(); };				// If button hit -> go to play
		
		// Add Buttons to highScores
		scoreHolder.addChild(backButton);
		
		// Must have twice (Bug or something)
		scoreHolder.endUpdates();
		scoreHolder.endUpdates();
		
	}
	
	// Update is called once per frame
	void Update () {
		slideScores();		// Slide Main Menu when needed
	}
	
	// Activate -> Move Move Menu & High Scores Left -> player now in HighScores		- Used in manager -
	public void Activate() {
		back = false;			// Default
		slideMenu = 2;
		activate = true;
	}
	
	// Back -> Move Move Menu & High Scores Right -> player now in Menu
	void goBack() {
		back = true;			// See gotoMain
		slideMenu = -2;
		activate = false;
	}
	
	// Played every frame so play script knows when to take over
	public bool gotoMain() {
		if(back) {
			back = false;	// Set to false to prevent errors
			return true;		
		} else
			return false;
	}
	
	void slideScores() {
		// Slide to with reference to Position Left (Center left)
		
		// X -(Left) +(Right) Y -(Up) +(Down)
		// X 0.20, Y -0.80f <- Natural 
		if(slideMenu == 2) {			// Slide Left		- Natural -
			targetX = 0.00f;
			targetY = -0.50f;
		} else if(slideMenu == -2) {	// Slide Right		- Hidden -
			targetX = 0.00f;
			targetY = 0.80f;
		}
		
		slideAction();
	}
	
	// Controls sliding of UI
	void slideAction() {
		if(slideMenu != 0) {
			// Move X and Y to target
			if(currentX != targetX)
				currentX = (currentX>targetX) ? currentX-velX : currentX+velX;	// Move CurrentX velX closer to TargetX
			
			if(currentY != targetY)
				currentY = (currentY>targetY) ? currentY-velY : currentY+velY;	// Move CurrentX velX closer to TargetX
			
			// High Scores slideMenu < 0 means larger values
			if(slideMenu > 0 && 
				(targetY > currentY || targetX > currentX)) {	// If target reached and exceeded
				currentX = targetX;								// Set position to target
				currentY = targetY;
				slideMenu = 0;									// End slide actions
			} else if(slideMenu < 0 && 
				(targetY < currentY || targetX < currentX)) {	// If target reached and exceeded
				currentX = targetX;								// Set Position to target
				currentY = targetY;
				slideMenu = 0;									// End slide Actions
			}
			
			// Actual Movement code
			scoreHolder.positionFromLeft(currentY,currentX);					// Set buttons to new position
			
		}
	}
}

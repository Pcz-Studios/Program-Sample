using UnityEngine;
using System.Collections;

public class main : MonoBehaviour {
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout mainHolder;	// Holds all buttons (for easy access and movement)
	
	bool play = false;		// True -> Play Game					False -> Do nothing
	bool highScore = false;	// True -> Go to High Scores board		False -> Do nothing
	public bool activate = true;	// True -> Go to Main			False -> Do nothing		// For other objects to access
	
	// Movement Variables
	int slideMenu = 0;			// 0 - No Slide									Yes I know about the positionTo
	float targetX = 0.20f;		// 1 - Slide Left	-1 - Slide Right			Command -> I just like to keep reference
	float targetY = -0.20f;		// 2 - Slide Up		-2 - Slide Down
	float velX = 0.02f;			// Velocity of slide X
	float velY = 0.02f;			// Velocity of slide Y
	float currentX = 0.20f;		// Current Location X
	float currentY = -0.20f;	// Current Location Y
	
	void Start () {
		
		// Declaration & Begin
		mainHolder = new UIVerticalLayout(10);	// 10 px spaces
		mainHolder.beginUpdates();				// Begin the updates (touchable)
		
		// Position & Size
		mainHolder.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;	// Center all
		mainHolder.positionFromLeft(-0.20f,0.20f);			// Position Left (Moved up & Right to center buttons)
		
		// Declare Buttons
		var playButton = UIButton.create(buttons,"playUp.png","playDown.png",0,0);
		var highScoreButton = UIButton.create(buttons,"hsUp.png","hsDown.png",0,0);
		
		var canyouSeeThisButton = UIButton.create(buttons,"playUp.png","playDown.png",640,400);	// For testing delete this
		
		// Button Actions
		playButton.onTouchUp += sender => { playGame(); };				// If button hit -> go to play
		highScoreButton.onTouchUp += sender => { seeHighScores(); };	// If button hit -> go to high scores
		
		// Add Buttons to main Menu
		mainHolder.addChild(playButton);
		mainHolder.addChild(highScoreButton);
		
		// Must have twice (Bug or something)
		mainHolder.endUpdates();
		mainHolder.endUpdates();
		
	}
	
	// Update is called once per frame
	void Update () {
		slideMainMenu();		// Slide Main Menu when needed
	}
	
	// Play -> Move Move Menu & Game Left -> player now playing game
	void playGame() {
		play = true;			// See gotoPlay
		slideMenu = 1;
		activate = false;		// Deactivate
	}
	// HighScores -> Move Move Menu & HighScores Up -> player now checking scores
	void seeHighScores() {
		highScore = true;		// See gotoHighScore
		slideMenu = 2;
		activate = false;
	}
	
	// Played every frame so play script knows when to take over
	public bool gotoPlay() {
		if(play) {
			play = false;	// Set to false to prevent errors
			return true;		
		} else
			return false;
	}
	
	public void Activate() {	//						- Used in manager -
		slideMenu = -1;			// Return to natural
		activate = true;		// Reactivate
	}
	
	// Run every frame so high Scores script knows when to take over
	public bool gotoHighScore() {
		if(highScore) {
			highScore = false;	// Set to false to prevent errors
			return true;		
		} else
			return false;
	}
	
	void slideMainMenu() {
		// Slide to with reference to Position Left (Center left)
		
		// X -(Left) +(Right) Y -(Up) +(Down)
		// X 0.20, Y -0.20f <- Natural 
		if(slideMenu == 1) {			// Slide Left	- Game -
			targetX = -0.80f;
			targetY = -0.20f;
		} else if(slideMenu == -1) {	// Slide Right	(To Natural)
			targetX = 0.20f;
			targetY = -0.20f;
		} else if(slideMenu == 2) {		// Slide Up		- High Scores -
			targetX = 0.20f;
			targetY = -1.20f;
		} else if(slideMenu == -2) {	// Slide Down	(To Natural)
			targetX = 0.20f;
			targetY = -0.20f;
		}
		
		slideAction();
		
	}
	
	void slideAction() {
		if(slideMenu != 0) {
			// Move X and Y to target
			if(currentX != targetX)
				currentX = (currentX>targetX) ? currentX-velX : currentX+velX;	// Move CurrentX velX closer to TargetX
			
			if(currentY != targetY)
				currentY = (currentY>targetY) ? currentY-velY : currentY+velY;	// Move CurrentX velX closer to TargetX
			
			// Main menu slideMenu > 0 means smaller target values
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
			mainHolder.positionFromLeft(currentY,currentX);					// Set buttons to new position
			
		}
	}
}

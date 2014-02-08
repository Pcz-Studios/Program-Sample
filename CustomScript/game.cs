using UnityEngine;
using System.Collections;

public class game : MonoBehaviour {
	
	public UIToolkit buttons;			// Buttons Texture
	public UIVerticalLayout gameHolder;	// Holds all buttons (for easy access and movement)
	
	bool back = false;				// True -> Goto Main					False -> Do nothing
	public bool activate = false;	// True -> Goto High games				False -> Do nothing		Unused(May delete)
	
	// Movement Variables
	int slideMenu = 0;			// 0 - No Slide									Yes I know about the positionTo
	float targetX = 1.20f;		// 1 - Slide Left	-1 - Slide Right			Command -> I just like to keep reference
	float targetY = -0.50f;		// 2 - Slide Up		-2 - Slide Down
	float velX = 0.02f;			// Velocity of slide X
	float velY = 0.02f;			// Velocity of slide Y
	float currentX = 1.20f;		// Current Location X
	float currentY = -0.50f;	// Current Location Y
	
	UITouchableSprite textSprite;							// Sprite for score
	
	UITextInstance scoreText;
	
	// Score stuff
	float timeLast = 0;			// Last time check
	
	long score = 0;				// Score
	int multiplier = 1;			// Multiplier
	int comboValue = 1;
	
	UIButton retryButton;
	
	// Mechanics stuff	- Swipes
	public swiper swipeManager = new swiper();
	public buttoner buttonManager = new buttoner();
	public rotator rotationManager = new rotator();
	
	// Timing
	float actionTime = 0;			// Last time an action was completed
	float deltaAction = 2;			// Time diff between actions
	
	int state = -1;				// -1 - Deactivated	|| 0 - Pressed || 1 - Swipe || 2 - Button
	public bool lose = false;
	
	void Start () {
		
		// Declaration & Begin
		gameHolder = new UIVerticalLayout(10);	// 10 px spaces
		gameHolder.beginUpdates();							// Begin the updates (touchable)
		
		// Position & Size
		gameHolder.alignMode = UIAbstractContainer.UIContainerAlignMode.Center;	// Center all
		gameHolder.positionFromLeft(0.80f,0.80f);			// Position Left (Moved up & Right to center buttons)
		
		// Declare Buttons
		var backButton = UIButton.create(buttons,"backUp.png","backDown.png",50,0);
		retryButton = UIButton.create(buttons,"backDown.png","backUp.png",500,0);
		
		// Button Actions
		backButton.onTouchUp += sender => { goBack(); };				// If button hit -> go to play
		retryButton.onTouchUp += sender => { Activate(); };
		
		// Disable retry initially
		retryButton.hidden = true;
		
		// Game Mechanics Start
		
		// Scores & Combos
		var text = new UIText("font","font.png");		// Font & size
		scoreText = text.addTextInstance("",50,50);				// Scores
		
		// To add text to gameHolder
		scoreText.parentUIObject = textSprite;
		
		
		// Game Mechanics End
		
		
		
		// Add Buttons & Text to game		- Do not add game objects (game holder has issues -
		gameHolder.addChild(backButton);
		
		// Must have twice (Bug or something)
		gameHolder.endUpdates();
		gameHolder.endUpdates();
		
		
	}
	
	// Update is called once per frame
	void Update () {
		slideGame();		// Slide Main Menu when needed
		
		if(activate){
			gameAction();		// game On
			actionPicker();		// Next action here
			loseCase();			// Check for loss
		}
		
	}
	
	// Activate -> Move Move Menu & High games Left -> player now in Highgames		- Used in manager -
	public void Activate() {
		back = false;			// Default
		slideMenu = 1;			// Slide position active
		state = 0;				// State = 0
		lose = false;			// Not yet lost
		timeLast = Time.time;	// Score
		actionTime = Time.time;	// last action
		activate = true;		// Yup
		retryButton.hidden = true;	// Disable extraneous Buttons
	}
	
	// Back -> Move Move Menu & High games Right -> player now in Menu
	void goBack() {
		back = true;			// See gotoMain
		slideMenu = -1;
		activate = false;
		
		// Deactivate buttons
		swipeManager.disable();
		buttonManager.disable();
		rotationManager.disable();
		retryButton.hidden = true;	// Disable extraneous Buttons
		
		score = 0;				// Reset score values
		scoreText.text = "";
	}
	
	// Visible to other scripts - played every frame so play script knows when to take over
	public bool gotoMain() {
		if(back) {
			back = false;	// Set to false to prevent errors
			return true;		
		} else
			return false;
	}
	
	void slideGame() {
		// Slide to with reference to Position Left (Center left)
		
		// X -(Left) +(Right) Y -(Up) +(Down)
		// X 0.20, Y -0.80f <- Natural 
		if(slideMenu == 1) {			// Slide Left	- Natural -
			targetX = 0.0f;
			targetY = -0.50f;
		} else if(slideMenu == -1) {	// Slide Right	- Hidden -
			targetX = 1.20f;
			targetY = -0.50f;
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
			
			// High games slideMenu < 0 means larger values
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
			gameHolder.positionFromLeft(currentY,currentX);					// Set buttons to new position
			
		}
	}
	
	void textAction() {
		scoreText.text = score.ToString();
		//score = System.Int64.MaxValue;
		//score += (int)Mathf.Floor(Time.deltaTime*1000) * multiplier;
		//multiplier = (int)Mathf.Floor(Time.time);
	}
	
	void scoreAction() {
		if(!lose) {			// If not lost, keep counting
			score += (int)(multiplier * (Time.time - timeLast));	// Multiplier * delta Time
			score += (int)(multiplier * comboValue);				// score + combo
		}
		timeLast = Time.time;
	}
	
	void actionPicker() {
		if(actionTime+deltaAction < Time.time) {	// If time for new action
			if(state != 0)
				lose = true;
			if(!lose) {
				state = (int)Mathf.Ceil(Random.value*3);		// Pick Random action
				if(state == 1)
					swipeManager.enable();					// Swiper
				else if(state == 2)
					buttonManager.enable();					// Button Press
				else if(state == 3)
					rotationManager.enable();
				
				actionTime = Time.time;
				if(deltaAction > 0.2f)	// Speed up delta Action
					deltaAction-=0.02f;
			}
		}
	}
	
	void gameAction() {
		textAction();		// Text stuff
		scoreAction();		// Inc score
		if(state == 1)
			state = swipeManager.forceSwipe();
		else if(state == 2)
			state = buttonManager.forcePress();
		else if(state == 3)
			state = rotationManager.forceTurn();
	}
	
	void loseCase() {
		if(lose) {
			// Disable all
			swipeManager.disable();
			buttonManager.disable();
			rotationManager.disable();
			//activate = false;
			
			// Create back and retry buttons;
			retryButton.hidden = false;
		}
	}
}

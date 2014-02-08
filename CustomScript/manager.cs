using UnityEngine;
using System.Collections;

// Master Script
public class manager : MonoBehaviour {
	
	public main mainMenu;
	public game mainGame;
	public highScores highScoresBoard;
	
	// Backgrounds
	public Texture2D bg_main;
	public Texture2D bg_game;
	public Texture2D bg_hs;
	public Texture2D bg_gameOver;
	
	public GUITexture background;
	
	// Use this for initialization
	void Start () {
		background.pixelInset = new Rect(-bg_main.width/2,-bg_main.height/2,bg_main.width,bg_main.height);
		background.texture = bg_main;
	}
	
	// Update is called once per frame
	void Update () {
		if(mainMenu.gotoHighScore()) {
			highScoresBoard.Activate();		// Set Main
			background.texture = bg_hs;		// Set BG
		} else if(mainMenu.gotoPlay()) {
			mainGame.Activate();
			background.texture = bg_game;
		} else if(highScoresBoard.gotoMain()) {
			mainMenu.Activate();
			background.texture = bg_main;
		} else if(mainGame.gotoMain()) {
			mainMenu.Activate();
			background.texture = bg_main;
		}
		
		if(mainGame.activate) {
			if(mainGame.lose)
				background.texture = bg_gameOver;
			else
				background.texture = bg_game;
		}
			
	}
	/*
	// Draw the background
	void OnGUI() {			
		if(mainMenu.activate) {
			Graphics.DrawTexture(new Rect(-20,-20,bg_main.width,bg_main.height),bg_main);
		} else if(highScoresBoard.activate) {
			Graphics.DrawTexture(new Rect(-20,-20,bg_game.width,bg_game.height),bg_game);
		} else if(mainGame.activate) {
			Graphics.DrawTexture(new Rect(-20,-20,bg_hs.width,bg_hs.height),bg_hs);
		} else {			// Default (Main Menu)
			Graphics.DrawTexture(new Rect(-20,-20,bg_main.width,bg_main.height),bg_main);
		}
		
	}*/
}

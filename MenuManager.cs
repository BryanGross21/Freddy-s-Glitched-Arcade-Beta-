using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace Freddys
{
	/// <summary>
	/// Enum representing which option on the menu is currently selected
	/// </summary>
	public enum Option
	{
		NewGame = 1,
		LoadGame = 2,
		Options = 3,
		Exit = 4
	}

	public class MenuManager
	{
		/// <summary>
		/// String array that contains all text to be drawn on the menu
		/// </summary>
		public string[] menuText = { "Freddy's Glitched Arcade",
			"New Game",
			"Load Game",
			"Options",
			"Exit",
			"Controls: ",
			"Selection: ",
			"Confirmation: ",
			"Freddy's Pizza\n    & Arcade",
			"Game Project 1 Instructions:\nThis build includes a small \ngame portion, to play hit 'E' on \nthe new game option. \nMore details on Git Release.",
			"This game is a fan game of \nFive Nights at Freddy's;\nFive Nights at Freddy's \nby Scott Cawthon" };
		/// <summary>
		/// Gets the current keyboard state
		/// </summary>
		private KeyboardState currentKeyboardState;
		/// <summary>
		/// Gets the previous keyboard state
		/// </summary>
		private KeyboardState pastKeyboardState;

		/// <summary>
		/// The current option selected on the menu
		/// </summary>
		private Option menuSelection = Option.NewGame;

		/// <summary>
		/// The background image for the main menu
		/// </summary>
		private Texture2D Background;

		/// <summary>
		/// A grey transparent background for option selections
		/// </summary>
		private Texture2D selectionBackground;

		/// <summary>
		/// A grey transparent background for the title
		/// </summary>
		private Texture2D TitleBackground;

		/// <summary>
		/// Shows the movementControls for selecting menu options
		/// </summary>
		private Texture2D movementControls;

		/// <summary>
		/// Shows a sprite for the key to press to confirm a menu option
		/// </summary>
		private Texture2D confirm;

		/// <summary>
		/// Shows a sprite for the door on the main menu
		/// </summary>
		private Texture2D door;

		/// <summary>
		/// Shows a sprite for the animated sign on the menu
		/// </summary>
		private Texture2D sign;

		/// <summary>
		/// Freddy Fazbear's character sprites for the menu including him waving at you (what a nice fellow)
		/// </summary>
		private Texture2D freddyFaz;


		/// <summary>
		/// Sprite font for all text on the menu
		/// </summary>
		private SpriteFont menuTextFont;

		/// <summary>
		/// Sprite font for the title of the game on the menu
		/// </summary>
		private SpriteFont titleFont;

		/// <summary>
		/// Sprite font for the instructions and credits
		/// </summary>
		private SpriteFont creditFont;

		/// <summary>
		/// The sign's animation timer
		/// </summary>
		private double signAnimationTimer;

		/// <summary>
		/// sign's current animation frame
		/// </summary>
		private short signAnimationFrame = 0;

		/// <summary>
		/// Freddy's animation timer
		/// </summary>
		private double fredAnimationTimer;

		/// <summary>
		/// Freddy's current animation frame
		/// </summary>
		private short fredAnimationColumn = 0;
		
		/// <summary>
		/// The current row of Freddy's animation
		/// </summary>
		private short fredAnimationFrameRow = 0;

		/// <summary>
		/// The amount of time each frame should hold for Freddy's animation
		/// </summary>
		private double fredAnimationTime = 1;

		/// <summary>
		/// Process an input to exit the game
		/// </summary>
		public bool Exit { get; private set; }

		/// <summary>
		/// Sees to draw and process menu actions
		/// </summary>
		public bool inMenu { get; private set; }

		/// <summary>
		/// Process the input to start a new game
		/// </summary>
		public bool startNewGame { get; private set; }

		public MenuManager() 
		{
			inMenu = true;
			startNewGame = false;
		}

		/// <summary>
		/// Loads the texture content for the menu
		/// </summary>
		/// <param name="content">The manager to load the content with</param>
		public void LoadContent(ContentManager content)
		{
			Background = content.Load<Texture2D>("Menu_Assets/Building_Background");
			selectionBackground = content.Load<Texture2D>("Menu_Assets/option_background");
			TitleBackground = content.Load<Texture2D>("Menu_Assets/Title_Box");
			movementControls = content.Load<Texture2D>("Menu_Assets/Controls_Arrows");
			confirm = content.Load<Texture2D>("Menu_Assets/Confirm_Key");
			door = content.Load<Texture2D>("Menu_Assets/Door_Menu");
			sign = content.Load<Texture2D>("Menu_Assets/menuSign");
			freddyFaz = content.Load<Texture2D>("Menu_Assets/Freddy_Menu_Sprites");
			menuTextFont = content.Load<SpriteFont>("menuItem");
			titleFont = content.Load<SpriteFont>("titleFont");
			creditFont = content.Load<SpriteFont>("Instructionsandcredit");
		}

		public void update(GameTime gameTime)
		{
			#region State Updating
			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();
			#endregion
			if (inMenu)
			{
				#region Option Selection Input
				//Get position from Keyboard
				if (currentKeyboardState.IsKeyDown(Keys.Up) && pastKeyboardState.IsKeyUp(Keys.Up))
				{
					if ((int)menuSelection - 1 == 0)
					{
						menuSelection = Option.Exit;
					}
					else
					{
						menuSelection -= 1;
					}
				}
				if (currentKeyboardState.IsKeyDown(Keys.Down) && pastKeyboardState.IsKeyUp(Keys.Down))
				{
					if ((int)menuSelection + 1 == 5)
					{
						menuSelection = Option.NewGame;
					}
					else
					{
						menuSelection += 1;
					}
				}
				#endregion

				#region Selection Action Input
				if (currentKeyboardState.IsKeyDown(Keys.E) && menuSelection == Option.NewGame)
				{
					startNewGame = true;
					inMenu = false;
				}
				if (currentKeyboardState.IsKeyDown(Keys.E) && menuSelection == Option.Exit)
				{
					Exit = true;
				}
				#endregion
			}
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (inMenu )
			{
				//The font color to draw
				Color colorToDraw = Color.White;
				//Handles the animation and drawing for the animated sign on the title
				spriteBatch.Draw(Background, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
				signAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (signAnimationTimer > .22)
				{
					signAnimationFrame++;
					if (signAnimationFrame > 2) signAnimationFrame = 0;
					signAnimationTimer -= .22;
				}
				var signSource = new Rectangle(signAnimationFrame * 256, 0, 256, 256);
				spriteBatch.Draw(sign, new Vector2(150, -125), signSource, Color.White, 0, new Vector2(0, 0), 1.3f, SpriteEffects.None, 0);
				spriteBatch.DrawString(titleFont, menuText[8], new Vector2(220, 72), Color.Goldenrod, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);

				fredAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				//Animates Freddy Fazbear on the menu, he emerges from the shadows, waves for a bit, and then reemerges into the shadows 
				if (fredAnimationTimer > fredAnimationTime)
				{
					if (fredAnimationFrameRow == 0)
					{
						fredAnimationColumn++;
						if (fredAnimationColumn > 4)
						{
							fredAnimationFrameRow = 1;
							fredAnimationColumn = 0;
							fredAnimationTime = .75;
						}
					}
					else if (fredAnimationFrameRow == 1)
					{
						fredAnimationColumn++;
						if (fredAnimationColumn > 4)
						{
							fredAnimationColumn = 3;
						}
					}
					fredAnimationTimer -= fredAnimationTime;
				}

				var fredSource = new Rectangle(fredAnimationColumn * 144, fredAnimationFrameRow * 144, 144, 144);
				spriteBatch.Draw(freddyFaz, new Vector2(212, 222), fredSource, Color.White, 0, new Vector2(0, 0), 1.45f, SpriteEffects.None, 0);

				//Draws the door above Freddy
				spriteBatch.Draw(door, new Vector2(226, 202), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
				spriteBatch.DrawString(titleFont, menuText[0], new Vector2(8, 16), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
				//This for loop sets-up the transparent grey background for menu options and the text
				int j = 1;
				for (int i = 1; i < 5; i++)
				{
					Vector2 Position = new Vector2(0, (64 * i));
					if(i < 5)
					{
						if (i > 1)
						{

							Position.Y -= 32 * j;
							j++;
						}
						spriteBatch.Draw(selectionBackground, Position, null, Color.White, 0, new Vector2(0, 0), new Vector2(.5f, .25f), SpriteEffects.None, 0);
					}
					if (i > 0 && i <= 4)
					{
						if (i == (int)menuSelection)
						{
							colorToDraw = Color.Red;
						}
						spriteBatch.DrawString(menuTextFont, menuText[i], new Vector2(12, (64 * i) - 32 * (j - 1)), colorToDraw, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
						colorToDraw = Color.White;
					}
				}
				//Gets the second control from the controls_arrows sprite sheet
				spriteBatch.Draw(TitleBackground, new Vector2(0, 192), null, Color.White, 0, new Vector2(0, 0), .85f, SpriteEffects.None, 0);
				spriteBatch.DrawString(menuTextFont, menuText[5], new Vector2(2, 192), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
				spriteBatch.DrawString(menuTextFont, menuText[6], new Vector2(4, 224), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
				spriteBatch.DrawString(menuTextFont, menuText[7], new Vector2(2, 256), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
				spriteBatch.Draw(movementControls, new Vector2(128, 192), new Rectangle(0, 96, 96, 96), Color.White, 0, new Vector2(0, 0), .65f, SpriteEffects.None, 1);
				spriteBatch.Draw(confirm, new Vector2(172, 256), null, Color.White, 0, new Vector2(0, 0), .35f, SpriteEffects.None, 1);
				//Instructions for how to exit the game and credit to Five Nights at Freddy's original creator
				spriteBatch.Draw(TitleBackground, new Vector2(404, 256), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
				spriteBatch.DrawString(creditFont, menuText[9], new Vector2(406, 256), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
				spriteBatch.DrawString(creditFont, menuText[10], new Vector2(406, 400), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1);
			}
		}

	}
}

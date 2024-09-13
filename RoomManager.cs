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
using Freddys.Collisions;


namespace Freddys
{
	public enum Direction
	{
		Down = 0,
		Up = 1
	}

	public class RoomManager
	{
		/// <summary>
		/// Gets the current keyboard state
		/// </summary>
		private KeyboardState currentKeyboardState;
		/// <summary>
		/// Gets the previous keyboard state
		/// </summary>
		private KeyboardState pastKeyboardState;

		private Vector2 position = new Vector2(0, 300);

		private Vector2 propPosition = new Vector2();

		/// The following lines of code are initializing objects for the sprites used for this room
		private Texture2D stage;
		private Texture2D curtains;
		private Texture2D freddy;
		private Texture2D bonnie;
		private Texture2D chica;
		private Texture2D table;
		private Texture2D present;
		private Texture2D monitor;
		private Texture2D player;
		private Texture2D arFred;
		private Texture2D arBon;
		private Texture2D arChica;
		private Texture2D arGuitar;
		private Texture2D interact;
		private Texture2D controls;
		private Texture2D arFilter;

		/// <summary>
		/// Sprite font for all text on the menu
		/// </summary>
		private SpriteFont menuTextFont;

		/// <summary>
		/// The image to use for the player sprite
		/// </summary>
		private short playerFrame = 0;

		/// <summary>
		/// The stage's animation timer
		/// </summary>
		private double stageAnimationTimer;

		/// <summary>
		/// The stage's current animation frame
		/// </summary>
		private short stageAnimationColumn = 0;

		/// <summary>
		/// Chica's animation timer
		/// </summary>
		private double chicaAnimationTimer;

		/// <summary>
		/// AR props' animation timer
		/// </summary>
		private double propAnimationTimer;

		/// <summary>
		/// Chica's current animation frame
		/// </summary>
		private short chicaAnimationColumn = 0;

		/// <summary>
		/// The current row of Chica's animation
		/// </summary>
		private short chicaAnimationFrameRow = 0;

		/// <summary>
		/// The amount of time each frame should hold for chica's animation
		/// </summary>
		private double chicaAnimationTime = 1;

		/// <summary>
		/// Bonnie's animation timer
		/// </summary>
		private double bonAnimationTimer;

		/// <summary>
		/// Bonnie's current animation frame
		/// </summary>
		private short bonAnimationColumn = 0;

		/// <summary>
		/// The current row of Bonnie's animation
		/// </summary>
		private short bonAnimationFrameRow = 0;

		/// <summary>
		/// The amount of time each frame should hold for Bonnie's animation
		/// </summary>
		private double bonAnimationTime = 1;

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
		/// Checks to see if the player is in the alternate view mode
		/// </summary>
		private bool isAr;

		/// <summary>
		/// Checks to see if the player is in the alternate view mode
		/// </summary>
		private bool isShowtime = false;

		/// <summary>
		/// Checks to see if the player is in game
		/// </summary>
		private bool isInGame;

		/// <summary>
		/// Process an input to exit the game to the menu
		/// </summary>
		public bool Exit { get; private set; }

		/// <summary>
		/// The amount of times the showtime has been called
		/// </summary>
		private int showtimeCount = 0;

		/// <summary>
		/// The direction the ar mode moving sprites are moving
		/// </summary>
		private Direction direction = Direction.Down;

		private BoundingRectangle playerBounds = new BoundingRectangle(new Vector2(0 - 48, 300 - 48), 192, 192);

		private BoundingRectangle monitorTableBounds = new BoundingRectangle(new Vector2(275 - 78, 340 - 32), 128, 128);

		/// <summary>
		/// Sees to flip the sprite based off of player direction
		/// </summary>
		private bool flipped;

		/// <summary>
		/// Checks to see if the player pressed the up arrow or w for the player to look up
		/// </summary>
		private bool isLookingUp;

		/// <summary>
		/// Checks to see if the interact key should be visible or not
		/// </summary>
		private bool interactKeyVisible;

		/// <summary>
		/// Checks to see if the player can deactivate the showtime
		/// </summary>
		private bool canDeactivateShowtime = false;

		/// <summary>
		/// Loads the texture content for the menu
		/// </summary>
		/// <param name="content">The manager to load the content with</param>
		public void LoadContent(ContentManager content)
		{
			stage = content.Load<Texture2D>("Showroom_assets/Stage");
			curtains = content.Load<Texture2D>("Showroom_assets/curtains");
			freddy = content.Load<Texture2D>("Showroom_assets/Freddy_stage_finalized");
			chica = content.Load<Texture2D>("Showroom_assets/Chica_stage");
			bonnie = content.Load<Texture2D>("Showroom_assets/Bonnie_stage_finalized");
			arFred = content.Load<Texture2D>("Showroom_assets/freddy_head");
			arBon = content.Load<Texture2D>("Showroom_assets/ar_bonnie_head");
			arChica = content.Load<Texture2D>("Showroom_assets/ar_chica_head");
			arGuitar = content.Load<Texture2D>("Showroom_assets/ar_guitar");
			player = content.Load<Texture2D>("Showroom_assets/Temp_Player (2)");
			controls = content.Load<Texture2D>("Menu_Assets/Controls_Arrows");
			interact = content.Load<Texture2D>("Menu_Assets/Confirm_Key");
			menuTextFont = content.Load<SpriteFont>("menuItem");
			monitor = content.Load<Texture2D>("Showroom_assets/Showtime_monitor");
			table = content.Load<Texture2D>("Showroom_assets/Table");
			present = content.Load<Texture2D>("Showroom_assets/present");
			arFilter = content.Load<Texture2D>("Showroom_assets/Blacklight_mode_filter");
		}

		public void update(GameTime gameTime)
		{
			#region State Updating
			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();
			#endregion

			#region Control Input
			//Get position from Keyboard
			if (currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W))
			{
				isLookingUp = true;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A))
			{
				isLookingUp = false;
				position += new Vector2(-1, 0);
				flipped = true;
			}
			if (currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D))
			{
				isLookingUp = false;
				position += new Vector2(1, 0);
				flipped = false;
			}
			#endregion

			#region Animation for moving sprites
			//Update directionTimer
			propAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

			//Switch directions every 2 seconds
			if (propAnimationTimer > 2.0)
			{
				switch (direction)
				{
					case Direction.Up:
						direction = Direction.Down;
						break;
					case Direction.Down:
						direction = Direction.Up;
						break;
				}
				propAnimationTimer -= 2.0;
			}

			switch (direction)
			{
				case Direction.Up:
					propPosition += new Vector2(0, -.25f) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
					break;
				case Direction.Down:
					propPosition += new Vector2(0, .25f) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
					break;
			}
			#endregion

			#region Collision Checks
			if (playerBounds.collidesWith(monitorTableBounds))
			{
				if (isShowtime == false || canDeactivateShowtime)
				{
					interactKeyVisible = true;
				}
				if (currentKeyboardState.IsKeyDown(Keys.E) && pastKeyboardState.IsKeyUp(Keys.E))
				{
					isShowtime = !isShowtime;
					canDeactivateShowtime = !canDeactivateShowtime;
				}
			}
			else 
			{
				interactKeyVisible = false;
			}
			#endregion

			#region Selection Action Input
			if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space) && isAr == false)
			{
				isAr = true;
			}
			else if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space) && isAr == true)
			{
				isAr = false;
			}

			if (currentKeyboardState.IsKeyDown(Keys.Escape))
			{
				Exit = true;
			}
			#endregion

			playerBounds.X = position.X - 96;
			playerBounds.Y = position.Y - 96;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//The font color to draw

			Color colorToDraw = Color.White;
			spriteBatch.Draw(stage, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), new Vector2(2.5f, 2f), SpriteEffects.None, 0);

			SpriteEffects spriteEffect;
			if (flipped)
			{
				spriteEffect = SpriteEffects.FlipHorizontally;
			}
			else 
			{
				 spriteEffect = SpriteEffects.None;
			}

			if (isAr)
			{
				fredAnimationFrameRow = 1;
				bonAnimationFrameRow = 1;
				chicaAnimationFrameRow = 1;
			}
			else
			{
				fredAnimationFrameRow = 0;
				bonAnimationFrameRow = 0;
				chicaAnimationFrameRow = 0;
			}

			if (isShowtime)
			{
				bonAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (bonAnimationTimer > 1)
				{
					bonAnimationColumn++;
					if (bonAnimationColumn == 6) bonAnimationColumn = 3;
					bonAnimationTimer -= 1;
				}
			}
			else if (isShowtime == false && showtimeCount != 0)
			{
				bonAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (bonAnimationTimer > 1)
				{
					if (bonAnimationColumn >= 1)
					{
						bonAnimationColumn--;
					}
				}
				bonAnimationTimer -= 1;
			}
			var bonSource = new Rectangle(bonAnimationColumn * 144,bonAnimationFrameRow * 144, 144, 144);
			spriteBatch.Draw(bonnie, new Vector2(125, 175), bonSource, Color.White, 0, new Vector2(0, 0), 1.45f, SpriteEffects.None, 0);

			if (isShowtime)
			{
				chicaAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (chicaAnimationTimer > 1)
				{
					chicaAnimationColumn++;
					if (chicaAnimationColumn == 6) chicaAnimationColumn = 3;
					chicaAnimationTimer -= 1;
				}
			}
			else if (isShowtime == false && showtimeCount != 0)
			{
				chicaAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (chicaAnimationTimer > 1)
				{
					if (chicaAnimationColumn >= 1)
					{
						chicaAnimationColumn--;
					}
				}
				chicaAnimationTimer -= 1;
			}
			var chicaSource = new Rectangle(chicaAnimationColumn * 144, chicaAnimationFrameRow * 144, 144, 144);
			spriteBatch.Draw(chica, new Vector2(325, 175), chicaSource, Color.White, 0, new Vector2(0, 0), 1.45f, SpriteEffects.None, 0);

			if (isShowtime)
			{
				fredAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (fredAnimationTimer > 1)
				{
					fredAnimationColumn++;
					if (fredAnimationColumn == 6) fredAnimationColumn = 3;
					fredAnimationTimer -= 1;
				}
			}
			else if (isShowtime == false && showtimeCount != 0)
			{
				fredAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (fredAnimationTimer > 1)
				{
					if (fredAnimationColumn >= 1)
					{
						fredAnimationColumn--;
					}
					fredAnimationTimer -= 1;
				}
			}

			var fredSource = new Rectangle(fredAnimationColumn * 144, fredAnimationFrameRow * 144, 144, 144);
			spriteBatch.Draw(freddy, new Vector2(225, 175), fredSource, Color.White, 0, new Vector2(0, 0), 1.45f, SpriteEffects.None, 0);

			if (isShowtime && stageAnimationColumn != 7)
			{
				stageAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (stageAnimationTimer > .55)
				{
					stageAnimationColumn++;
					stageAnimationTimer -= .55;
				}
				if (stageAnimationColumn == 7)
				{
					showtimeCount++;
					canDeactivateShowtime = true;
				}
			}
			else if (isShowtime == false && showtimeCount != 0)
			{
				stageAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (stageAnimationTimer > .55)
				{
					if (stageAnimationColumn >= 1)
					{
						stageAnimationColumn--;
					}
					stageAnimationTimer -= .55;
				}
			}
			var stageSource = new Rectangle(stageAnimationColumn * 256, 0, 256, 256);
			spriteBatch.Draw(curtains, new Vector2(0, 0), stageSource, Color.White, 0, new Vector2(0, 0), new Vector2(2.5f, 2f), SpriteEffects.None, 1);



			spriteBatch.Draw(table, new Vector2(225, 380), Color.White);
			string monitorText = "Show:\nOFF";
			int rowOfMonitor = 0;
			if (isShowtime) 
			{
				rowOfMonitor = 1;
				monitorText = "Show:\nON";
			}
			var monitorSource = new Rectangle(0, rowOfMonitor * 128, 128, 128);
			spriteBatch.Draw(monitor, new Vector2(275, 340), monitorSource, Color.White);
			spriteBatch.DrawString(menuTextFont, monitorText, new Vector2(300, 375), Color.White);


			if (interactKeyVisible) 
			{
				spriteBatch.Draw(interact, position + new Vector2(55, -50), null, Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
			}

			if (isLookingUp)
			{
				playerFrame = 1;
			}
			else
			{
				playerFrame = 0;
			}
			var playerSource = new Rectangle(playerFrame * 192, 0, 192, 192);
			spriteBatch.Draw(player, position, playerSource, Color.White, 0, Vector2.Zero, 1f, spriteEffect, 0);

			if (isAr) 
			{
				spriteBatch.Draw(arFred, new Vector2(0, 0) + propPosition, null, Color.White, -.25f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
				spriteBatch.Draw(arGuitar, new Vector2(400, -75) + propPosition, null, Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.FlipHorizontally, 1);
				spriteBatch.Draw(arFilter, new Vector2(0, 0), Color.White);
			}


		}
	}
}


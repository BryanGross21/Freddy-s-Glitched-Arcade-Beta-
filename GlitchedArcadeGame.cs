using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.X3DAudio;

namespace Freddys
{
	/// <summary>
	/// Enum representing which state to draw
	/// </summary>
	public enum state
	{
		menu = 1,
		game = 2,
	}

	public class GlitchedArcadeGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private MenuManager menuManager;
		private RoomManager roomManager;
		private state currentGameState;
		private bool newGame;

		public GlitchedArcadeGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			menuManager = new();
			roomManager = new();
			_graphics.PreferredBackBufferWidth = 640;
			_graphics.PreferredBackBufferHeight = 480;
			_graphics.ApplyChanges();
			Window.Title = "Freddy's Glitched Arcade";
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			menuManager.LoadContent(Content);
			roomManager.LoadContent(Content);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{

			// TODO: Add your update logic here
			if (newGame == false)
			{
				menuManager.update(gameTime);
				if (menuManager.startNewGame)
				{
					newGame = true;
				}
				if (menuManager.Exit)
				{
					Exit();
				}
			}
			else if (newGame) 
			{
				roomManager.update(gameTime);
				if (roomManager.Exit) 
				{
					Exit();
				}
			}


			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.DarkGray);
			_spriteBatch.Begin();
			if (newGame == false)
			{
				menuManager.Draw(gameTime, _spriteBatch);
			}
			else if (newGame == true)
			{
				roomManager.Draw(gameTime, _spriteBatch);
			}
			_spriteBatch.End();
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}

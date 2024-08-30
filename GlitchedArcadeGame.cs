using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Freddys
{
	public class GlitchedArcadeGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private MenuManager menuManager;

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

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{

			// TODO: Add your update logic here

			menuManager.update(gameTime);

			if (menuManager.Exit) 
			{
				Exit();
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			_spriteBatch.Begin();
			menuManager.Draw(gameTime, _spriteBatch);
			_spriteBatch.End();
			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}

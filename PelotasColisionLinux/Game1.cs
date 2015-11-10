#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace PelotasColisionLinux
{
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;		
		//Recursos del juego
		Rectangle blueBar;
		Rectangle redBar;
		Rectangle ball;

		//Texturas
		Texture2D grass;
		Texture2D spriteSheet;

		//efector de sonido
		SoundEffect ballBounce;
		SoundEffect playerScored;

		Rectangle blueSrcRect=new Rectangle(0,0,32,128);
		Rectangle redSrcRect=new Rectangle(32,0,32,128);
		Rectangle ballSrcRect=new Rectangle(64,0,32,32);

		Vector2 ballVelocity=Vector2.Zero;

		int blueScore=0;
		int redScore=0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = false;		
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
			blueBar = new Rectangle (
				32,
				GraphicsDevice.Viewport.Bounds.Height/2-64,
				32,
				128
				);
			redBar = new Rectangle (
				GraphicsDevice.Viewport.Bounds.Width-64,
				GraphicsDevice.Viewport.Bounds.Height/2-64,
				32,
				128
				);
			ball = new Rectangle (
				GraphicsDevice.Viewport.Bounds.Width/2-16,
				GraphicsDevice.Viewport.Bounds.Height/2-16,
				32,
				32
				);
            base.Initialize();
				
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			grass=Content.Load<Texture2D>("Textures/Grass");
			spriteSheet=Content.Load<Texture2D>("Textures/Objects");

			ballBounce=Content.Load<SoundEffect>("Sounds/Bounce");
			playerScored = Content.Load<SoundEffect> ("Sounds/Supporters");
            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}
			if (Keyboard.GetState ().IsKeyDown (Keys.E))
				blueBar.Y -= 10;
			if (Keyboard.GetState ().IsKeyDown (Keys.D))
				blueBar.Y += 10;
			if (Keyboard.GetState ().IsKeyDown (Keys.Up))
				redBar.Y -= 10;
			if (Keyboard.GetState ().IsKeyDown (Keys.Down))
				redBar.Y += 10;

			if (redBar.Y < 0)
				redBar.Y = 0;
			if (blueBar.Y < 0)
				blueBar.Y = 0;
			if (redBar.Y + redBar.Height > GraphicsDevice.Viewport.Bounds.Height)
				redBar.Y = GraphicsDevice.Viewport.Bounds.Height-redBar.Height;
			if (blueBar.Y + redBar.Height > GraphicsDevice.Viewport.Bounds.Height)
				blueBar.Y = GraphicsDevice.Viewport.Bounds.Height-blueBar.Height;

			ball.X += (int)ballVelocity.X;
			ball.Y += (int)ballVelocity.Y;

			if (ball.X < 0) {
				redScore++;
				initBall ();
			}else if(ball.X+ball.Width>GraphicsDevice.Viewport.Bounds.Width){
				blueScore++;
				initBall ();

			}

            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

		private void initBall(){
			int speed = 5;
			Random rand = new Random ();
			switch(rand.Next(4)){
			case 0:
				ballVelocity.X = speed;
				ballVelocity.Y = speed;
				break;
			case 1:
				ballVelocity.X = -speed;
				ballVelocity.Y = speed;
				break;
			case 2:
				ballVelocity.X = speed;
				ballVelocity.Y = -speed;
				break;
			case 3:
				ballVelocity.X = -speed;
				ballVelocity.Y = -speed;
				break;
			}
			ball.X = GraphicsDevice.Viewport.Bounds.Width / 2 - ball.Width/2;
			ball.Y = GraphicsDevice.Viewport.Bounds.Height / 2 - ball.Height / 2;
		}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           	graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin ();
			spriteBatch.Draw (
				grass,
				GraphicsDevice.Viewport.Bounds,
				Color.White
				);
			spriteBatch.End ();
            //TODO: Add your drawing code here
			spriteBatch.Begin (SpriteSortMode.Immediate,BlendState.AlphaBlend);
			spriteBatch.Draw (spriteSheet,redBar,redSrcRect,Color.White);
			spriteBatch.Draw (spriteSheet,blueBar,blueSrcRect,Color.White);
			spriteBatch.Draw (spriteSheet,ball,ballSrcRect,Color.White);
			spriteBatch.End ();
			if (Keyboard.GetState ().IsKeyDown (Keys.Space))
				initBall ();

			if (ball.Y < 0 || ball.Y + ball.Height > GraphicsDevice.Viewport.Bounds.Height) {
				ballVelocity.Y = -ballVelocity.Y;
				ballBounce.Play ();
			}

			if (ball.Intersects (redBar) || ball.Intersects (blueBar)) {
				ballVelocity.X = -ballVelocity.X;
				ballBounce.Play ();
			}
            
            base.Draw(gameTime);
        }
    }
}


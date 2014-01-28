using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Cholesterol
    {
        // Animation representing the player
        private Animation playerAnimation = new Animation();

        // Position of the Player relative to the upper left side of the screen
        public Vector2 position;
        public Vector2 direction;
        public float speed;

        SpriteFont debugFont;

        // State of the player
        public bool active;

        //textura
        Texture2D playerTexture;

        public int life = 3;
        public int damage = 20;

        //Para piscar na tela
        public Blinking blinking;

        //Tela
        int screenWidth;
        int screenHeight;

        // Retangulo Colisao
        public Rectangle collisionRect;

        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;
        Random rnd = new Random();

        
        public void Initialize(ContentManager _content, Vector2 _position)
        {

            playerTexture = _content.Load<Texture2D>("monster-2");
            debugFont = _content.Load<SpriteFont>("gamefont");
            playerAnimation.Initialize(playerTexture, 8, 1, 8, 60, 1f, true);
           

            // Set the player to be active
            active = true;

            //Posicao inicial
            position = _position;
            speed = 60f;
            
            //direcao inicial
            direction = new Vector2(-1, (float)(rnd.NextDouble() * 2) - 1);  

            //Instancia o retangulo de debug
            Primitives.Init(_content);

            //Instancia piscar
            blinking = new Blinking();
            blinking.Initialize(0.5f, 1f, 0.5f);

        }


        // Update the player animation
        public void Update(GameTime gameTime, int minTop, int maxBottom)
        {
            if (active)
            {
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                
                position += direction * speed * elapsedTime;

                //movimenta dentro das barreiras
                if (position.Y > maxBottom) direction = new Vector2(direction.X, -Math.Abs(direction.Y));
                if (position.Y < minTop) direction = new Vector2(direction.X, Math.Abs(direction.Y));   

                playerAnimation.position = position;
                playerAnimation.Update(gameTime);

                //Atualiza retangulo de colisao
                collisionRect = playerAnimation.destRect;
                //collisionRect.Inflate(5, -4);

                //Atualiza piscar
                blinking.Update(gameTime);
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (blinking.blink) playerAnimation.Draw(spriteBatch);
                if (debug)
                {
                    Primitives.DrawRect(spriteBatch, Color.White, playerAnimation.destRect);
                    Primitives.DrawRect(spriteBatch, Color.Red, collisionRect);  
                }

            }
        }

        public void setDebug(bool _debug)
        {
            debug = _debug;
        }

        public void blink()
        {
            blinking.On();
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace Nano
{
    class Player
    {
        // Animation representing the player
        private Animation playerAnimation = new Animation();

        // Position of the Player relative to the upper left side of the screen
        public Vector2 position;
        public float speed;

        // State of the player
        public bool active;

        //Para piscar na tela
        public Blinking blinking;

        //textura
        Texture2D playerTexture;

        //Tela
        int screenWidth;
        int screenHeight;

        // Retangulo Colisao
        public Rectangle collisionRect;

        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;


        // Initialize the asteroid
        public void Initialize(ContentManager _content, Vector2 _position, int s_width, int s_height)
        {

            playerTexture = _content.Load<Texture2D>("sprite-nave");
            playerAnimation.Initialize(playerTexture, 8, 6, 42, 60, 1f, true);

            // Resolucao tela
            screenWidth = s_width;
            screenHeight = s_height;

            // Set the player to be active
            active = true;

            //Posicao inicial
            position = _position;
            speed = 200f;

            //Instancia o retangulo de debug
            Primitives.Init(_content);

            //Instancia piscar
            blinking = new Blinking();
            blinking.Initialize(1f, 1f, 0.5f);
        }


        // Update the player animation
        public void Update(GameTime gameTime, int topPosition, int bottomPosition, Vector2 direction)
        {
            if (active)
            {
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                position += direction * speed * elapsedTime;
                //position = new Vector2(position.X, topPosition);

                //Limita o movimento do player
                    position = Vector2.Clamp(position,
                        new Vector2(collisionRect.Width / 2, topPosition),
                        new Vector2(screenWidth - collisionRect.Width / 2, bottomPosition));

                playerAnimation.position = position;
                playerAnimation.Update(gameTime);

                //Atualiza retangulo de colisao
                collisionRect = playerAnimation.destRect;
                collisionRect.Inflate(-10, -8);

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

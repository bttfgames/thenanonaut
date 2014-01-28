using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Virus
    {
        // Animation representing the player
        private Animation virusAnimation = new Animation();

        // Position of the Player relative to the upper left side of the screen
        public Vector2 position;
        public Vector2 direction;
        public float speed;
        Random rnd;

        // State of the player
        public bool active;

        //textura
        Texture2D virusTexture;
                
        //Tela
        int screenWidth;
        int screenHeight;

        // Retangulo Colisao
        public Rectangle collisionRect;

        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;

        //dano
        public int damage = 10;


        // Initialize the virus
        public void Initialize(ContentManager _content, Vector2 _position)
        {
            rnd = new Random();
            virusTexture = _content.Load<Texture2D>("monster-1");
            virusAnimation.Initialize(virusTexture, 2, 4, 8, 60, 1f, true);
           
            // Resolucao tela
            screenWidth = GameVars.screenWidth;
            screenHeight = GameVars.screenHeigth;

            // Set the player to be active
            active = true;

            //Posicao inicial
            position = _position;
            speed = 150f + (int)rnd.NextDouble()*100;

            
            //direcao inicial
            direction = new Vector2(-1, (float)(rnd.NextDouble() * 2) - 1 ); 

            //Instancia o retangulo de debug
            Primitives.Init(_content);
        }


        // Update the player animation
        public void Update(GameTime gameTime, int minTop, int maxBottom)
        {
            if (active)
            {
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                position += direction * speed * elapsedTime;

                //movimenta dentro das barreiras
                if (position.Y > maxBottom) direction = new Vector2(direction.X, - Math.Abs(direction.Y));
                if (position.Y < minTop) direction = new Vector2(direction.X, Math.Abs(direction.Y));   

                virusAnimation.position = position;
                virusAnimation.Update(gameTime);

                //Atualiza retangulo de colisao
                collisionRect = virusAnimation.destRect;
                collisionRect.Inflate(-2, -2);
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                virusAnimation.Draw(spriteBatch);
                if (debug)
                {
                    Primitives.DrawRect(spriteBatch, Color.White, virusAnimation.destRect);
                    Primitives.DrawRect(spriteBatch, Color.Red, collisionRect);
                   
                }

            }
        }

        public void setDebug(bool _debug)
        {
            debug = _debug;
        }
    }
}

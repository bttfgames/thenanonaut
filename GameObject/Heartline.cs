using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Heartline
    {
        // Position of the Player relative to the upper left side of the screen
        public Vector2 position;
        public Vector2 direction;
        public float speed;

        // State of the player
        public bool active;

        //textura
        Texture2D heartline;
                
        //Tela
        int screenWidth;
        int screenHeight;
        
        //Retangulo de origem do heartline
        Rectangle sourceRect;
        float length;
        public float scale;

        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;

        public bool freeze;
        public bool remove;

        // Initialize the virus
        public void Initialize(ContentManager _content, Vector2 _position)
        {
            heartline = _content.Load<Texture2D>("heartline");
           
            // Resolucao tela
            screenWidth = GameVars.screenWidth;
            screenHeight = GameVars.screenHeigth;

            // Set the player to be active
            active = true;

            //Posicao inicial
            position = _position;
            length = 0f;
            
            //direcao inicial
            direction = new Vector2(-1, 0);

            speed = 200.0f;
            remove = false;
            scale = 1.0f;

            //Instancia o retangulo de debug
            Primitives.Init(_content);
        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                position += direction * speed * elapsedTime;

                if(!freeze)
                    length += speed * elapsedTime;

                sourceRect = new Rectangle(0, 0, (int) length, heartline.Height);

                if (position.X < - length )
                    remove = true;
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(heartline, position, sourceRect, Color.White, 0, new Vector2(0,0), scale, SpriteEffects.FlipHorizontally, 0);

            }
        }

        public void setDebug(bool _debug)
        {
            debug = _debug;
        }
    }
}

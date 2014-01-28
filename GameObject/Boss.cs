using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Boss
    {
        // Animation representing the player
        private Animation playerAnimation = new Animation();

        // Position of the Player relative to the upper left side of the screen
        public Vector2 position;
        public Vector2 direction;

        public Vector2 healthBarPosition;

        public float speed;

        SpriteFont debugFont;

        // State of the player
        public bool active;

        //textura
        Texture2D playerTexture;
        Texture2D healthBarTexture;
        Texture2D healthBarTexture2;

        public int life = 60;

        // Retangulo Colisao
        public Rectangle collisionRect;

        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;


        
        public void Initialize(ContentManager _content, Vector2 _position)
        {

            playerTexture = _content.Load<Texture2D>("boss");
            healthBarTexture = _content.Load<Texture2D>("screens\\healthbar");
            healthBarTexture2 = _content.Load<Texture2D>("screens\\healthbar_02");
            debugFont = _content.Load<SpriteFont>("gamefont");
            playerAnimation.Initialize(playerTexture, 4, 1, 4, 60, 1f, true);
           

            // Set the player to be active
            active = true;

            //Posicao inicial
            position = _position;
            speed = 60f;
            
            //direcao inicial
            direction = new Vector2(-1, 0);

            healthBarPosition = new Vector2(400, 480 + healthBarTexture.Height);

            //Instancia o retangulo de debug
            Primitives.Init(_content);

        }


        // Update the player animation
        public void Update(GameTime gameTime, Vector2 _position)
        {
            if (active)
            {
                position = _position;

                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                healthBarPosition += new Vector2(0, -150) * elapsedTime ;

                if (healthBarPosition.Y <= 100)
                    healthBarPosition.Y = 100;
                //position += direction * speed * elapsedTime;

                playerAnimation.position = position;
                playerAnimation.Update(gameTime);

                //Atualiza retangulo de colisao
                collisionRect = playerAnimation.destRect;
                collisionRect.Inflate(-5, -4);
             
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                spriteBatch.Draw(healthBarTexture2, healthBarPosition, Color.White);
                if (life < 10)
                    spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, life * 5, 
                                     healthBarTexture.Height), Color.Red);
                else
                    spriteBatch.Draw(healthBarTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, life * 5,
                                     healthBarTexture.Height), Color.Green);

                if (life <= 60 && life >= 30)
                {
                    playerAnimation.Draw(spriteBatch);
                    GameVars.quantCho = 2;
                    GameVars.quantVirus = 6;
                }
                
                if (life < 30 && life >=15)
                {
                    playerAnimation.Draw(spriteBatch, Color.SaddleBrown);
                    GameVars.quantCho = 3;
                    GameVars.quantVirus = 9;
                }
                
                if (life < 15)
                {
                    playerAnimation.Draw(spriteBatch, Color.Red);
                    GameVars.quantCho = 4;
                    GameVars.quantVirus = 12;
                }

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
    }
}

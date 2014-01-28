using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Shoot
    {
        // Animation representing
        private Animation shootAnimation = new Animation();

        // Position
        public Vector2 position;

        // State
        public bool active;

        //textura
        Texture2D shootTexture;

        // Retangulo Colisao
        public Rectangle collisionRect;

        // The time since we last updated the frame
        float elapsedTime;

        // Speed
        float speed;
        Vector2 direction;

        //retangulo debug
        bool debug = false;


        // Initialize the asteroid
        public void Initialize(ContentManager content, Vector2 _position)
        {

            shootTexture = content.Load<Texture2D>("sprite-tiro");
            shootAnimation.Initialize(shootTexture, 1, 5, 5, 60, 1f, true);

            // Set the player to be active
            active = true;

            //Seta origem e destino
            this.position = _position;
            shootAnimation.position = position;

            //Seta movimento
            speed = 600f;
            direction = new Vector2(1, 0);
        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                position += direction * speed * elapsedTime;

                shootAnimation.position = position;
                shootAnimation.Update(gameTime);
                //active = shootAnimation.active;

                //Atualiza retangulo de colisao
                collisionRect = shootAnimation.destRect;
                collisionRect.Inflate(-20, -5);
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active) shootAnimation.Draw(spriteBatch);

            if (debug)
            {
                Primitives.DrawRect(spriteBatch, Color.White, shootAnimation.destRect);
                Primitives.DrawRect(spriteBatch, Color.Red, collisionRect);
            }
        }

    }
}

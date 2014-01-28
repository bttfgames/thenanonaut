using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Nano
{
    class Explosion
    {
        // Animation representing
        private Animation ExplosionAnimation = new Animation();

        // Position
        public Vector2 position;

        // State
        public bool active;

        //textura
        Texture2D explosionTexture;

        // Retangulo Colisao
        public Rectangle collisionRect;

        //retangulo debug
        bool debug = false;


        // Initialize the asteroid
        public void Initialize(ContentManager content, Vector2 _position, int tipo)
        {

            explosionTexture = content.Load<Texture2D>("explode-monster-" + tipo.ToString());
            ExplosionAnimation.Initialize(explosionTexture, 4, 1, 4, 90, 1f, false);

            // Set the player to be active
            active = true;

            //Seta origem e destino
            this.position = _position;
            ExplosionAnimation.position = position;
        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                ExplosionAnimation.Update(gameTime);
                active = ExplosionAnimation.active;
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active) ExplosionAnimation.Draw(spriteBatch);
        }

    }
}

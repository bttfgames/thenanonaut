// Animation.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nano

{
    public class Animation
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        // The scale used to display the sprite strip
        public float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCountT;

        // The number of frames that the animation contains in X
        int frameCountX;

        // The number of frames that the animation contains in Y
        int frameCountY;

        // The index of the current frame we are displaying
        int currentFrame;

        // The index of the current frame we are displaying
        int currentFrameX;

        // The index of the current frame we are displaying
        int currentFrameY;

        // The color of the frame we will be displaying
        public Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // Width of a given frame
        public int frameWidth;

        // Height of a given frame
        public int frameHeight;

        // The state of the Animation
        public bool active;

        // Determines if the animation will keep playing or deactivate after one run
        bool looping;

        // Width of a given frame
        public Vector2 position;

        // Origem
        Vector2 Origin;

        // Angulo do Sprite
        public float angle;

        //Flip imagem
        public bool flip;

        // The area of the image strip we want to display
        public Rectangle destRect = new Rectangle();

        public void Initialize(Texture2D texture,
        int frameCountX, int frameCountY,int frameCountT,
        int frametime, float scale, bool _looping)
        {
            // Keep a local copy of the values passed in
            this.color = Color.White;
            this.frameWidth = texture.Width / frameCountX;
            this.frameHeight = texture.Height / frameCountY;
            this.frameCountX = frameCountX;
            this.frameCountY = frameCountY;
            this.frameTime = frametime;
            this.frameCountT = frameCountT;
            this.scale = scale;

            looping = _looping;
            position = Vector2.Zero;
            spriteStrip = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            active = true;

            flip = false;
        }


        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (active == false)
                return;

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++; 
                currentFrameX++;

                if (currentFrameX == frameCountX) 
                { 
                    currentFrameY++;
                    currentFrameX = 0;
                    if (currentFrameY == frameCountY) {currentFrameY = 0; }
                }
                

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCountT)
                {
                    currentFrame = 0;
                    currentFrameY = 0;
                    currentFrameX = 0;

                    // If we are not looping deactivate the animation
                    if (looping == false)
                        active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }


            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrameX * frameWidth, currentFrameY * frameHeight, frameWidth, frameHeight);

            
            // Origem do Sprite
            Origin = new Vector2(frameWidth / 2, frameHeight / 2);

            // Retangulo de destino
            Vector2 destOrigin = position - Origin;
            destRect = new Rectangle((int)destOrigin.X, (int)destOrigin.Y, frameWidth, frameHeight);

        }


        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (active)
            {
                //spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
                if (flip)
                {
                    spriteBatch.Draw(spriteStrip, position, sourceRect, color, angle, Origin, scale, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(spriteStrip, position, sourceRect, color, angle, Origin, scale, SpriteEffects.None, 0);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color _color)
        {
            if (flip)
            {
                spriteBatch.Draw(spriteStrip, position, sourceRect, _color, angle, Origin, scale, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(spriteStrip, position, sourceRect, _color, angle, Origin, scale, SpriteEffects.None, 0);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color _color, float _scale)
        {
            if (flip)
            {
                spriteBatch.Draw(spriteStrip, position, sourceRect, _color, angle, Origin, _scale, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(spriteStrip, position, sourceRect, _color, angle, Origin, _scale, SpriteEffects.None, 0);
            }
        }

    }
}

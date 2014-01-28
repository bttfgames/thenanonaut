using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


namespace Nano
{
    class Heart
    {
        
        public Animation heartAnimation1 = new Animation();
        public Animation heartAnimation2 = new Animation();

        public float pulse;
        public float lastPulse;
        public bool didPulsePlay = true;
        public Color heartColor;

        List<Heartline> Heartline_List = new List<Heartline>();

        // State of the player
        public bool active;

        //texturas
        Texture2D heart1;
        Texture2D heart2;
        Texture2D heartline;
              
        // The time since we last updated the frame
        float elapsedTime;

        //retangulo debug
        bool debug = false;
        public bool isDead = false;
        float interval;
        public float beatRate;
        public float frequency;
        public float deltaFreq = 0;

        //som da batida
        SoundEffect beat;
        SoundEffect flatline;

        ContentManager contentManager;

        // Initialize the asteroid
        public void Initialize(ContentManager _content)
        {
            contentManager = _content;
            heart1 = _content.Load<Texture2D>("heart1");
            heart2 = _content.Load<Texture2D>("heart2");
            beat = _content.Load<SoundEffect>("sound\\heartbeat");
            flatline = _content.Load<SoundEffect>("sound\\flatline");

            heartAnimation1.Initialize(heart1, 1, 1, 1, 60, 1.0f, true);
            heartAnimation2.Initialize(heart2, 1, 1, 1, 60, 1.0f, true);

            // Set the heart animation position
            heartAnimation1.position = new Vector2(400, 430);
            heartAnimation2.position = new Vector2(400, 430);

            // Set the player to be active
            active = true;

            //Instancia o retangulo de debug
            Primitives.Init(_content);

            beatRate = 1f;
            interval = beatRate;
            heartColor = Color.White;
        }


        // Update the player animation
        public void Update(GameTime gameTime)
        {
            if (active)
            {
                
                // Pulsate the size of the selected menu entry.
                double time = gameTime.TotalGameTime.TotalSeconds;
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                interval -= elapsed;

                if (GameVars.energy <= 0)
                {
                    if (beatRate != 0f)
                    {
                        flatline.Play((float)GameVars.soundVolume / 10f, 0f, 0f);
                        heartColor = Color.Purple;
                        isDead = true;
                    }
                    beatRate = 0f;
                }

                if (beatRate > 0)
                    frequency = 1 / beatRate;
                else
                    frequency = 0;

                pulse = (float)Math.Pow(Math.Sin(Math.PI * time * frequency - deltaFreq), 6);

                // 
                if (lastPulse < 0.5f && pulse > 0.5f)
                {
                    if (!didPulsePlay)
                    {
                        beat.Play((float)GameVars.soundVolume / 10f, 0.5f, 0f);
                        didPulsePlay = true;

                        if(Heartline_List.Count > 0)
                            Heartline_List[Heartline_List.Count - 1].freeze = true;
                        Heartline h = new Heartline();
                        h.Initialize(contentManager, new Vector2(350, 350));
                        Heartline_List.Add(h);

                        // Pra não perder a fase da animação, a janela de alterar frequencia é logo após o pulso
                        beatRate = GameVars.energy/100.0f;
                    }
                }
                else
                    didPulsePlay = false;

                lastPulse = pulse;

                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                heartAnimation1.Update(gameTime);
                heartAnimation2.Update(gameTime);

                foreach (Heartline h in Heartline_List) h.Update(gameTime);

                CleanList();
            }

        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                float scale = 1 + pulse * 0.5f;
                heartAnimation2.Draw(spriteBatch, heartColor, scale);
                heartAnimation1.Draw(spriteBatch, heartColor, scale);

                foreach (Heartline h in Heartline_List) h.Draw(spriteBatch);
            }
        }

        void CleanList()
        {
            for (int i = 0; i < Heartline_List.Count; i++)
            {
                if (Heartline_List[i].remove)
                {
                    Heartline_List.RemoveAt(i);
                    break;
                }
            }
        }

        public void setDebug(bool _debug)
        {
            debug = _debug;
        }
    }
}

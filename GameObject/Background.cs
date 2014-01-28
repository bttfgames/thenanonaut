using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;


namespace Nano
{
    class Background
    {
        // Position
        public Vector2 position;

        // The time since we last updated the frame
        float elapsedTime;

        //Dados deslocamento
        Vector2 direction;

        // State
        public bool active;

        public List<Texture2D> textures;
        public List<Texture2D> controles;
        public int lengthScene; //comprimento da cena

        Texture2D bgTexture;
        Texture2D pixel;

        // Origem
        Vector2 origin;

        //Posicao do teto
        public int[] topPosition;
        public int[] bottomPosition;

        //Shaking
        public bool isShaking;
        Vector2 shaking;
        float startShake;

        Random rand = new Random();

        // Initialize the asteroid
        public void Initialize(ContentManager content, Vector2 _position)
        {
            //cenarioTexture = content.Load<Texture2D>("cenario");
            //controleTexture = content.Load<Texture2D>("alpha");
            bgTexture = content.Load<Texture2D>("screens\\background_level");
            pixel = content.Load<Texture2D>("pixel");
            textures = new List<Texture2D>(){
                content.Load<Texture2D>("screens\\cenario_inicio_01"),
                content.Load<Texture2D>("screens\\cenario_inicio_02"),
                content.Load<Texture2D>("screens\\cenario_inicio_03"),
                content.Load<Texture2D>("screens\\cenario_inicio_04"),
                content.Load<Texture2D>("screens\\cenario_inicio_05"),
                content.Load<Texture2D>("screens\\cenario_fim"),
                content.Load<Texture2D>("screens\\cenario_fim"),
                content.Load<Texture2D>("screens\\cenario_fim"),
                content.Load<Texture2D>("screens\\cenario_fim"),
                content.Load<Texture2D>("screens\\cenario_pre_boss"),
                content.Load<Texture2D>("screens\\cenario_boss"),
            };

            controles = new List<Texture2D>(){
                content.Load<Texture2D>("screens\\cenario_inicio_01_alfa"),
                content.Load<Texture2D>("screens\\cenario_inicio_02_alfa"),
                content.Load<Texture2D>("screens\\cenario_inicio_03_alfa"),
                content.Load<Texture2D>("screens\\cenario_inicio_04_alfa"),
                content.Load<Texture2D>("screens\\cenario_inicio_05_alfa"),
                content.Load<Texture2D>("screens\\cenario_fim_alfa"),
                content.Load<Texture2D>("screens\\cenario_fim_alfa"),
                content.Load<Texture2D>("screens\\cenario_fim_alfa"),
                content.Load<Texture2D>("screens\\cenario_fim_alfa"),
                content.Load<Texture2D>("screens\\cenario_fim_alfa"),
                content.Load<Texture2D>("screens\\cenario_boss_alfa"),
            };

            //Origin
            origin = new Vector2(0,0);

            // Set active
            active = true;

            //Seta origem
            this.position = _position;

            //Seta momento
            direction = new Vector2(0, -1);

            //Velocidade da tela
            GameVars.speed = 200f;

            isShaking = false;
            shaking = new Vector2();

            // Get Color data of each Texture
            lengthScene = 0;
            for (int i = 0; i < textures.Count; i++) lengthScene += textures[i].Width;
            topPosition = new int[lengthScene];
            bottomPosition = new int[lengthScene];

            int index = 0;
            for (int i = 0; i < controles.Count; i++)
            {
                CalculateTopBottom(controles[i], index);
                index += controles[i].Width;
            }
        }


        // Update the player animation
        public void Update(GameTime gameTime, Vector2 _direction)
        {
            if (active)
            {
                direction = _direction;
                //Atualiza posicao tiro
                elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
                position += direction * GameVars.speed * elapsedTime;

                position = Vector2.Clamp(position, new Vector2(- lengthScene + 1, 0), new Vector2(0,0));

                if (- position.X > lengthScene - 800)
                {
                    //GameVars.speed *= (1f - 0.05f);
                    GameVars.speed = 0;
                }

                if (isShaking) shakeBackground(gameTime);

            }
        }

        // Draw the background
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                //Desenha o tile de fundo
                for(int x = 0; x < GameVars.screenWidth; x += bgTexture.Width ){
                    for( int y = 0; y < GameVars.screenHeigth; y += bgTexture.Height) 
                        spriteBatch.Draw(bgTexture, new Vector2(x,y), Color.White);
                }


                //Desenha o cenario
                float posInc = 0;

                for (int i = 0; i < textures.Count; i++)
                {
                    spriteBatch.Draw(textures[i], position + new Vector2(posInc, 0) + shaking, null, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
                    posInc += textures[i].Width;
                }


                ////debug limites
                //for (int i = 0; i < GameVars.screenWidth; i++)
                //{
                //    spriteBatch.Draw(pixel, new Vector2(i, topPosition[(int)-position.X + i]), Color.White);
                //    spriteBatch.Draw(pixel, new Vector2(i, bottomPosition[(int)-position.X + i]), Color.White);
                //}

            }
        }

        public void CalculateTopBottom(Texture2D controleTexture, int index)
        {
            Color[] bitsA;
            bitsA = new Color[controleTexture.Width * controleTexture.Height];
            controleTexture.GetData(bitsA);
            for (int v = 0; v < controleTexture.Width; v++)
                {
                    for (int i = 0; i < controleTexture.Height; ++i)
                    {
                        // Get the color from each texture
                        Color a = bitsA[(v) + (i * controleTexture.Width)];
                        if (a.A == 0) // If color are transparent (the alpha channel is 0)
                            {
                                topPosition[v + index] = i;
                                break;
                            }
                    }

                }


            for (int v = 0; v < controleTexture.Width; v++)
                {
                    for (int i = 1; i <= controleTexture.Height; ++i)
                    {
                        // Get the color from each texture
                        Color a = bitsA[(v) + ((controleTexture.Height - i) * controleTexture.Width)];
                        if (a.A == 0) // If color are transparent (the alpha channel is 0)
                        {
                            bottomPosition[v + index] = controleTexture.Height - i;
                            break;
                        }
                    }

                }

        }

        public int GetMinTop(Rectangle rect)
        {
            int minTop = 0;
            int posX = rect.Left;
            posX += -(int)position.X;

            
            for (int i = 0; i <= rect.Width; i++)
			{
                if (posX >= lengthScene) posX = lengthScene -1;
                minTop = Math.Max(topPosition[posX], minTop);
                posX++;
			}
            return minTop + rect.Height / 2;
        }

        public int GetMaxBottom(Rectangle rect)
        {
            int maxBottom = 480;
            int posX = rect.Left;
            posX += -(int)position.X;

            for (int i = 0; i <= rect.Width; i++)
            {
                if (posX >= lengthScene) posX = lengthScene -1;
                maxBottom = Math.Min(bottomPosition[posX], maxBottom);
                posX++;
            }
            return maxBottom - rect.Height / 2;
        }

        public void shakeBackground(GameTime gameTime)
        {
            float range = 10;

            shaking = new Vector2(range / 2f - (float)rand.NextDouble() * range, range / 2f - (float)rand.NextDouble() * range);
            startShake += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            if (startShake > 1.0f)
            {
                isShaking = false;
                startShake = 0f;
                shaking = new Vector2();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nano
{
    class Blinking
    {
        //Blinking
        public bool isblinking = false;

        //Se esta ligado ou desligado
        public bool blink = true;

        //Tempo entre piscadas
        public float blinktime_on;
        public float blinktime_off;
        public float blinktime_total;

        //Varialvel de controle
        float blinktime_control;

        //Controle do tempo
        float ElapsedTime;
        float TotalElapsedTime;

        public void Initialize(float t_blinktime_total, float t_blinktime_on, float t_blinktime_off)
        {
            blinktime_on = t_blinktime_on;
            blinktime_off = t_blinktime_off;
            blinktime_total = t_blinktime_total;
            isblinking = false;
        }

        public void Update(GameTime gameTime)
        {
            if (isblinking)
            {
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                TotalElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


                if (blinktime_control < ElapsedTime % (blinktime_control + 1))
                {
                    if (blink)
                    {
                        blink = false;
                        blinktime_control = blinktime_off;
                        ElapsedTime = 0;
                    }
                    else
                    {
                        blink = true;
                        blinktime_control = blinktime_on;
                        ElapsedTime = 0;
                    }
                }

                //Se acabou o tempo delsiga o Blinking
                if (blinktime_total * 1000 < TotalElapsedTime)
                {
                    isblinking = false;
                    blink = true;
                    ElapsedTime = 0;
                    TotalElapsedTime = 0;
                }  

            }
        }

        public void On()
        {
            isblinking = true;
        }

    }
}

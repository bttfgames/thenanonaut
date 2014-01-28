using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;

namespace Nano
{
    public class Joystick
    {
        //Variaveis
        GraphicsDevice graphicsDevice;
        ContentManager content;

        private Vector2 centerJoy;
        private Vector2 dirJoy;
        public bool fire;
        private bool firePressed;

        //Textura joystick
        Texture2D joystickTexture;

        // Origem
        Vector2 origin;

        // State
        public bool active;

        //Tela
        int screenWidth;
        int screenHeight;

        //Tipo de controle
        int typeControl;

        //Acelerometro
        Accelerometer accelerometer;
        public bool isCalibrate;
        public bool calibrateFinish;

        //Sensibilidade
        public int sensibilityX;
        public int sensibilityY;
        public int sensibilityJoy;

        public Joystick(ContentManager _content, GraphicsDevice _graphicsDevice, Vector2 _centerJoy)
        {
            graphicsDevice = _graphicsDevice;
            content = _content;

            joystickTexture = content.Load<Texture2D>("joystick");

            //Origin
            origin = new Vector2(joystickTexture.Width / 2, joystickTexture.Height / 2);

            this.centerJoy = _centerJoy;
            this.dirJoy = new Vector2(0, 0);
            this.fire = false;

            // Resolucao tela
            screenWidth = GameVars.screenWidth;
            screenHeight = GameVars.screenHeigth;

            active = true;

            typeControl = GameVars.typeController;

            startAccelerometer();
            isCalibrate = false;
            calibrateFinish = false;

            //Sensibilidade
            sensibilityX = 10;
            sensibilityY = 6;
            sensibilityJoy = 50;
            
        }

        private void startAccelerometer()
        {
            if (accelerometer == null)
            {
                // Instantiate the Accelerometer.
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
            }

            try
            {
                accelerometer.Start();
            }
            catch (InvalidOperationException ex)
            {
                //statusTextBlock.Text = "unable to start accelerometer.";
            }

        }


        public void Update(InputState input)
        {
            //Se atirou seta estado
            if (fire)
            {
                firePressed = true;
                fire = false;
            }
            typeControl = GameVars.typeController;

            switch (typeControl)
            {
                case 0:
                    useJoystick(input);
                    break;

                case 1:
                    useAccelerometer(input);
                    break;

                case 2:
                    useKeyboard();
                    break;
            }

        }

        void useJoystick(InputState input)
        {
            foreach (TouchLocation tl in input.TouchState)
            {
                if (tl.State == TouchLocationState.Pressed)
                {
                    //Se tocou na tela
                    if (tl.Position.X * (1f / GameVars.horScaling) < screenWidth / 2)
                    {
                        dirJoy = new Vector2(tl.Position.X * (1f / GameVars.horScaling), tl.Position.Y * (1f / GameVars.verScaling));
                        dirJoy -= centerJoy;
                        dirJoy /= sensibilityJoy;
                        //dirJoy.Normalize();

                    }
                    else
                    {
                        if (!firePressed)
                        {
                            fire = true;
                        }
                    }
                }

                //Se moveu o toque
                if (tl.State == TouchLocationState.Moved)
                {
                    if (tl.Position.X * (1f / GameVars.horScaling) < screenWidth / 2)
                    {
                        dirJoy = new Vector2(tl.Position.X * (1f / GameVars.horScaling), tl.Position.Y * (1f / GameVars.verScaling));
                        dirJoy -= centerJoy;
                        dirJoy /= sensibilityJoy;
                        //dirJoy.Normalize();
                    }
                }

                //Se deixou de tocar a tela
                if (tl.State == TouchLocationState.Released)
                {
                    if (tl.Position.X * (1f / GameVars.horScaling) < screenWidth / 2)
                    {
                        dirJoy = new Vector2(0, 0);
                    }
                    else
                    {
                        fire = false;
                        firePressed = false;
                    }
                }
            }
        }

        public void calibrate(InputState input)
        {
            //Pega o vetor de estado do acelerometro
            Vector3 acceleration = accelerometer.CurrentValue.Acceleration;

            //Se segurou na tela coloca este como posicao default
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Hold)
                {
                    GameVars.confortablePosition = acceleration;
                }
            }
        }

        void useAccelerometer(InputState input)
        {

            foreach (TouchLocation tl in input.TouchState)
            {
                if (tl.State == TouchLocationState.Pressed)
                {
                    if (!firePressed)
                    {
                        fire = true;
                    }
                }

                //Se deixou de tocar a tela
                if (tl.State == TouchLocationState.Released)
                {
                    fire = false;
                    firePressed = false;
                }
            }

            //Pega o vetor de estado do acelerometro
            Vector3 acceleration = accelerometer.CurrentValue.Acceleration;

            //Se segurou na tela coloca este como posicao default
            isCalibrate = false;

            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Hold)
                {
                    GameVars.confortablePosition = acceleration;
                    //SaveLoadState.SaveToIsolatedStorage();
                    isCalibrate = true;
                }

                if (gesture.GestureType == GestureType.DoubleTap)
                {
                    calibrateFinish = true;
                }

            }

            acceleration -= GameVars.confortablePosition;
            dirJoy = new Vector2(-acceleration.Y * sensibilityX, -acceleration.X * sensibilityY);
        }

        void useKeyboard()
        {
            int speedInc = 2;
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Left)) dirJoy = new Vector2(-1, 0);
            if (newState.IsKeyDown(Keys.Right)) dirJoy = new Vector2(1, 0);
            if (newState.IsKeyDown(Keys.Up)) dirJoy = new Vector2(0, -1);
            if (newState.IsKeyDown(Keys.Down)) dirJoy = new Vector2(0, 1);

            //Se soltou
            if (newState.IsKeyUp(Keys.Left) && newState.IsKeyUp(Keys.Right) &&
                newState.IsKeyUp(Keys.Up) && newState.IsKeyUp(Keys.Down))
            {
                dirJoy = new Vector2(0, 0);
            }

            if (newState.IsKeyDown(Keys.Space))
            {
                if (!firePressed)
                {
                    fire = true;
                }
            }

            //Se deixou de tocar a tela
            if (newState.IsKeyUp(Keys.Space))
            {
                fire = false;
                firePressed = false;
            }

            //Incrementa a velocidade para teste no teclado
            dirJoy *= speedInc;
        }

        public Vector2 GetDirJoy()
        {
            return dirJoy;
        }

        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (typeControl == 0)
                spriteBatch.Draw(joystickTexture, centerJoy, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
            }
        }

    }
}

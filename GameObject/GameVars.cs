using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Nano
{
    class GameVars
    {
        //Screen
        public static int screenWidth;
        public static int screenHeigth;
        public static float horScaling;
        public static float verScaling;
        //public static Matrix globalTransformation;
        public static bool isScaling;

        //Variaveis jogo
        public static int score;
        public static int energy;
        public static float speed;

        //controle
        public static int typeController;
        public static bool musicMute;
        public static int musicVolume;
        public static int soundVolume;

        //variaveis para score
        public static int scoreVirus;
        public static int scoreChol;

        //Posicao confortavel para acelerometro
        public static Vector3 confortablePosition;

        //Para record
        public const string HighscorePopupTitle = "You made a high score!";
        public const string HighscorePopupText = "Enter your name (max 15 characters)";
        public const string HighscorePopupDefault = "Player";

        //chefe
        public static bool bossIsDead;
        public static int quantVirus;
        public static int quantCho;
        public static bool isBoss;

        public static void Init()
        {
            //Resolucao do game
            screenWidth = 800;
            screenHeigth = 480;
            horScaling = 1f;
            verScaling = 1f;

            //Som
            musicMute = false;
            musicVolume = 2;
            soundVolume = 10;

            //Quanto ganha de score por
            scoreVirus = 10;
            scoreChol = 40;

            //Para balanceamento
            

        }

        public static void InitGameVars()
        {
            //Variaveis game
            //typeController = 0;
            score = 0;
            energy = 100;
            bossIsDead = false;
            quantVirus = 3;
            quantCho = 1;
            isBoss = false;

        }
    }
}

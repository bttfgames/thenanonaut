#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Nano
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields
        MenuEntry controllerMenuEntry;
        MenuEntry MusicMenuEntry;
        MenuEntry musicVolumeMenuEntry;
        MenuEntry soundVolumeMenuEntry;

        enum Controller
        {
            Joystick,
            Accelerometer,
            Keyboard,
        }

        static Controller currentController;

        static bool Music = true;

        static int musicVolume;
        static int soundVolume;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("")
        {
            //Default
            //SaveLoadState.LoadFromIsolatedStorage();
            currentController = (Controller)GameVars.typeController;
            musicVolume = GameVars.musicVolume;
            soundVolume = GameVars.soundVolume;
            Music = GameVars.musicMute;
            //if (!GameVars.OutsideMusic)
            //{
                MediaPlayer.IsMuted = !Music;
                MediaPlayer.Volume = (float)soundVolume / 10f;
            //}

            // Create our menu entries.
            controllerMenuEntry = new MenuEntry(string.Empty);
            MusicMenuEntry = new MenuEntry(string.Empty);
            musicVolumeMenuEntry = new MenuEntry(string.Empty);
            soundVolumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            // Hook up menu event handlers.
            controllerMenuEntry.Selected += UngulateMenuEntrySelected;
            MusicMenuEntry.Selected += MusicMenuEntrySelected;
            musicVolumeMenuEntry.Selected += musicVolumeMenuEntrySelected;
            soundVolumeMenuEntry.Selected += soundVolumeMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(controllerMenuEntry);
            MenuEntries.Add(MusicMenuEntry);
            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(soundVolumeMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            controllerMenuEntry.Text = "Controller: " + currentController;
            MusicMenuEntry.Text = "Music: " + (!Music ? "on" : "off");
            musicVolumeMenuEntry.Text = "Music volume: " + musicVolume;
            soundVolumeMenuEntry.Text = "Sound volume: " + soundVolume;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentController++;

            if (currentController > Controller.Keyboard)
                currentController = 0;

            SetMenuEntryText();

            //seta variavel
            GameVars.typeController = (int)currentController;
            //SaveLoadState.SaveToIsolatedStorage();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Music = !Music;

            SetMenuEntryText();

            //seta variavel
            //if (!Game1.OutsideMusic) MediaPlayer.IsMuted = !Music;
            GameVars.musicMute = Music;
            //SaveLoadState.SaveToIsolatedStorage();
        }

        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void musicVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            musicVolume++;
            if (musicVolume > 10)
                musicVolume = 0;

            SetMenuEntryText();
            //if (!Game1.OutsideMusic) MediaPlayer.Volume = (float)musicVolume / 10f;
            GameVars.musicVolume = musicVolume;
            MediaPlayer.Volume = (float)soundVolume / 10f;
            //SaveLoadState.SaveToIsolatedStorage();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void soundVolumeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            soundVolume++;
            if (soundVolume > 10)
                soundVolume = 0;

            SetMenuEntryText();
            GameVars.soundVolume = soundVolume;
            //SaveLoadState.SaveToIsolatedStorage();
        }


        #endregion

    }
}

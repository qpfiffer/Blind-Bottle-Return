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
#endregion

namespace bottleReturn
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry fullScreen;
        MenuEntry debugOptions;
        MenuEntry soundEnable;
        MenuEntry cursorEnable;

        Options globalOptions;
        
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(Options globalOptions)
            : base("Options")
        {
            this.globalOptions = globalOptions;
            // Create our menu entries.
            fullScreen = new MenuEntry(string.Empty);
            debugOptions = new MenuEntry(string.Empty);
            soundEnable = new MenuEntry(string.Empty);
            cursorEnable = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            fullScreen.Selected += FullscreenMenuEntrySelected;
            debugOptions.Selected += DebugMenuEntrySelected;
            soundEnable.Selected += SoundEnableEntrySelected;
            cursorEnable.Selected += CursorEnableEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(fullScreen);
            MenuEntries.Add(debugOptions);
            MenuEntries.Add(soundEnable);
            MenuEntries.Add(cursorEnable);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            fullScreen.Text = "Fullscreen: " + (globalOptions.FULLSCREEN ? "On" : "Off");
            debugOptions.Text = "Debug Text: " + (globalOptions.DEBUG_TEXT ? "On" : "Off");
            soundEnable.Text = "Sound: " + (globalOptions.SOUND_ENABLE ? "On" : "Off");
            cursorEnable.Text = "Mouse Cursor: " + (globalOptions.CURSOR_ENABLE ? "On" : "Off");
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Fullscreen menu entry is selected.
        /// </summary>
        void FullscreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            globalOptions.FULLSCREEN = !globalOptions.FULLSCREEN;
            Options temp = ScreenManager.GlobalOptions;
            temp.FULLSCREEN = globalOptions.FULLSCREEN;
            ScreenManager.GlobalOptions = temp;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Debug Text menu entry is selected.
        /// </summary>
        void DebugMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            globalOptions.DEBUG_TEXT = !globalOptions.DEBUG_TEXT;
            Options temp = ScreenManager.GlobalOptions;
            temp.DEBUG_TEXT = globalOptions.DEBUG_TEXT;
            ScreenManager.GlobalOptions = temp;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event Handler for when the Sound Enable menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SoundEnableEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            globalOptions.SOUND_ENABLE = !globalOptions.SOUND_ENABLE;
            Options temp = ScreenManager.GlobalOptions;
            temp.SOUND_ENABLE = globalOptions.SOUND_ENABLE;
            ScreenManager.GlobalOptions = temp;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event Handler for when the Mouse Cursor Enable menu entry is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CursorEnableEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            globalOptions.CURSOR_ENABLE = !globalOptions.CURSOR_ENABLE;
            Options temp = ScreenManager.GlobalOptions;
            temp.CURSOR_ENABLE = globalOptions.CURSOR_ENABLE;
            ScreenManager.GlobalOptions = temp;

            SetMenuEntryText();
        }
        #endregion
    }
}

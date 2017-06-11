using System;
using System.Windows.Forms;
using System.Diagnostics;
using SlimDX;
using SlimDX.Windows;
using System.Drawing;
using SlimDX.DirectInput;

namespace Gamology.System
{
    class GameWindow : IDisposable
    {
        # region member variables
        private bool m_IsDisposed = false;
        private bool m_IsInitialized = false;
        private bool m_IsFullScreen = false;
        //private bool m_IsPaused = false;
        private RenderForm m_Form;
        private Color4 m_ClearColor;
        private long m_CurrFrameTime;
        private long m_LastFrameTime;
        //private int m_FrameCount;
        private int m_FPS;
        private UserInput m_UserInput;
        # endregion

        public Color4 ClearColor
        {
            get
            {
                return this.m_ClearColor;
            }

            protected set
            {
                this.m_ClearColor = value;
            }
        }

        /// <summary>
        /// Returns the underlying RenderForm object that actually represents the window itself.
        /// </summary>
        public RenderForm FormObject => this.m_Form;

        public bool IsDisposed => this.m_IsDisposed;

        public UserInput UserInput => this.m_UserInput;

        public GameWindow(string title, int width, int height, bool fullscreen)
        {  // Store parameters in member variables.  
            this.m_IsFullScreen = fullscreen;
            this.m_ClearColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);
            // Create the game window that will display the game.  
            this.m_Form = new RenderForm(title);
            this.m_Form.ClientSize = new Size(width, height);
            // Hook up event handlers so we can receive events from the form  
            this.m_Form.FormClosed += this.FormClosed;
            //initializing user input
            this.m_UserInput = new UserInput();
        }

        public virtual void FormClosed(object o, FormClosedEventArgs e)
        {
            if (this.m_IsDisposed)
            {
                return;
            }

            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.m_IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Unregister events       
                this.m_Form.FormClosed -= this.FormClosed;
                // get rid of managed resources here 

                // dispose user input
                this.m_UserInput.Dispose();
            }

            // get rid of unmanaged resources here

            this.m_IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);

            // Since this Dispose() method already cleaned up the resources    
            // used by this object, there's no need for the Garbage Collector to   
            // call this class's Finalizer, so we tell it not to 

            GC.SuppressFinalize(this);
        }

        public virtual void GameLoop()
        {
            this.m_LastFrameTime = this.m_CurrFrameTime;
            this.m_CurrFrameTime = Stopwatch.GetTimestamp();

            UpdateScene((double) (this.m_CurrFrameTime - this.m_LastFrameTime) / Stopwatch.Frequency);

            RenderScene();

            // This code tracks our frame rate.
            this.m_FPS = (int)(Stopwatch.Frequency / (float)(this.m_CurrFrameTime - this.m_LastFrameTime)); 
        }

        public virtual void UpdateScene(double elapsedFrameTime)
        {
            // Get the latest user input
            this.m_UserInput.Update();

            if (this.m_UserInput.IsKeyPressed(Key.Return) && 
                (this.m_UserInput.IsKeyPressed(Key.LeftAlt) || m_UserInput.IsKeyPressed(Key.RightAlt)))
            {
                // Toggle full screen mode
                this.ToggleFullScreen();
            }

            else if (this.m_UserInput.IsKeyPressed(Key.Escape))
            {
                // close the program
                this.m_Form.Close();
            } 
        }

        public virtual void RenderScene()
        {
            if (!this.m_IsInitialized || this.m_IsDisposed)
            {
                return;
            }
        }

        public virtual void StartGameLoop()
        {
            if (this.m_IsInitialized)
            {
                return;
            }

            this.m_IsInitialized = true;

            MessagePump.Run(this.m_Form, this.GameLoop);
        }

        public void ToggleFullScreen()
        {
            this.m_IsFullScreen = !this.m_IsFullScreen;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.DirectInput;
using SlimDX.XInput;

namespace Gamology.System
{
    public class UserInput : IDisposable
    {
        private bool m_IsDisposed = false;
        private DirectInput m_DirectInput;
        private Keyboard m_Keyboard;
        private KeyboardState m_KeyboardStateCurrent;
        private KeyboardState m_KeyboardStateLast;
        private Mouse m_Mouse;
        private MouseState m_MouseStateCurrent;
        private MouseState m_MouseStateLast;

        public bool IsDisposed => this.m_IsDisposed;

        public Keyboard Keyboard => this.m_Keyboard;

        public UserInput()
        {
            InitDirectInput();
            this.m_KeyboardStateCurrent = new KeyboardState();
            this.m_KeyboardStateLast = new KeyboardState();
            this.m_MouseStateCurrent = new MouseState();
            this.m_MouseStateLast = new MouseState();
        }

        private void InitDirectInput()
        {
            this.m_DirectInput = new DirectInput();
            this.m_Keyboard = new Keyboard(this.m_DirectInput);
            this.m_Mouse = new Mouse(this.m_DirectInput);

            // For debugging purposes
            this.PrintAttachedInputDevices();
        }

        public void Update()
        {   // Reacquire the devices in case another application has  
            // taken control of them.  
            this.m_Keyboard.Acquire();  m_Mouse.Acquire();
            // Update our keyboard state variables.  
            this.m_KeyboardStateLast = m_KeyboardStateCurrent;
            this.m_KeyboardStateCurrent = m_Keyboard.GetCurrentState();
            // Update our mouse state variables.  
            this.m_MouseStateLast = m_MouseStateCurrent;
            this.m_MouseStateCurrent = m_Mouse.GetCurrentState();
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

               // get rid of managed resources
               if (this.m_DirectInput != null)
               {
                    this.m_DirectInput.Dispose();
               }

               if (this.m_Keyboard != null)
               {
                    this.m_Keyboard.Dispose();
               }

               if (this.m_Mouse != null)
               {
                    this.m_Mouse.Dispose();
               }
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

        public bool IsKeyPressed(Key key)
        {
            return this.m_KeyboardStateCurrent.IsPressed(key);
        }

        public bool IsKeyReleased(Key key)
        {
            return this.m_KeyboardStateCurrent.IsReleased(key);
        }

        public bool IsKeyHeldDown(Key key)
        {
            return this.m_KeyboardStateCurrent.IsPressed(key) && this.m_KeyboardStateLast.IsPressed(key);
        }

        public bool IsMouseButtonPressed(int button)
        {
            return this.m_MouseStateCurrent.IsPressed(button);
        }

        public bool IsMouseButtonReleased(int button)
        {
            return this.m_MouseStateCurrent.IsReleased(button);
        }

        public bool IsMouseButtonHeldDown(int button)
        {
            return this.m_MouseStateCurrent.IsPressed(button) && this.m_MouseStateLast.IsPressed(button);
        }

        public void PrintInputDevices(DeviceEnumerationFlags inputDeviceStatus)
        {
            IList<DeviceInstance> attachedInputDevices = this.m_DirectInput.GetDevices(DeviceClass.GameController, inputDeviceStatus);
            if (!attachedInputDevices.Any())
            {
                Console.WriteLine("No Game Controllers were found");
                return;
            }

            foreach(DeviceInstance device in attachedInputDevices)
            {
                Console.WriteLine("Game Controller Product Name: " + device.ProductName);
            }
        }

        public void PrintAttachedInputDevices()
        {
            PrintInputDevices(DeviceEnumerationFlags.AttachedOnly);
        }
    }
}

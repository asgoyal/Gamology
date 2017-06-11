using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System;
using SlimDX;
using SlimDX.Direct2D;
using SlimDX.DirectInput;
using SlimDX.Windows;
using SlimDXBitmap = SlimDX.Direct2D.Bitmap;
using SystemBitmap = System.Drawing.Bitmap;
using SlimDX.DXGI;
using Factory = SlimDX.Direct2D.Factory;
using System.Drawing.Imaging;
using SystemPixelFormat = System.Drawing.Imaging.PixelFormat;
using SlimDXPixelFormat = SlimDX.Direct2D.PixelFormat;

namespace Gamology.System
{
    class TileGameWindow : GameWindow, IDisposable
    {
        const float PLAYER_MOVE_SPEED = 0.05f;

        public struct Player
        {
            public float PositionX;
            public float PositionY;
            public int AnimFrame;
            public double LastFrameChange;
        }

        public struct Tile
        {
            public bool IsSolid;
            public int SheetPosX;
            public int SheetPosY;
        }

        private WindowRenderTarget m_RenderTarget;
        private Factory m_Factory;
        private Player m_Player;
        private SlimDXBitmap m_PlayerSprites;
        private SlimDXBitmap m_TileSheet;
        private List<Tile> m_TileList;
        private int[,] m_Map;
        private SolidColorBrush m_DebugBrush;

        public TileGameWindow(string title, int width, int height, bool fullscreen) 
            : base(title, width, height, fullscreen)
        {
            this.m_Factory = new Factory();

            RenderTargetProperties rtProperties = new RenderTargetProperties
            {
                PixelFormat = new SlimDXPixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)
            };

            WindowRenderTargetProperties properties = new WindowRenderTargetProperties
            {
                Handle = FormObject.Handle,
                PixelSize = new Size(width, height)
            };

            this.m_RenderTarget = new WindowRenderTarget(this.m_Factory, rtProperties, properties);

            this.m_DebugBrush = new SolidColorBrush(this.m_RenderTarget, new Color4(1.0f, 1.0f, 1.0f, 0.0f));

            this.m_PlayerSprites = LoadBitmap(Application.StartupPath + "\\Robot.png");
            this.m_Player = new Player
            {
                PositionX = 4,
                PositionY = 8
            };

            this.InitalizeTileGameWorld();
        }

        private void InitalizeTileGameWorld()
        {
            // Initiailize the world.
            // =======================================================================================================

            // Load the tile sheet.
            m_TileSheet = LoadBitmap(Application.StartupPath + "\\TileSheet.png");


            // Set up our tile list.
            m_TileList = new List<Tile>();

            // The code below sets up the behavior of each sprite in terms of whether the player can walk on it or not.
            // It also specifies the position of each sprite in the sprite sheet so we know which tile to use to draw it.

            // First row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 0, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 1, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 2, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 3, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 4, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 5, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 6, SheetPosY = 0 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 0 });

            // Second row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 1 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 1 });

            // Third row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 0, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 1, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 2, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 3, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 4, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 5, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 6, SheetPosY = 2 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 7, SheetPosY = 2 });

            // Fourth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 0, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 1, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 2, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 3, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 4, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 5, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 6, SheetPosY = 3 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 7, SheetPosY = 3 });

            // Fifth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 4 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 4 });

            // Sixth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 5 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 5 });

            // Seventh row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 6 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 6 });

            // Eighth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 7 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 7 });

            // Nineth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 0, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 1, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 4, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 5, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 6, SheetPosY = 8 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 7, SheetPosY = 8 });

            // Tenth row of sprites in the sprite sheet.
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 0, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 1, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 2, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = true, SheetPosX = 3, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 4, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 5, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 6, SheetPosY = 9 });
            m_TileList.Add(new Tile() { IsSolid = false, SheetPosX = 7, SheetPosY = 9 });


            // Create our level.
            m_Map = new int[,] { { 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14 },
                                   { 14, 10,  0,  3,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  8, 53,  0,  0,  0,  0,  0,  3,  6, 25, 24,  6, 14 },
                                   { 14,  0, 12, 12, 12, 12, 12,  2,  0,  0,  0,  0,  0,  0,  9,  0,  0,  0,  0,  0, 48,  1,  0,  0,  0, 10,  0,  6, 20, 21,  6, 14 },
                                   { 14,  0, 12,  7,  7,  7, 12,  0,  0,  0,  8,  0,  0,  0,  0,  4,  0,  0,  0,  0, 48,  0, 10,  0,  0,  0,  0,  6, 20, 21,  6, 14 },
                                   { 14,  3, 12,  7,  7,  7, 12,  0,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6, 48,  6,  6,  6,  6,  6,  6,  6, 20, 21,  6, 14 },
                                   { 14,  0, 12,  7,  7,  7, 12,  0,  6, 25, 18, 18, 18, 18, 18, 18, 18, 18, 18, 18, 19, 18, 18, 18, 18, 18, 18, 18 ,22, 23, 24, 14 },
                                   { 14,  0, 12, 12, 15, 12, 12,  9,  6, 17, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 27, 26, 26, 26, 26, 26, 26, 26, 30, 31, 16, 14 },
                                   { 14,  0,  5, 11,  0,  0,  8,  0,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6, 48,  6,  6,  6,  6,  6,  6,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0,  0,  0,  0,  0,  0,  0, 72,  0,  0,  0,  0,  0,  0, 56, 49, 57,  3, 48,  0, 10,  0,  0, 10,  0,  6, 20, 21,  6, 14 },
                                   { 14,  9,  0,  0,  0,  0,  0,  1, 75, 72, 74,  1,  0,  0,  2,  0, 48,  9, 63, 49, 50, 55, 13, 13, 13, 13, 13,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 13, 13, 13, 13, 13, 73, 13, 13, 13, 13,  0,  0, 58, 49, 59,  4, 48,  0, 13,  7,  7,  7, 13,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 73, 73, 73, 73, 13, 73, 73, 79, 73, 13,  0,  0,  0,  0,  0,  0, 48,  8, 13,  7,  7,  7, 13,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 13, 13, 76, 13, 13, 13, 13, 13, 78, 13,  0,  9,  0,  0,  0,  0, 48,  0, 13,  7,  7,  7, 13,  6, 20, 21,  6, 14 },
                                   { 14,  0,  8, 13, 73, 73, 73, 73, 73, 73, 13, 13, 78, 13,  0,  0,  0,  0,  0, 10, 48,  8, 13, 13, 15, 13, 13,  6, 20, 21,  6, 14 },
                                   { 14,  2,  0, 13, 73, 73, 73, 73, 73, 73, 77, 77, 73, 13,  5,  0,  0,  0,  0,  0, 48,  0,  0,  0, 72,  0, 11,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 73, 73, 73, 73, 73, 13, 13, 13, 13, 13,  0,  0,  0,  8,  0,  0, 48,  0,  0,  9, 72,  9,  0,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 73, 73, 73, 73, 73, 13,  0, 10,  3, 10,  0,  0,  0,  0,  1, 32, 38, 33,  4,  0, 72,  0,  0,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 13, 13, 78, 13, 13, 13,  3, 56, 49, 49, 57,  0,  0,  0, 32, 36, 51, 37, 35, 35, 35, 33,  2,  6, 20, 21,  6, 14 },
                                   { 14,  9,  0, 13, 73, 73, 73, 73, 73, 13, 10, 48,  9,  0, 54,  0,  0, 32, 36, 51, 51, 51, 45, 42, 44, 37, 33,  6, 20, 21,  6, 14 },
                                   { 14,  0,  0, 13, 13, 13, 13, 13, 13, 13, 32, 38, 33,  2,  0, 56, 49, 47, 51, 51, 51, 51, 43,  5, 34, 51, 37, 35, 28, 29, 35, 14 },
                                   { 14,  0,  0,  0,  0,  0,  0,  0,  0, 52, 47, 51, 39, 49, 60, 59,  8, 40, 44, 51, 51, 51, 37, 35, 36, 51, 51, 51, 28, 29, 51, 14 },
                                   { 14,  0,  0,  8,  0,  0,  8,  0,  4,  0, 40, 44, 43,  1, 48,  0,  3,  0, 40, 44, 51, 51, 51, 51, 51, 51, 45, 42, 28, 29, 42, 14 },
                                   { 14,  4,  0,  0,  0,  0,  0,  0,  0,  0,  2, 40, 68, 49, 62, 49, 55,  2,  0, 40, 42, 42, 42, 42, 42, 42, 41,  6, 17, 16,  6, 14 },
                                   { 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14, 14 }};
        }

        /// <summary>
        /// This method loads a bitmap file into memory for us.
        /// </summary>
        /// <param name="filename"></param>
        public SlimDXBitmap LoadBitmap(string filename)
        {
            // This will hold the Direct2D Bitmap that we will return at the end of this function.
            SlimDXBitmap d2dBitmap = null;


            // Load the bitmap using the System.Drawing.Bitmap class.
            SystemBitmap originalImage = new SystemBitmap(filename);

            // Create a rectangle holding the size of the bitmap image.
            Rectangle bounds = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

            // Lock the memory holding this bitmap so that only we are allowed to mess with it.
            BitmapData imageData = originalImage.LockBits(bounds, ImageLockMode.ReadOnly, SystemPixelFormat.Format32bppPArgb);

            // Create a DataStream attached to the bitmap.
            DataStream dataStream = new DataStream(imageData.Scan0,
                                                          imageData.Stride * imageData.Height,
                                                          true,
                                                          false);

            // Set the pixel format and properties.
            BitmapProperties bmpProperties = new BitmapProperties
            {
                PixelFormat = new SlimDXPixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)
            };


            // Copy the image data into a new SlimDX.Direct2D.Bitmap object.
            d2dBitmap = new SlimDXBitmap(
                this.m_RenderTarget, 
                new Size(bounds.Width, bounds.Height),
                dataStream,
                imageData.Stride,
                bmpProperties);

            // Unlock the memory that is holding the original bitmap object.
            originalImage.UnlockBits(imageData);


            // Get rid of the original bitmap object since we no longer need it.
            originalImage.Dispose();


            // Return the Direct2D bitmap.
            return d2dBitmap;
        }

        /// <summary>
        /// This method renders a debug display.  When this method is called, it draws a yellow border on every tile
        /// that is solid (unwalkable).
        /// </summary>
        public void RenderDebug()
        {
            Tile s;

            // Loop through the y axis.
            for (int y = 0; y < m_Map.GetLength(0); y++)
            {
                // Loop through the x axis.
                for (int x = 0; x < m_Map.GetLength(1); x++)
                {
                    // Get the tile at the current coordinates.
                    s = m_TileList[m_Map[y, x]];

                    // Check if the tile is solid.  If so, draw a yellow border on it.
                    if (s.IsSolid)
                        m_RenderTarget.DrawRectangle(m_DebugBrush, new Rectangle(x * 32, y * 32, 32, 32));

                }
            }
        }



        /// <summary>
        /// This method renders the player character.  This method MUST be called after RenderWorld() or the world would be drawn on top of the player
        /// character, causing the player character to be effectively invisible.
        /// </summary>
        public void RenderPlayer()
        {
            // Render the player character.
            m_RenderTarget.DrawBitmap(m_PlayerSprites,
                                      new Rectangle((int)(m_Player.PositionX * 32), (int)(m_Player.PositionY * 32), 32, 32),
                                      1.0f,
                                      InterpolationMode.Linear,
                                      new Rectangle(m_Player.AnimFrame * 32, 0, 32, 32));

        }

        /// <summary>
        /// This method renders our 2D world.
        /// </summary>
        public void RenderWorld()
        {
            Tile s;

            // Loop through the y axis.
            for (int y = 0; y < m_Map.GetLength(0); y++)
            {
                // Loop through the x axis.
                for (int x = 0; x < m_Map.GetLength(1); x++)
                {
                    // Get the tile at the current coordinates.
                    s = m_TileList[m_Map[y, x]];

                    // Render the tile.
                    m_RenderTarget.DrawBitmap(m_TileSheet,
                                              new Rectangle(x * 32, y * 32, 32, 32),
                                              1.0f,
                                              InterpolationMode.Linear,
                                              new Rectangle(s.SheetPosX * 32, s.SheetPosY * 32, 32, 32));
                }
            }
        }

        /// <summary>
        /// This method is called once per frame and is where we update objects in our scene.
        /// <param name="frameTime">The amount of time (in seconds) that has elapsed since the previous update.</param>
        /// </summary>
        public override void UpdateScene(double frameTime)
        {
            base.UpdateScene(frameTime);


            // Figure out which grid square each corner of the player sprite is currently in.
            PointF TL = new PointF(m_Player.PositionX + 0.25f, m_Player.PositionY + 0.25f); // Top left corner
            PointF BL = new PointF(m_Player.PositionX + 0.25f, m_Player.PositionY + 0.75f); // Bottom left corner
            PointF TR = new PointF(m_Player.PositionX + 0.75f, m_Player.PositionY + 0.25f); // Top right corner
            PointF BR = new PointF(m_Player.PositionX + 0.75f, m_Player.PositionY + 0.75f); // Bottom right corner




            // DIRECTINPUT KEYBOARD CODE
            // =============================================================================================================================================

            // Check if the user is pressing left.
            if (UserInput.IsKeyPressed(Key.A) ||         // Is the player pressing left?
                (UserInput.IsKeyPressed(Key.LeftArrow)))
            {
                // Check if there is a solid tile in the way of the upper left or lower left corner of the player character's bounding box.
                if ((!m_TileList[m_Map[(int)TL.Y, (int)(TL.X - PLAYER_MOVE_SPEED)]].IsSolid) &&
                    (!m_TileList[m_Map[(int)BL.Y, (int)(BL.X - PLAYER_MOVE_SPEED)]].IsSolid))
                {
                    // No collision detected, so move the player character.
                    m_Player.PositionX -= PLAYER_MOVE_SPEED;
                }

            }


            // Check if the user is pressing right.
            else if (UserInput.IsKeyPressed(Key.D) ||         // Is the player pressing right?
                (UserInput.IsKeyPressed(Key.RightArrow)))
            {
                // Check if there is a solid tile in the way of the upper right or lower right corner of the player character's bounding box.
                if ((!m_TileList[m_Map[(int)TR.Y, (int)(TR.X + PLAYER_MOVE_SPEED)]].IsSolid) &&
                    (!m_TileList[m_Map[(int)BR.Y, (int)(BR.X + PLAYER_MOVE_SPEED)]].IsSolid))
                {
                    // No collision detected, so move the player character.
                    m_Player.PositionX += PLAYER_MOVE_SPEED;
                }

            }

            // Check if the user is pressing up.
            else if (UserInput.IsKeyPressed(Key.W) ||         // Is the player pressing up?
                (UserInput.IsKeyPressed(Key.UpArrow)))
            {
                // Check if there is a solid tile in the way of the upper left or upper right corner of the player character's bounding box.
                if ((!m_TileList[m_Map[(int)(TL.Y - PLAYER_MOVE_SPEED), (int)TL.X]].IsSolid) &&
                    (!m_TileList[m_Map[(int)(TR.Y - PLAYER_MOVE_SPEED), (int)TR.X]].IsSolid))
                {
                    // No collision detected, so move the player character.
                    m_Player.PositionY -= PLAYER_MOVE_SPEED;
                }

            }

            // Check if the user is pressing down.
            else if (UserInput.IsKeyPressed(Key.D) ||         // Is the player pressing down?
                (UserInput.IsKeyPressed(Key.DownArrow)))
            {
                // Check if there is a solid tile in the way of the lower left or lower right corner of the player character's bounding box.
                if ((!m_TileList[m_Map[(int)(BL.Y + PLAYER_MOVE_SPEED), (int)BL.X]].IsSolid) &&
                    (!m_TileList[m_Map[(int)(BR.Y + PLAYER_MOVE_SPEED), (int)BR.X]].IsSolid))
                {
                    // No collision detected, so move the player character.
                    m_Player.PositionY += PLAYER_MOVE_SPEED;
                }

            }

            // Animate the player character.
            m_Player.LastFrameChange += frameTime; // Add the frametime to the LastFrameChange variable so we know how long its been since the last time we changed the player's animation frame.
            if (m_Player.LastFrameChange > 0.1) // Change the player's animation frame once every 0.1 seconds.
            {
                m_Player.LastFrameChange = 0; // Reset this variable to zero so we will know when it is time to change the player character's animation frame again.

                m_Player.AnimFrame++; // Increment the frame counter.
                if (m_Player.AnimFrame > 7)                 // If we are passed the last frame, reset it to the first frame.
                    m_Player.AnimFrame = 0;
            }

            // This line of code will show the player's x,y coords in the Visual Studio Output Pane if you uncomment it.
            //System.Diagnostics.Debug.WriteLine("PLAYER: (" + m_Player.PositionX.ToString() + ", " + m_Player.PositionY.ToString() + ")");
        }

        /// <summary>
        /// This function is called each time we need to render the next frame.
        /// </summary>
        public override void RenderScene()
        {
            base.RenderScene();

            // Tell the render target we are ready to begin drawing.
            m_RenderTarget.BeginDraw();

            // Clear the screen.
            m_RenderTarget.Clear(ClearColor);


            // Render the world.
            RenderWorld();

            // Render debug display.  If you uncomment this, all solid (unwalkable) tiles will have a yellow border on them.
            //RenderDebug();

            // Render the player character.
            RenderPlayer();


            // Tell the render target that we are done drawing.
            m_RenderTarget.EndDraw();
        }

        /// <summary>
        /// This is the GameWindow class's internal Dispose() method that actually disposes of the game window class.
        /// </summary>
        /// <param name="disposing">If this method is being called by this class's own code, then the value true should be passed into this parameter to indicate that this method was not called by the Garbage Collector.</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                /*
                * The following text is from MSDN  (http://msdn.microsoft.com/en-us/library/fs2xkftw%28VS.80%29.aspx)
                * 
                * 
                * Dispose(bool disposing) executes in two distinct scenarios:
                * 
                * If disposing equals true, the method has been called directly or indirectly by a user's code and managed and unmanaged resources can be disposed.
                * If disposing equals false, the method has been called by the runtime from inside the finalizer and only unmanaged resources can be disposed. 
                * 
                * When an object is executing its finalization code, it should not reference other objects, because finalizers do not execute in any particular order. 
                * If an executing finalizer references another object that has already been finalized, the executing finalizer will fail.
                */
                if (disposing)
                {
                    // Unregister events


                    // get rid of managed resources
                    if (m_RenderTarget != null)
                        m_RenderTarget.Dispose();

                    if (m_Factory != null)
                        m_Factory.Dispose();

                    if (m_TileSheet != null)
                        m_TileSheet.Dispose();

                    if (m_PlayerSprites != null)
                        m_PlayerSprites.Dispose();

                    if (m_DebugBrush != null)
                        m_DebugBrush.Dispose();
                }


                // get rid of unmanaged resources

            }


            base.Dispose(disposing);

        }
    }
}

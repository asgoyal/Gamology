using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System;
using SlimDX;
using SlimDX.Direct2D;
using SlimDX.Windows;

namespace Gamology.System
{
    class GameWindow2D : GameWindow, IDisposable
    {
        # region member variables
        private WindowRenderTarget m_RenderTarget;
        private Factory m_Factory;
        private PathGeometry m_Geometry;
        private SolidColorBrush m_BrushRed;
        private SolidColorBrush m_BrushGreen;
        private SolidColorBrush m_BrushBlue;
        # endregion

        public GameWindow2D(string title, int width, int height, bool fullscreen) 
            : base(title, width, height, fullscreen)
        {
            this.m_Factory = new Factory();

            this.m_RenderTarget = new WindowRenderTarget(this.m_Factory,
                new WindowRenderTargetProperties
                {
                    Handle = FormObject.Handle,
                    PixelSize = new Size(width, height)
                });

            this.m_BrushRed = new SolidColorBrush(m_RenderTarget, new Color4(1.0f, 1.0f, 0.0f, 0.0f));
            this.m_BrushGreen = new SolidColorBrush(m_RenderTarget, new Color4(1.0f, 0.0f, 1.0f, 0.0f));
            this.m_BrushBlue = new SolidColorBrush(m_RenderTarget, new Color4(1.0f, 0.0f, 0.0f, 1.0f));

            this.initGeometry();
        }

        private void initGeometry()
        {
            m_Geometry = new PathGeometry(m_RenderTarget.Factory);
            using (GeometrySink sink = m_Geometry.Open())
            {
                int top = (int)(0.25f * FormObject.Height);
                int left = (int)(0.25f * FormObject.Width);
                int right = (int)(0.75f * FormObject.Width);
                int bottom = (int)(0.75f * FormObject.Height);
                PointF p0 = new Point(left, top);
                PointF p1 = new Point(right, top);
                PointF p2 = new Point(right, bottom);
                PointF p3 = new Point(left, bottom);

                sink.BeginFigure(p0, FigureBegin.Filled);
                sink.AddLine(p1);
                sink.AddLine(p2);
                sink.AddLine(p3);
                sink.EndFigure(FigureEnd.Closed);
                sink.Close();
            }
        }

        public override void UpdateScene(double elapsedFrameTime)
        {
            base.UpdateScene(elapsedFrameTime);
        }

        public override void RenderScene()
        {
            base.RenderScene();

            this.m_RenderTarget.BeginDraw();
            this.m_RenderTarget.Clear(ClearColor);
            this.m_RenderTarget.FillGeometry(m_Geometry, this.m_BrushBlue);
            this.m_RenderTarget.DrawGeometry(m_Geometry, this.m_BrushRed, 1.0f);
            this.m_RenderTarget.EndDraw();
        }

        protected override void Dispose(bool disposing)
        {
            if (this.IsDisposed)
            {
                return;
            }

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
                if (m_BrushRed != null)
                    m_BrushRed.Dispose();

                if (m_BrushGreen != null)
                    m_BrushGreen.Dispose();

                if (m_BrushBlue != null)
                    m_BrushBlue.Dispose();

                if (m_Geometry != null)
                    m_Geometry.Dispose();

                if (m_RenderTarget != null)
                    m_RenderTarget.Dispose();

                if (m_Factory != null)
                    m_Factory.Dispose();
            }

            // get rid of unmanaged resources

            base.Dispose(disposing);
        }
    }
}

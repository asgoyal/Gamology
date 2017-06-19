using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFCanvas = System.Windows.Controls.Canvas;

namespace Gamology.UserInterface.UserControls.Canvas
{
    public class EditableCanvas : WPFCanvas
    {
        public EditableCanvas()
        {
            Loaded += OnCanvasLoaded;
        }

        private CanvasViewModel GetModel()
        {
            var canvasViewModel = this.DataContext as CanvasViewModel;
            if (canvasViewModel == null)
            {
                throw new InvalidOperationException("No Data context associated.");
            }
            return canvasViewModel;
        } 

        protected override void OnRender(DrawingContext dc)
        {
            foreach(var sprite in GetModel().Sprites)
            {
                BitmapImage img = new BitmapImage(new Uri(sprite.TextureFilename));
                dc.DrawImage(img, new Rect(sprite.PositionX, sprite.PositionY, img.PixelWidth, img.PixelHeight));
            }
        }

        private void OnCanvasLoaded(object sender, RoutedEventArgs e)
        {
            GetModel().OnRefresh += OnRefresh;
        }

        private void OnRefresh()
        {
            this.InvalidateVisual();
        }
    }
}

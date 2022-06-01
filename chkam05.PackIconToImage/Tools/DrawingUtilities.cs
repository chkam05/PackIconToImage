using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chkam05.PackIconToImage.Tools
{
    public static class DrawingUtilities
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Draw image from interface control. </summary>
        /// <param name="control"> Interface control. </param>
        /// <param name="width"> Image width. </param>
        /// <param name="height"> Image height. </param>
        /// <param name="background"> Background brush. </param>
        /// <returns> Image source of drawed interface control. </returns>
        public static ImageSource DrawImageFromControl(Control control, double width, double height, Brush background)
        {
            Grid drawingGrid = new Grid()
            {
                Background = background,
                Height = height,
                Width = width
            };

            var size = new Size(width, height);

            drawingGrid.Children.Add(control);
            drawingGrid.Measure(size);
            drawingGrid.Arrange(new Rect(size));
            drawingGrid.UpdateLayout();

            var renderBitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Default);
            renderBitmap.Render(drawingGrid);

            var bitmap = BitmapFrame.Create(renderBitmap);
            bitmap.Freeze();
            return bitmap;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Draw PackIcon control with specified PackIconKind. </summary>
        /// <param name="packIconKind"> Pack icon kind. </param>
        /// <param name="width"> Image width. </param>
        /// <param name="height"> Image height. </param>
        /// <param name="background"> Background brush. </param>
        /// <param name="foreground"> Foreground brush. </param>
        /// <returns> Image source of drawed pack icon. </returns>
        public static ImageSource DrawImageFromPackIconKind(PackIconKind packIconKind, double width, double height, Brush background, Brush foreground)
        {
            PackIcon packIcon = new PackIcon()
            {
                Foreground = foreground,
                Kind = packIconKind,
                Height = height,
                Width = width
            };

            return DrawImageFromControl(packIcon, width, height, background);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save image source to file. </summary>
        /// <param name="imageSource"> Image source. </param>
        /// <param name="encoder"> Image type encoder. </param>
        /// <param name="filePath"> Save file path. </param>
        public static void SaveImageSource(ImageSource imageSource, BitmapEncoder encoder, string filePath)
        {
            var frame = imageSource as BitmapFrame;

            if (frame != null)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    encoder.Frames.Add(frame);
                    encoder.Save(fileStream);

                    fileStream.Flush();
                    fileStream.Close();
                }
            }
        }

    }
}

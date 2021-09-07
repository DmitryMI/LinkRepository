using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace LinkRepository.Utils
{
    public static class ImageUtils
    {
        public static byte[] ImageToBytes(Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] xByte = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            return xByte;
        }

        public static Image ImageFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null)
            {
                return null;
            }

            if (imageBytes.Length == 0)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream(imageBytes);
            Image image = Image.FromStream(ms);
            ms.Close();
            ms.Dispose();
            return image;
        }        

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            if (image == null)
            {
                return null;
            }
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}

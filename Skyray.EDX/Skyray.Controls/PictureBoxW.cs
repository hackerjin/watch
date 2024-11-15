﻿
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Skyray.Controls
{
    /// <summary>
    /// Picture box control for displaying an image.
    /// </summary>
    /// 
    /// <remarks><para>This control is inherited from System.Windows.Forms.PictureBox and is
    /// aimed to resolve one of its issues - inability to display images with high color depth,
    /// like 16 bpp grayscale, 48 bpp and 64 bpp color images. .NET framework does not handle
    /// 16 bpp grayscale images at all, throwing exception when user tries to display them. Color
    /// images with 48 bpp and 64 bpp are "kind of" supported, but only maximum of 13 bits for each
    /// color plane are allowed. Therefore this control is created, which allows to display as
    /// 16 bpp grayscale images, as 48 bpp and 64 bpp color images.</para>
    /// 
    /// <para><note>To display high color depth images, the control does internal conversion of them
    /// to lower color depth images - 8 bpp grayscale, 24 bpp and 32 bpp color images respectively. In
    /// the case source image already has low color depth, it is displayed without any conversions.
    /// </note></para>
    /// </remarks>
    /// 
    public class PictureBoxW : PictureBox
    {
        private Image sourceImage = null;
        private Image convertedImage = null;

        /// <summary>
        /// Gets or sets the image that the PictureBox displays.
        /// </summary>
        /// 
        /// <remarks><para>The property is used to set image to be displayed or to get currently
        /// displayed image.</para>
        /// 
        /// <para><note>In the case if source image has high color depth, like 16 bpp grayscale image,
        /// 48 bpp or 64 bpp color image, it is converted to lower color depth before displaying -
        /// to 8 bpp grayscale, 24 bpp or 32 bpp color image respectively.</note></para>
        /// 
        /// <para><note>During color conversion the original source image is kept unmodified, but internal
        /// converted copy is created. The property always returns original source image.</note></para>
        /// </remarks>
        /// 
        public new Image Image
        {
            get { return sourceImage; }
            set
            {
                // check source image format
                if (
                    (value != null) && (value is Bitmap) &&
                    ((value.PixelFormat == PixelFormat.Format16bppGrayScale) ||
                      (value.PixelFormat == PixelFormat.Format48bppRgb) ||
                      (value.PixelFormat == PixelFormat.Format64bppArgb)))
                {
                    // convert and display image
                    Image tempImage = ConvertImage((Bitmap)value);
                    base.Image = tempImage;

                    // dispose previous image if required
                    if (convertedImage != null)
                    {
                        convertedImage.Dispose();
                    }

                    convertedImage = tempImage;
                }
                else
                {
                    // display source image as it is
                    base.Image = value;
                }
                sourceImage = value;
            }
        }

        // Convert high color depth image into lower color depth image
        private Bitmap ConvertImage(Bitmap source)
        {
            Bitmap newImage = null;
            int layers = 0;

            // get image size
            int width = source.Width;
            int height = source.Height;

            // create new image depending on source image format
            switch (source.PixelFormat)
            {
                case PixelFormat.Format16bppGrayScale:
                    {
                        // create new grayscale image
                        newImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                        // get palette
                        ColorPalette cp = newImage.Palette;
                        // init palette with grayscale colors
                        for (int i = 0; i < 256; i++)
                        {
                            cp.Entries[i] = Color.FromArgb(i, i, i);
                        }
                        // set palette back
                        newImage.Palette = cp;

                        layers = 1;
                        break;
                    }

                case PixelFormat.Format48bppRgb:
                    {
                        // create new color 24 bpp image
                        newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                        layers = 3;
                        break;
                    }

                case PixelFormat.Format64bppArgb:
                    {
                        // create new color 32 bpp image
                        newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                        layers = 4;
                        break;
                    }

                default:
                    return null;
            }

            // lock both images
            BitmapData sourceData = source.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, source.PixelFormat);
            BitmapData newData = newImage.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, newImage.PixelFormat);

            unsafe
            {
                // base pointers
                int sourceBasePtr = (int)sourceData.Scan0.ToPointer();
                int newBasePtr = (int)newData.Scan0.ToPointer();
                // image strides
                int sourceStride = sourceData.Stride;
                int newStride = newData.Stride;

                for (int y = 0; y < height; y++)
                {
                    ushort* sourcePtr = (ushort*)(sourceBasePtr + y * sourceStride);
                    byte* newPtr = (byte*)(newBasePtr + y * newStride);

                    for (int x = 0, lineSize = width * layers; x < lineSize; x++, sourcePtr++, newPtr++)
                    {
                        *newPtr = (byte)(*sourcePtr >> 8);
                    }
                }
            }

            // unlock both image
            source.UnlockBits(sourceData);
            newImage.UnlockBits(newData);

            return newImage;
        }
    }
}

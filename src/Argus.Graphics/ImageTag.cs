using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using GraphicsFx = System.Drawing.Graphics;

namespace Argus.Graphics
{
    /// <summary>
    ///     This class can be used to place text on an image, or tag it.  You can specify the text
    ///     color, a background color (transparent by default) that a colored rectangle underneath
    ///     should be.  You can use the auto color feature to let the class try to pick the best
    ///     foreground color or use the auto multi color feature to try to determine the best color
    ///     for each character (white or black based on the brightness of the top left pixel of the
    ///     region to be rendered... auto color multi renders character by character whereas the other
    ///     other color types render the whole text block at once).  The class can either tag an image
    ///     file on disk or an in memory Bitmap object.
    /// </summary>
    public class ImageTag
    {
        //*********************************************************************************************************************
        //
        //             Class:  ImageTag
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  11/20/2007
        //      Last Updated:  11/21/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     List of the available color options
        /// </summary>
        public enum ColorOptions
        {
            SingleColorSolid,
            AutoColor,
            AutoMultiColor
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public ImageTag()
        {
            this.Font = new Font("Verdana", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            this.ForeGroundColor = Color.Black;
            this.BackGroundColor = Color.Transparent;
        }

        /// <summary>
        ///     The text you want to display on the picture.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     This is the name of the file that you want to read in as your source file.  The will be read into memory
        ///     and the contents of this file will never be overwritten unless specified by you when you call the Save method.
        /// </summary>
        public string SourceFileName { get; set; }

        /// <summary>
        ///     The font to be used to on the image tag.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        ///     The foreground color to use.  This is what will be used if the default color type (SingleColorSolid)
        ///     is selected.  If the color type is set to AutoColor or AutoMultiColor then this property will be ignored.
        /// </summary>
        public Color ForeGroundColor { get; set; }

        /// <summary>
        ///     This is the background color to use underneath the text.  It is set to transparant as default.  Specifing any other color
        ///     will place that colored rectangle underneath your text.
        /// </summary>
        public Color BackGroundColor { get; set; }

        /// <summary>
        ///     The X coordinate for the top left corner of where the text should be rendered.
        /// </summary>
        public int X { get; set; } = 1;

        /// <summary>
        ///     The Y coordinate for the top left corner of where the text should be rendered.
        /// </summary>
        public int Y { get; set; } = 1;

        /// <summary>
        ///     The color type specifies how you want your foreground color to display.  There are three options.
        ///     1.)  SingleColorSolid:  This is the default option.  The text will be one color that is the color specified in the ForegroundColor property.
        ///     2.)  AutoColor:  The text will be one color solid black or white that is determined by looking at the upper left hand pixel's brightness.
        ///     3.)  AutoMultiColor:  Text text will poetentially be multiple colors for each character based on the brightness of the upper left hand pixel of that character's position on the image.
        /// </summary>
        public ColorOptions ColorType { get; set; }

        /// <summary>
        ///     Saves the image to the destination passed in via the Filename variable.  All rendering of the picture
        ///     occurs in this function as well.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            var image = new Bitmap(this.SourceFileName);
            this.Save(image);
            image.Save(filename, ImageFormat.Jpeg);
            image.Dispose();
            image = null;
        }

        /// <summary>
        ///     Tags the Bitmap passed in to the method.
        /// </summary>
        /// <param name="image"></param>
        public void Save(Bitmap image)
        {
            var gr = GraphicsFx.FromImage(image);
            int currentX = this.X;
            int currentY = this.Y;
            var strFormat = new StringFormat(StringFormat.GenericTypographic);
            strFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            switch (this.ColorType)
            {
                case ColorOptions.AutoColor:
                    float brightness = image.GetPixel(this.X, this.Y).GetBrightness();

                    if (brightness > 0.5)
                    {
                        this.ForeGroundColor = Color.Black;
                    }
                    else
                    {
                        this.ForeGroundColor = Color.White;
                    }

                    break;
            }

            var brush = new SolidBrush(this.ForeGroundColor);
            var rectSize = gr.MeasureString(this.Text, this.Font);
            var backgroundBrush = new SolidBrush(this.BackGroundColor);
            gr.FillRectangle(backgroundBrush, this.X, this.Y, rectSize.Width, rectSize.Height);
            gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            if (this.ColorType == ColorOptions.AutoMultiColor)
            {
                for (int counter = 0; counter <= this.Text.Length - 1; counter++)
                {
                    float brightness = 0;

                    try
                    {
                        brightness = image.GetPixel(currentX, currentY).GetBrightness();
                    }
                    catch
                    {
                    }

                    if (brightness > 0.5)
                    {
                        brush.Color = Color.Black;
                    }
                    else
                    {
                        brush.Color = Color.White;
                    }

                    gr.DrawString(this.Text.Substring(counter, 1), this.Font, brush, currentX, currentY);
                    gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    int tmp1 = 0;
                    int tmp2 = 0;

                    var size = gr.MeasureString(this.Text.Substring(counter, 1), this.Font, new SizeF(0, 0), strFormat, out tmp1, out tmp2);
                    currentX += Convert.ToInt32(size.Width);
                }
            }
            else
            {
                gr.DrawString(this.Text, this.Font, brush, this.X, this.Y);
            }

            backgroundBrush.Dispose();
            brush.Dispose();
            brush = null;
        }
    }
}
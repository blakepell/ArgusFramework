using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Argus.Graphics
{

    /// <summary>
    /// A simple captcha implementation that can be used 
    /// </summary>
    /// <remarks></remarks>
    public class Captcha : IDisposable
    {
        //*********************************************************************************************************************
        //
        //             Class:  Captcha
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  04/12/2008
        //      Last Updated:  11/15/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>

        public Captcha()
        {
            this.Font = new Font("Courier New", FontSize + _random.Next(14, 18), FontStyle.Bold);
        }

        /// <summary>
        /// Creates the skewed image with text and puts it into a memory stream which can be written out
        /// to display as the caller requires. 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns>The text that was displayed on the image that was placed into the MemoryStream.</returns>
        public string CreateImage(MemoryStream memoryStream)
        {
            // The will set the Text property to a random set of text that will fit in our Image.  It's
            // a property so the Caller will have the ability to get it's value out for validation.
            string text = GetRandomText();

            using (var bitmap = new Bitmap(200, 50, PixelFormat.Format32bppArgb))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    var rect = new Rectangle(0, 0, 200, 70);
                    int counter = 0;

                    g.FillRectangle(Brushes.DarkKhaki, rect);

                    for (int i = 0; i <= text.Length - 1; i++)
                    {
                        g.DrawString(text[i].ToString(), Font, GetRandomBrush(), new PointF(10 + counter, 10));
                        counter += 20;
                    }

                    DrawRandomLines(g);
                    bitmap.Save(memoryStream, ImageFormat.Jpeg);
                }
            }

            return text;
        }

        /// <summary>
        /// Draws random lines via a graphics object.
        /// </summary>
        /// <param name="g"></param>
        private void DrawRandomLines(System.Drawing.Graphics g)
        {
            for (int i = 0; i <= 10; i++)
            {
                g.DrawLine(new Pen(Color.Gray, 1), GetRandomPoint(), GetRandomPoint2());
            }
        }

        /// <summary>
        /// Gets a random point within the top half of the image boundaries
        /// </summary>
        private Point GetRandomPoint()
        {
            return new Point(_random.Next(0, 100), _random.Next(1, 25));
        }

        /// <summary>
        /// Gets a random point within the bottom half of the image boundaries
        /// </summary>
        private Point GetRandomPoint2()
        {
            return new Point(_random.Next(101, 200), _random.Next(26, 50));
        }

        /// <summary>
        /// Gets a random brush color
        /// </summary>
        private Brush GetRandomBrush()
        {
            switch (_random.Next(1, 5))
            {
                case 1:
                    return Brushes.Blue;
                case 2:
                    return Brushes.Black;
                case 3:
                    return Brushes.Red;
                case 4:
                    return Brushes.Green;
                case 5:
                    return Brushes.Maroon;
                default:
                    return Brushes.White;
            }
        }

        /// <summary>
        /// Gets random text.
        /// </summary>
        private string GetRandomText()
        {
            var randomText = new StringBuilder();
            string characterSet = "abcdefghijklmnopqrstuvwxyz23456789";

            for (int counter = 0; counter <= 7; counter++)
            {
                randomText.Append(characterSet[_random.Next(characterSet.Length)]);
            }

            return randomText.ToString();
        }

        private bool _disposed = false;

        /// <summary>
        /// Disposes of any used resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.Font != null)
                {
                    this.Font.Dispose();
                    this.Font = null;
                }
            }

            _disposed = true;
        }

        /// <summary>
        /// Random number generator.
        /// </summary>
        private Random _random = new Random();

        /// <summary>
        /// The font size to use on the image.
        /// </summary>
        /// <remarks>The default font size is 10</remarks>
        public int FontSize { get; set; } = 10;

        /// <summary>
        /// The font to use on the image.
        /// </summary>
        /// <remarks>The default font is Courier New.</remarks>
        public Font Font { get; set; }

    }
}
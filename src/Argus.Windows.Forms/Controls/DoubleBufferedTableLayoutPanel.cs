/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-07-25
 * @last updated      : 2019-03-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Windows.Forms;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    /// A TableLayoutPanel that is double buffered for rendering performance.
    /// </summary>
    public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DoubleBufferedTableLayoutPanel()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.UpdateStyles();
        }
    }
}
using System.Windows.Forms;

namespace Argus.Windows.Forms.Controls
{
    /// <summary>
    /// A TableLayoutPanel that is double buffered for rendering performance.
    /// </summary>
    public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
    {
        //*********************************************************************************************************************
        //
        //             Class:  DoubleBufferedTableLayout
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  07/25/2009
        //      Last Updated:  03/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************


        public DoubleBufferedTableLayoutPanel()
        {           
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            UpdateStyles();
        }
    }
}
namespace Argus.Windows.Uwp.Models
{
    /// <summary>
    ///     A cursor position for use with text editing controls.  Holds the data for the row, column and absolute position.
    /// </summary>
    public class CursorPosition
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public CursorPosition()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="row">The current row the cursor is on in the text box.</param>
        /// <param name="column">The current column the cursor is on.</param>
        /// <param name="position">The absolute position of where the cursor is within the entirety of the string.</param>
        public CursorPosition(int row, int column, int position)
        {
            this.Row = row;
            this.Column = column;
            this.Position = position;
        }

        /// <summary>
        ///     The current row the cursor is on in the text box.
        /// </summary>
        public int Row { get; set; } = 1;

        /// <summary>
        ///     The current column the cursor is on.
        /// </summary>
        public int Column { get; set; } = 1;

        /// <summary>
        ///     The absolute position of where the cursor is within the entirety of the string.
        /// </summary>
        public int Position { get; set; } = 1;
    }
}
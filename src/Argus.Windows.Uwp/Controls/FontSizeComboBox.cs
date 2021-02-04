/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using Windows.UI.Xaml.Controls;

namespace Argus.Windows.Uwp.Controls
{
    public class FontSizeComboBox : ComboBox
    {
        public FontSizeComboBox()
        {
            this.Items.Add(8);
            this.Items.Add(9);
            this.Items.Add(10);
            this.Items.Add(11);
            this.Items.Add(12);
            this.Items.Add(14);
            this.Items.Add(16);
            this.Items.Add(18);
            this.Items.Add(20);
            this.Items.Add(22);
            this.Items.Add(24);
            this.Items.Add(26);
            this.Items.Add(28);
            this.Items.Add(36);
            this.Items.Add(48);
            this.Items.Add(72);
        }
    }
}
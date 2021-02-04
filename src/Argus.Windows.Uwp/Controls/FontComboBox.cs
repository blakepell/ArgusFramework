/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Text;

namespace Argus.Windows.Uwp.Controls
{
    public class FontComboBox : ComboBox
    {
        private bool _showAsFont = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public FontComboBox()
        {
            // Initialize our font list
            this.Fonts = new List<string>();

            this.SetupItemTemplate();

            // Put the string array into the ItemsSource
            this.ItemsSource = CanvasTextFormat.GetSystemFontFamilies();

            this.Loaded += this.FontComboBox_Loaded;
        }

        /// <summary>
        /// Gets the FontFamily for the SelectedItem
        /// </summary>
        public FontFamily SelectedFontFamily => new FontFamily((string) this.SelectedValue);

        /// <summary>
        /// The list of the fonts currently on the system.
        /// </summary>
        public List<string> Fonts { get; set; }

        /// <summary>
        /// Whether or not the text in the ComboBox should be displayed as the font that it is.
        /// </summary>
        public bool ShowAsFont
        {
            get => _showAsFont;
            set
            {
                _showAsFont = value;
                this.SetupItemTemplate();
            }
        }

        /// <summary>
        /// Loaded event where we can setup the initial state of the form and have available to us the
        /// properties the caller set in their Xaml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FontComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Select the default font as the font of this control which will either be the
            // default font or one the caller provides in their Xaml declaration.
            foreach (string item in this.Items)
            {
                if (item == this.FontFamily.Source)
                {
                    this.SelectedItem = item;

                    break;
                }
            }
        }

        /// <summary>
        /// Sets up or changes the ItemTemplate based off of properties set on this class.
        /// </summary>
        private void SetupItemTemplate()
        {
            // Create the DataTemplate that we are going to put into the ItemTemplate that will be used to
            // bind the font to the TextBlock AND display what that font looks like.
            string template = "";

            if (this.ShowAsFont)
            {
                template = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""> 
                                 <TextBlock Text=""{Binding}"" FontFamily=""{Binding}"" /> 
                             </DataTemplate>";
            }
            else
            {
                template = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                                 <StackPanel Orientation=""Horizontal"">
                                     <TextBlock Text=""{Binding}"" />
                                 </StackPanel>
                             </DataTemplate>";
            }

            this.ItemTemplate = XamlReader.Load(template) as DataTemplate;
        }
    }
}
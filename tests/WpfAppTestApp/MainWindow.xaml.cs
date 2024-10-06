using System;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Xsl;

namespace WpfAppTestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonExecute_Click(object sender, RoutedEventArgs e)
        {
            string xmlFile = "path/to/custom_ui.xml";
            string xsltFile = "path/to/transform.xslt";
            string outputFile = "path/to/output.xaml";

            // Load the XML
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(TextLeft.Text);

            // Load the XSLT
            // XslCompiledTransform xslt = new XslCompiledTransform();
            // xslt.Load(xsltFile);
            XslCompiledTransform xslt = new XslCompiledTransform();
            using (XmlReader xsltReader = XmlReader.Create(new StringReader(_xamlStylesheet)))
            {
              xslt.Load(xsltReader);
            }
            
            // Apply the transformation and save the output
            using (XmlWriter writer = XmlWriter.Create(outputFile, xslt.OutputSettings))
            {
                xslt.Transform(xmlDocument, writer);
            }

            Console.WriteLine("Transformation complete. Output saved to " + outputFile);
        }

        private string _xamlStylesheet = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
  
  <!-- Root template -->
  <xsl:template match=""/MyUI"">
    <xsl:apply-templates/>
  </xsl:template>

  <!-- Transform Window tag -->
  <xsl:template match=""Window"">
    <Window Title=""{@title}"" Width=""{@width}"" Height=""{@height}"">
      <xsl:apply-templates/>
    </Window>
  </xsl:template>

  <!-- Transform StackPanel tag -->
  <xsl:template match=""StackPanel"">
    <StackPanel Orientation=""{@orientation}"">
      <xsl:apply-templates/>
    </StackPanel>
  </xsl:template>

  <!-- Transform Label tag -->
  <xsl:template match=""Label"">
    <Label Content=""{@content}""/>
  </xsl:template>

  <!-- Transform TextBox tag -->
  <xsl:template match=""TextBox"">
    <TextBox Name=""{@name}""/>
  </xsl:template>

  <!-- Transform Button tag -->
  <xsl:template match=""Button"">
    <Button Content=""{@content}"" Click=""{@click}""/>
  </xsl:template>

</xsl:stylesheet>
        ";
    }
}
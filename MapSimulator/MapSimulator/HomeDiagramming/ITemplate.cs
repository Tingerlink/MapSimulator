using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HomeDiagramming
{
    [Serializable]
    public class TemplateDot
    {
        public TemplateDot(string _dotLable = "Точка", int _dotRadius = 16)
        {
            this.DotLable = _dotLable;
            this.DotRadius = _dotRadius;
        }

        //public Color DotColor { get; set; }
        //public Color DotBorderColor { get; set; }
        public string DotLable { get; set; }
        public int DotRadius { get; set; }
    }
    
    public class ITemplate
    {
        public ControlTemplate GetStartTemplate(TemplateDot dotStructure)
        {         
            string ItemTemplate;
            ItemTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
                "<Grid Height=\"" + dotStructure.DotRadius.ToString() + "\" Width=\"" + dotStructure.DotRadius.ToString() + "\">" +
                "<Ellipse Fill=\"#FF485F6A\" Stroke=\"#FF24DE35\"/>" +
                "<Ellipse Fill=\"#FF24DE35\" Stroke=\"Black\" HorizontalAlignment=\"Center\"" +
                    " VerticalAlignment=\"Center\" Width=\"" + (dotStructure.DotRadius / 2).ToString() + "\" Height=\""
                    + (dotStructure.DotRadius / 2).ToString() + "\" StrokeThickness=\"0\"/>" +
                "</Grid>"+
            "</ControlTemplate>"; ;         

            XmlReader xmlReader = XmlReader.Create(new StringReader(ItemTemplate));

            return (ControlTemplate)XamlReader.Load(xmlReader);
        }

        public ControlTemplate GetEndTemplate(TemplateDot dotStructure)
        {
            string ItemTemplate;
            ItemTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
                "<Grid Height=\"" + (dotStructure.DotRadius).ToString() + "\" Width=\"" + (dotStructure.DotRadius).ToString() + "\">" +
                "<Ellipse Fill=\"#FFCD0C5B\" Stroke=\"#9924DE35\"/>" +
                "<Ellipse Fill=\"#9924DE35\" Stroke=\"Black\" HorizontalAlignment=\"Center\"" +
                    " VerticalAlignment=\"Center\" Width=\"" + ((dotStructure.DotRadius) / 2).ToString() + "\" Height=\""
                    + ((dotStructure.DotRadius) / 2).ToString() + "\" StrokeThickness=\"0\"/>" +
                "</Grid>" +
            "</ControlTemplate>"; ;

            XmlReader xmlReader = XmlReader.Create(new StringReader(ItemTemplate));

            return (ControlTemplate)XamlReader.Load(xmlReader);
        }

        public ControlTemplate GetSimpleTemplate(TemplateDot dotStructure)
        {
            string ItemTemplate;
            ItemTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +
                "<Grid Height=\"" + dotStructure.DotRadius.ToString() + "\" Width=\"" + dotStructure.DotRadius.ToString() + "\">" +
                "<Ellipse Fill=\"#FFF5F515\" Stroke=\"#9924DE35\"/>" +
                "<Ellipse Fill=\"#9924DE35\" Stroke=\"Black\" HorizontalAlignment=\"Center\"" +
                    " VerticalAlignment=\"Center\" Width=\"" + (dotStructure.DotRadius / 2).ToString() + "\" Height=\""
                    + (dotStructure.DotRadius / 2).ToString() + "\" StrokeThickness=\"0\"/>" +
                "</Grid>" +
            "</ControlTemplate>"; ;

            XmlReader xmlReader = XmlReader.Create(new StringReader(ItemTemplate));

            return (ControlTemplate)XamlReader.Load(xmlReader);
        }


        public ControlTemplate GetSimulationObject(string name, string nextPivotName, string sectorName, string time, string comment)
        {
            string ItemTemplate;
            ItemTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" +


              "<Grid Canvas.Left=\"154\" Canvas.Top=\"114\">"+
                    "<Grid Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Width=\"16\" Height=\"16\">"+
                        "<Ellipse Fill=\"#FF4CA2A6\" Stroke=\"#FF727171\"/>"+
                        "<Ellipse Fill=\"#FFF9F9F9\" Stroke=\"#FF8B8B8B\" Margin=\"2\"/>"+
                        "<Ellipse Fill=\"#FF1A1A93\" Margin=\"4\"/>"+
                    "</Grid>"+
                    "<StackPanel Margin=\"27,0,0,0\" HorizontalAlignment=\"Right\" VerticalAlignment=\"Top\">"+
                        "<StackPanel Margin=\"0\" Orientation=\"Horizontal\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" Text=\"Объект:\" FontFamily=\"Tahoma\" FontSize=\"8\" Padding=\"0,0,5,0\" Foreground=\"#FFA4A4A4\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"/>"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" TextWrapping=\"Wrap\" Text=\"" + name + "\" FontFamily=\"Tahoma\" FontSize=\"9\" Padding=\"0,0,5,0\" Foreground=\"#FF080374\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" MaxWidth=\"180\"/>" +
                        "</StackPanel>"+
                        "<StackPanel Orientation=\"Horizontal\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" Text=\"Направление:\" FontFamily=\"Tahoma\" FontSize=\"8\" Padding=\"0,0,5,0\" Foreground=\"#FFA4A4A4\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"/>"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" TextWrapping=\"Wrap\" Text=\"" + nextPivotName + "\" FontFamily=\"Tahoma\" FontSize=\"9\" Padding=\"0,0,5,0\" Foreground=\"#FF080374\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" MaxWidth=\"180\"/>" +
                        "</StackPanel>"+
                        "<StackPanel Orientation=\"Horizontal\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\">" +
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" Text=\"Участок\" FontFamily=\"Tahoma\" FontSize=\"8\" Padding=\"0,0,5,0\" Foreground=\"#FFA4A4A4\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"/>" +
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" TextWrapping=\"Wrap\" Text=\"" + sectorName + "\" FontFamily=\"Tahoma\" FontSize=\"9\" Padding=\"0,0,5,0\" Foreground=\"#FF080374\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" MaxWidth=\"180\"/>" +
                        "</StackPanel>" +
                        "<StackPanel Margin=\"0,0,25,0\" Orientation=\"Horizontal\">"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" Text=\"Время прохождения участка:\" FontFamily=\"Tahoma\" FontSize=\"8\" Padding=\"0,0,5,0\" Foreground=\"#FFA4A4A4\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"/>"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" TextWrapping=\"Wrap\" Text=\"" + time + " (сек.)\" FontFamily=\"Tahoma\" FontSize=\"9\" Padding=\"0,0,5,0\" Foreground=\"#FF080374\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" MaxWidth=\"180\"/>" +

                        "</StackPanel>"+
                        "<StackPanel Margin=\"0,0,25,0\" Orientation=\"Horizontal\">"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" Text=\"Информация:\" FontFamily=\"Tahoma\" FontSize=\"8\" Padding=\"0,0,5,0\" Foreground=\"#FFA4A4A4\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\"/>"+
                            "<TextBlock TextOptions.TextFormattingMode=\"Display\" TextWrapping=\"Wrap\" Text=\"" + comment + "\" FontFamily=\"Tahoma\" FontSize=\"9\" Padding=\"0,0,5,0\" Foreground=\"#FF080374\" Margin=\"0\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" MaxWidth=\"180\"/>" +

                        "</StackPanel>"+

                    "</StackPanel>"+

                "</Grid>"+
            "</ControlTemplate>";

            XmlReader xmlReader = XmlReader.Create(new StringReader(ItemTemplate));

            return (ControlTemplate)XamlReader.Load(xmlReader);
        }

    }
}

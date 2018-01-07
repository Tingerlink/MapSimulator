using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HomeDiagramming.Connectors
{    
    public class ShapeConnectorBase : Shape
    {        
        LineGeometry linegeo;

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(ShapeConnectorBase), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(ShapeConnectorBase), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        #region Публичные поля класса
        
        private double _coefSize = 1.0;                

        private BasicShape _startDot;
        private BasicShape _endDot;

        private Color _color = Colors.GreenYellow;

        private double _lenght;
        private double _time = 3;

        private string _name = "";
        private int _id;
        private int _routeId;

        public BasicShape StartDot
        {
            get { return _startDot; }
            set { _startDot = value; }
        }
        public BasicShape EndDot
        {
            get { return _endDot; }
            set { _endDot = value; }
        }
        public Color Color
        {
            get  
            { 
                return _color; 
            }
            set { _color = value; }
        }
        public double Lenght
        {
            get
            {
                _lenght = System.Math.Sqrt(System.Math.Pow((_endDot.GetPosition().X - _startDot.GetPosition().X), 2) + System.Math.Pow((_endDot.GetPosition().Y - _startDot.GetPosition().Y), 2)) * _coefSize;
                return _lenght;
            }
        }
        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public string LineName
        {
            get
            {
                if (_name == "")
                {
                    return StartDot.DotName + " - " + EndDot.DotName;
                }

                return _name;
            }
            set { _name = value; }
        }
        public double CoefSize
        {
            get { return _coefSize; }
            set { _coefSize = value; }
        }
        public int ConnectorId
        {
            get { return _id; }
            set { _id = value; }
        }
        public int RouteId
        {
            get { return _routeId; }
            set { _routeId = value; }
        }

        public void SetToolTip()
        {
            this.ToolTip = LineName;
        }

        private Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }
        private Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        #endregion
                       
        public ShapeConnectorBase()
        {
            linegeo = new LineGeometry();
            
            this.StrokeThickness = 2;

            this.Stroke = new SolidColorBrush(Color);
                       
            this.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(_conectionOption);        
        }

    

        /// <summary>
        /// Обработчик события нажатия правой кнопки по линии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _conectionOption(object sender, RoutedEventArgs e)
        {

        }
        
        protected override Geometry DefiningGeometry
        {
            get
            {
                linegeo.StartPoint = StartPoint;
                linegeo.EndPoint = EndPoint;
                return linegeo;
            }
        }

    }
}

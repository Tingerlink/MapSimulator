using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HomeDiagramming
{
    
    public class BasicShape : Thumb, IDraggable, IConnectable
    {
        public BasicShape(TemplateDot dotTemplate, Point position, int id, int routeId)
        {
            this.CanDrag = true;
            this.CanConnect = true;
            this.DragDelta += new DragDeltaEventHandler(BasicShape_DragDelta);
            //this.MouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(BasicShape_MouseRightButtonDown);

            this.DotTemplate = dotTemplate;
            this.DotPoint = position;
            this.DotId = id;            
            this.RouteId = routeId;

            this._dotName = "Точка №" + id.ToString();
            this.ToolTip = DotName;
            this.SetType("End");

            Point centerDot = new Point(position.X - dotTemplate.DotRadius / 2, position.Y - dotTemplate.DotRadius / 2);

            this.SetPosition(centerDot);
        }
        
        #region Templates

        public void SetEndTemplate()
        {
            this.Template = (new ITemplate()).GetEndTemplate(this.DotTemplate);
        }
        public void SetStartTemplate()
        {
            this.Template = (new ITemplate()).GetStartTemplate(this.DotTemplate);
        }
        public void SetSimpleTemplate()
        {
            this.Template = (new ITemplate()).GetSimpleTemplate(this.DotTemplate);
        }

        public void SetType(string type)
        {
            this.DotType = type;

            switch (type)
            {
                case "Start":
                    {
                        SetStartTemplate();
                        break;
                    }
                case "End":
                    {
                        SetEndTemplate();
                        break;
                    }
                case "Simple":
                    {
                        SetSimpleTemplate();
                        break;
                    }

                default:
                    break;
            }
        }

        #endregion
        
        private string _dotName = "";
                
        #region        
        public TemplateDot DotTemplate { get; set; }
        public string DotName
        {
            get 
            {
                return _dotName;
            }
            set { _dotName = value; }        
        }
        public Point DotPoint { get { return this.GetPosition(); } set { } }
        public int DotId { get; set; }
        public BasicShape NextDot { get; set; }
        public BasicShape PrevDot { get; set; }
        public int RouteId { get; set; }
        public string DotType { get; set; }


        #endregion


        #region Events
        void BasicShape_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!CanDrag)
                return;

            this.SetPosition(new Point(Canvas.GetLeft(this) + e.HorizontalChange, Canvas.GetTop(this) + e.VerticalChange));
        }
        #endregion

        #region IDraggable Members

        public bool CanDrag { get; set; }

        public void SetPosition(Point value)
        {            
            Canvas.SetLeft(this, value.X);
            Canvas.SetTop(this, value.Y);
        }

        public Point GetPosition()
        {
            return new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
        }

        #endregion

        #region IConnectable Members
                
        public bool CanConnect { get; set; }
        
        #endregion
    }
}

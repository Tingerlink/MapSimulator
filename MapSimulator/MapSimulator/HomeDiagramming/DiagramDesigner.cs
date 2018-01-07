using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HomeDiagramming.Connectors;
using HomeDiagramming.Connectors.Converters;


namespace HomeDiagramming
{
     
    public class DiagramDesigner
    {
        public Canvas DiagramView { get; private set; }

        #region ctor
        public DiagramDesigner(Canvas surface)
        {
            this.DiagramView = surface;            
        }

        #endregion

        public BasicShape AddShape(Point position, int id, int routeId, TemplateDot templateDot)
        {
            BasicShape shape = new BasicShape(templateDot, position, id, routeId);
           
            this.DiagramView.Children.Add(shape);
            
            return shape;
        }

        public BasicShape AddShape(BasicShape shape)
        {         
            this.DiagramView.Children.Add(shape);

            return shape;
        }

        public ShapeConnectorBase AddConnection(BasicShape source, BasicShape target, int id, int routeId)
        {
            ShapeConnectorBase conn = new ShapeConnectorBase();
            conn.SetBinding(ShapeConnectorBase.StartPointProperty, CreateConnectorBinding((IConnectable)target));
            conn.SetBinding(ShapeConnectorBase.EndPointProperty, CreateConnectorBinding((IConnectable)source));
            conn.StartDot = target;
            conn.EndDot = source;
            conn.SetToolTip();
            conn.ConnectorId = id;
            conn.RouteId = routeId;

            this.DiagramView.Children.Insert(0, conn);
          
            return conn;
        }

        public void RemoveShape(BasicShape removingShape)
        {
            this.DiagramView.Children.Remove(removingShape);
        }
        public void RemoveConnection(HomeDiagramming.Connectors.ShapeConnectorBase removingConnection)
        {
            this.DiagramView.Children.Remove(removingConnection);
        }


        #region Обработчики событий

        private void del_item_click(object sender, RoutedEventArgs e)
        {

        }
        private void re_item_click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
        
        private MultiBinding CreateConnectorBinding(IConnectable connectable)
        {
            // Create a multibinding collection and assign an appropriate converter to it
            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Converter = new ConnectorBindingConverter();

            // Create binging #1 to IConnectable to handle Left
            Binding binding = new Binding();
            binding.Source = connectable;
            binding.Path = new PropertyPath(Canvas.LeftProperty);
            multiBinding.Bindings.Add(binding);

            // Create binging #2 to IConnectable to handle Top
            binding = new Binding();
            binding.Source = connectable;
            binding.Path = new PropertyPath(Canvas.TopProperty);
            multiBinding.Bindings.Add(binding);

            // Create binging #3 to IConnectable to handle ActualWidth
            binding = new Binding();
            binding.Source = connectable;
            binding.Path = new PropertyPath(FrameworkElement.ActualWidthProperty);
            multiBinding.Bindings.Add(binding);

            // Create binging #4 to IConnectable to handle ActualHeight
            binding = new Binding();
            binding.Source = connectable;
            binding.Path = new PropertyPath(FrameworkElement.ActualHeightProperty);
            multiBinding.Bindings.Add(binding);

            return multiBinding;
        }


    }

}
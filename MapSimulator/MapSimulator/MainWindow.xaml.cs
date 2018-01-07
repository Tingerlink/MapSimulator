using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.IO;
using HomeDiagramming;

namespace MapSimulator
{    
    public class MetaInfo
    {
        private string imagePath;
        public string ImagePath
        {
            get { return this.imagePath; }
            set { this.imagePath = value; }
        }       
        public string ProjectPath { get; set;}
    }
    
    public class PresentationSector
    {
        public PresentationSector(int sectorId, int routeId, string sectorName, string startDotName, string endDotName, double sectorTime)
        {
            this.SectorId = sectorId;
            this.RouteId = routeId;
            this.SectorName = sectorName;
            this.StartDotName = startDotName;
            this.EndDotName = endDotName;
            this.SectorTime = sectorTime;            
        }

        private double _sectorTime;

        public int SectorId { get; private set; }
        public int RouteId { get; private set; }
        public string SectorName { get; set; }
        public string StartDotName { get; private set; }
        public string EndDotName { get; private set; }
        public double SectorTime
        {
            get
            {
                return _sectorTime;
            }
            set
            {
                try
                {
                    double newValue = double.Parse(value.ToString());

                    _sectorTime = newValue;
                }
                catch
                {
                    _sectorTime = 3;
                }
            }
        }
        public double SectorLenght { get; private set; }
    }

    public class PresentationPivot
    {
        public PresentationPivot(int pivotId, int routeId, string pivotName, string prevDotName, string nextDotName)
        {
            this.PivotId = pivotId;
            this.RouteId = routeId;
            this.PivotName = pivotName;
            this.PrevDotName = prevDotName;
            this.NextDotName = nextDotName;
        }

        public int PivotId { get; private set; }
        public int RouteId { get; private set; }
        public string PivotName { get; set; }
        public string PrevDotName { get; private set; }
        public string NextDotName { get; private set; }        
    }

    public class AnimationSector
    {
        public AnimationSector (Point startPosition, Point endPosition, double time)
	    {
            StartPosition = new Thickness(startPosition.X, startPosition.Y, 0, 0);
            EndPosition = new Thickness(endPosition.X, endPosition.Y, 0, 0);

            TimeAnimation = time;
	    }

        public Thickness StartPosition { get; private set; }
        public Thickness EndPosition { get; private set; }
        public double TimeAnimation { get; private set; }

    }
        
    public class Route
    {
        public Route(int id)
        {
            this.RouteId = id;
            this.Pivots = new List<BasicShape>();
            this.Sectors = new List<HomeDiagramming.Connectors.ShapeConnectorBase>();
            this.RouteName = "Маршрут №" + (id + 1);
            this.RouteSpeed = 60.0;
            this.RouteComment = "...";
        }
        
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public double RouteSpeed { get; set; }
        public string RouteComment { get; set; }

        public int AnimateSectorIndex { get; set; }
        public Control AnimationObject { get; set; }
        public Storyboard AnimationStoryBoard { get; set; }

        public List<HomeDiagramming.Connectors.ShapeConnectorBase> Sectors { get; set; }
        public List<BasicShape> Pivots { get; set; }
        
        public List<AnimationSector> AnimationSectors { get; set; }

        public void AddSector(HomeDiagramming.Connectors.ShapeConnectorBase sector)
        {
            Sectors.Add(sector);
        }
        public void AddPivot(BasicShape pivot)
        {
            Pivots.Add(pivot);
        }

        public BasicShape GetPivotById(int id)
        {
            foreach (BasicShape item in Pivots)
            {
                if (item.DotId == id)
                {
                    return item;
                }
            }

            return null;
        }
        public HomeDiagramming.Connectors.ShapeConnectorBase GetSectorById(int id)
        {
            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in Sectors)
            {
                if (item.ConnectorId == id)
                {
                    return item;
                }
            }

            return null;
        }
        public HomeDiagramming.Connectors.ShapeConnectorBase GetSectorByStartAndEndPivot(BasicShape startPivot, BasicShape endPivot)
        {
            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in Sectors)
            {
                if ((item.StartDot == startPivot && item.EndDot == endPivot) || (item.StartDot == endPivot && item.EndDot == startPivot))
                {
                    return item;
                }
            }

            return null;
        }
        public HomeDiagramming.Connectors.ShapeConnectorBase GetSectorHavingStartPivot(BasicShape startPivot)
        {
            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in Sectors)
            {
                if (item.StartDot == startPivot)
                {
                    return item;
                }
            }

            return null;
        }
        public HomeDiagramming.Connectors.ShapeConnectorBase GetSectorHavingEndPivot(BasicShape endPivot)
        {
            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in Sectors)
            {
                if (item.EndDot == endPivot)
                {
                    return item;
                }
            }

            return null;
        }
        
        public void RemoveSectorByStartAndEndPivot(BasicShape startPivot, BasicShape endPivot)
        {
            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in Sectors)
            {
                if ((item.StartDot == startPivot && item.EndDot == endPivot) || (item.StartDot == endPivot && item.EndDot == startPivot))
                {
                    Sectors.Remove(item);

                    return;
                }
            }
        }
        public void RemovePivot(BasicShape pivot)
        {
            Pivots.Remove(pivot);
        }
        public void RemoveSector(HomeDiagramming.Connectors.ShapeConnectorBase sector)
        {
            Sectors.Remove(sector);
        }
        
        public int ConcatRoute(Route route)
        {
            foreach (BasicShape item in route.Pivots)
            {
                item.RouteId = this.RouteId;
                this.AddPivot(item);
            }

            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in route.Sectors)
            {
                this.AddSector(item);
            }

            return this.RouteId;

        }
        
        public List<PresentationSector> GetPresentationSectors()
        {
            List<PresentationSector> presentationSectors = new List<PresentationSector>();

            foreach (HomeDiagramming.Connectors.ShapeConnectorBase item in this.Sectors)
            {
                presentationSectors.Add(new PresentationSector(item.ConnectorId, item.RouteId, item.LineName, item.StartDot.DotName,
                    item.EndDot.DotName, item.Time));
            }
            
            return presentationSectors;
        }
        public List<PresentationPivot> GetPresentationPivot()
        {
            List<PresentationPivot> presentationPivots = new List<PresentationPivot>();

            foreach (BasicShape item in this.Pivots)
            {
                string prevPivodName = "Не определена";
                string nextPivodName = "Не определена";

                if (item.PrevDot != null)
                {
                    prevPivodName = item.PrevDot.DotName;
                }

                if (item.NextDot != null)
                {
                    nextPivodName = item.NextDot.DotName;
                }

                presentationPivots.Add(new PresentationPivot(item.DotId, item.RouteId, item.DotName, prevPivodName, nextPivodName));
            }

            return presentationPivots;
        }
        
        public BasicShape GetStartPivot()
        {
            foreach (BasicShape item in Pivots)
            {
                if (item.DotType == "Start")
                {
                    return item;
                }
            }

            return null;
        }

        public List<AnimationSector> GetAnimationSectors(Canvas parent)
        {
            List<AnimationSector> animationSectors = new List<AnimationSector>();

            BasicShape startPivot = GetStartPivot();
            BasicShape endPivot = startPivot.NextDot;

            if (startPivot == null) return null;
            Random rand = new Random();

            while (endPivot != null)
            {
                HomeDiagramming.Connectors.ShapeConnectorBase sector = GetSectorByStartAndEndPivot(startPivot, endPivot);
               
                double time = sector.Time;

                animationSectors.Add(new AnimationSector(startPivot.GetPosition(), endPivot.GetPosition(), time));

                if (endPivot.DotType == "Start") break;

                startPivot = endPivot;
                endPivot = endPivot.NextDot;
            }

            return animationSectors; 

        }

        public void SetAnimationObject()
        {
            BasicShape prevPivot, nextPivot;
            int nextIndex = this.AnimateSectorIndex + 1;

            if (this.AnimateSectorIndex + 1 >= this.Pivots.Count)
            {
                nextIndex = 0;
            }

            prevPivot = this.Pivots[this.AnimateSectorIndex];
            nextPivot = this.Pivots[nextIndex];

            HomeDiagramming.Connectors.ShapeConnectorBase sector = GetSectorByStartAndEndPivot(prevPivot, nextPivot);

            AnimationObject = new Control();
            AnimationObject.Template = (new ITemplate()).GetSimulationObject(RouteName,
                this.Pivots[nextIndex].DotName,
                sector.LineName,
                Math.Round(sector.Time, 2).ToString(), 
                RouteComment);
        }
        public void UpdateTemplate()
        {
            BasicShape prevPivot, nextPivot;
            int nextIndex = this.AnimateSectorIndex + 1;

            if (this.AnimateSectorIndex + 1 >= this.Pivots.Count)
            {
                nextIndex = 0;
            }            

            prevPivot = this.Pivots[this.AnimateSectorIndex];
            nextPivot = this.Pivots[nextIndex];

            HomeDiagramming.Connectors.ShapeConnectorBase sector = GetSectorByStartAndEndPivot(prevPivot, nextPivot);

            AnimationObject.Template = (new ITemplate()).GetSimulationObject(RouteName,
                this.Pivots[nextIndex].DotName,
                sector.LineName,
                Math.Round(sector.Time, 2).ToString(),
                RouteComment);
        }

    }

    [Serializable]
    public class PivotSerialize
    {
        public PivotSerialize(Point position, int dotId, int nextDotId, int prevDotId, int routeId, string dotName, string type)
        {
            this.Position = position;
            this.DotId = dotId;
            this.NextDotId = nextDotId;
            this.PrevDotId = prevDotId;
            this.RouteId = routeId;
            this.DotName = dotName;
            this.Type = type;
        }

        public Point Position {get; set; }
        public string Type { get; set; }
        public int DotId {get; set; }
        public int NextDotId {get; set; }
        public int PrevDotId { get; set; }
        public int RouteId {get; set; }
        public string DotName {get; set; }        
    }

    [Serializable]
    public class SectorSerialize
    {
        public SectorSerialize(int startDotId, int endDotId, double time, string lineName, int connectorId, int routeId)
        {
            this.StartDotId = startDotId;
            this.EndDotId = endDotId;
            this.Time = time;
            this.LineName = lineName;
            this.ConnectorId = connectorId;
            this.RouteId = routeId;
        }

        public int StartDotId { get; set; }
        public int EndDotId { get; set; }

        public double Time { get; set; }
        public string LineName { get; set; }

        public int ConnectorId { get; set; }
        public int RouteId { get; set; }
    }

    [Serializable]
    public class RouteSerialize
    {
        public List<PivotSerialize> Pivots;
        public List<SectorSerialize> Sectors;

        public int RouteId {get;set;}
        public string RouteName {get;set;}
        public string RouteComment {get;set;}
    }

    [Serializable]
    public class ProjectSerialize
    {
        public List<RouteSerialize> Routes {get; set;}
        public int NextDotId {get; set;}
        public int NextConnectorId {get; set;}
        public int NextRouteId {get; set;}
    }

    public partial class MainWindow : Window
    {
        #region Triggers
        private bool isNewPivot = false;
        private bool isDeletePivot = false;
        private bool isSimalation = false;
        private bool isSimalationPause = false;        
        #endregion

        #region Private Variably
        private List<Route> routes { get; set; }

        private MetaInfo projectMeta;
        private DiagramDesigner designer { get; set; }      
        private List<PresentationSector> presentationSector { get; set; }
        private List<PresentationPivot> presentationPivot { get; set; }

        private BasicShape lastDot { get; set; }

        private int NextDotId = 0;
        private int NextConnectorId = 0;
        private int NextRouteId = 0;

        private int SelectRouteId = -1;

        private double FasterCoef = 1.0;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.SetMainEvents();
        }       
        
        #region Window Settings
        private void SetMainEvents()
        {
            this.WorkAreaCanvas.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(WorkAreaCanvas_PreviewMouseLeftButtonDown);            
        }     
        #endregion

        #region Route Methods
        private Route CreateNewRoute(int id)
        {
            Route newRoute = new Route(id);

            return newRoute;
        }
        private void RemoveRouteById(int id)
        {
            routes.Remove(GetRouteById(id));            
            if (routes.Count <= 0)
            {
                VisStartRoutePanel();
                SelectRouteId = -1;
            }
        }
        private void AddRoute(Route route)
        {
            routes.Add(route);                        
        }
        private Route GetRouteById(int id)
        {
            foreach (Route item in this.routes)
            {
                if (item.RouteId == id)
                {
                    return item;
                }
            }

            return null;
        }
        #endregion

        #region Work Area Events
        private void WorkAreaCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isSimalation)
            {
                return;
            }

            if (isNewPivot)
            {
                ClickForCreateNewDot(sender, e);
            }

            if (isDeletePivot)
            {
                ClickForDeletePivot(sender, e);
            }

            if (!isNewPivot && !isDeletePivot && e.Source is BasicShape && ((IConnectable)e.Source).CanConnect)
            {
                SetRouteInfo(((BasicShape)e.Source).RouteId);
                HideStartRoutePanel();
            }
        }

        #region Delete pivot

        private void ClickForDeletePivot(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is BasicShape && ((IConnectable)e.Source).CanConnect)
            {
                ClickForDeleteFromDot((BasicShape)e.Source);
            }
        }

        private void ClickForDeleteFromDot(BasicShape removingPivot)
        {            
            if (removingPivot.NextDot == null && removingPivot.PrevDot == null)
            {
                ClickForDeleteTrivialPivot(removingPivot);
            }

            if (removingPivot.NextDot != null && removingPivot.PrevDot == null)
            {
                ClickForDeleteStartPivot(removingPivot);
            }

            if (removingPivot.NextDot == null && removingPivot.PrevDot != null)
            {
                ClickForDeleteEndPivot(removingPivot);
            }

            if (removingPivot.NextDot != null && removingPivot.PrevDot != null)
            {
                ClickForDeleteSimplePivot(removingPivot);
            }
        }
        
        private void ClickForDeleteTrivialPivot(BasicShape removingPivot)
        {
            designer.RemoveShape(removingPivot);

            RemoveRouteById(removingPivot.RouteId);
        }
        private void ClickForDeleteStartPivot(BasicShape removingPivot)
        {
            removingPivot.NextDot.SetType("Start");
            removingPivot.NextDot.PrevDot = null;

            Route selectRoute = GetRouteById(removingPivot.RouteId);

            designer.RemoveShape(removingPivot);
            designer.RemoveConnection(selectRoute.GetSectorByStartAndEndPivot(removingPivot, removingPivot.NextDot));

            selectRoute.RemoveSectorByStartAndEndPivot(removingPivot, removingPivot.NextDot);
            selectRoute.RemovePivot(removingPivot);
        }
        private void ClickForDeleteEndPivot(BasicShape removingPivot)
        {
            if (removingPivot.PrevDot.DotType != "Start")
            {
                removingPivot.PrevDot.SetType("End");
            }

            removingPivot.PrevDot.NextDot = null;

            Route selectRoute = GetRouteById(removingPivot.RouteId);

            designer.RemoveShape(removingPivot);
            designer.RemoveConnection(selectRoute.GetSectorByStartAndEndPivot(removingPivot.PrevDot, removingPivot));

            selectRoute.RemoveSectorByStartAndEndPivot(removingPivot.PrevDot, removingPivot);
            selectRoute.RemovePivot(removingPivot);
        }
        private void ClickForDeleteSimplePivot(BasicShape removingPivot)
        {
            if (removingPivot.DotType == "Start") removingPivot.PrevDot.SetType("Start");

            Route selectRoute = GetRouteById(removingPivot.RouteId);

            designer.RemoveConnection(selectRoute.GetSectorByStartAndEndPivot(removingPivot.PrevDot, removingPivot));
            designer.RemoveConnection(selectRoute.GetSectorByStartAndEndPivot(removingPivot, removingPivot.NextDot));
            designer.RemoveShape(removingPivot);

            if (selectRoute.GetSectorByStartAndEndPivot(removingPivot.PrevDot, removingPivot.NextDot) == null)
            {
                selectRoute.AddSector(designer.AddConnection(removingPivot.PrevDot, removingPivot.NextDot, NextConnectorId, removingPivot.RouteId));
            }
            else
            {
                removingPivot.PrevDot.PrevDot = null;
                removingPivot.PrevDot.NextDot = null;
                removingPivot.NextDot.NextDot = null;
                removingPivot.NextDot.PrevDot = null;

                if (removingPivot.DotType == "Start")
                {
                    removingPivot.NextDot.SetType("Start");
                    removingPivot.PrevDot.SetType("End");
                }
                else
                {
                    if (removingPivot.NextDot.DotType != "Start")
                    {
                        removingPivot.NextDot.SetType("End");
                    }
                    if (removingPivot.PrevDot.DotType != "Start")
                    {
                        removingPivot.PrevDot.SetType("End");
                    }

                }
                
            }

            selectRoute.RemoveSectorByStartAndEndPivot(removingPivot.PrevDot, removingPivot);
            selectRoute.RemoveSectorByStartAndEndPivot(removingPivot, removingPivot.NextDot);
            selectRoute.RemovePivot(removingPivot);

            removingPivot.PrevDot.NextDot = removingPivot.NextDot;
            removingPivot.NextDot.PrevDot = removingPivot.PrevDot;

            NextConnectorId++;
        }


        #endregion

        #region Create new pivot

        private void ClickForCreateNewDot(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is BasicShape && ((IConnectable)e.Source).CanConnect)
            {
                ClickForCreateFromDot(sender, e);
            }

            if (e.Source is Canvas)
            {
                ClickForCreateFromCanvas(sender, e);
            }
        }

        private void ClickForCreateFromDot(object sender, MouseButtonEventArgs e)
        {
            if (lastDot == null)
            {
                ClickFromDotAndNotHavingLastDot((BasicShape)e.Source);
                return;
            }
            else
            {
                ClickFromDotAndHavingLastDot((BasicShape)e.Source);
                return;
            }
        }
        private void ClickForCreateFromCanvas(object sender, MouseButtonEventArgs e)
        {
            if (lastDot == null)
            {
                ClickFromCanvasAndNotHavingLastDot(e);
                return;
            }
            else
            {
                ClickFromCanvasAndHavingLastDot(e);
                return;
            }
        }
        
        private void ClickFromCanvasAndHavingLastDot(MouseButtonEventArgs e)
        {
            if (lastDot.DotType != "Start") lastDot.SetType("Simple");

            int routeId = lastDot.RouteId;

            BasicShape newPivot = designer.AddShape(e.GetPosition(WorkAreaCanvas), NextDotId, routeId, new TemplateDot());
            Route selectRoute = this.GetRouteById(routeId);
            selectRoute.AddPivot(newPivot);
            selectRoute.AddSector(designer.AddConnection(newPivot, lastDot, NextConnectorId, lastDot.RouteId));

            lastDot.NextDot = newPivot;
            newPivot.PrevDot = lastDot;

            NextDotId++;
            NextConnectorId++;

            lastDot = newPivot;
        }
        private void ClickFromCanvasAndNotHavingLastDot(MouseButtonEventArgs e)
        {
            Route newRoute = CreateNewRoute(NextRouteId);
            BasicShape newPivot = designer.AddShape(e.GetPosition(WorkAreaCanvas), NextDotId, NextRouteId, new TemplateDot());
            newPivot.SetType("Start");

            newRoute.AddPivot(newPivot);
            AddRoute(newRoute);
            
            lastDot = newPivot;

            NextRouteId++;
            NextDotId++;
        }
        private void ClickFromDotAndHavingLastDot(BasicShape nextPivot)
        {
            if (nextPivot != lastDot)
            {
                if (nextPivot.PrevDot == null)
                {
                    if (nextPivot.RouteId == lastDot.RouteId)
                    {
                        RoundSelfRoute(nextPivot);
                    }
                    else
                    {
                        RoundNotSelfRoute(nextPivot);
                    }
                }
            }
        }
        private void ClickFromDotAndNotHavingLastDot(BasicShape nextPivot)
        {
            if (nextPivot.NextDot == null)
            {
                lastDot = nextPivot;                
            }
        }
        
        private void RoundSelfRoute(BasicShape nextPivot)
        {
            if (lastDot.DotType != "Start") lastDot.SetType("Simple");

            Route selectRoute = this.GetRouteById(lastDot.RouteId);

            lastDot.NextDot = nextPivot;
            nextPivot.PrevDot = lastDot;

            selectRoute.AddSector(designer.AddConnection(nextPivot, lastDot, NextConnectorId, lastDot.RouteId));

            NextConnectorId++;

            lastDot = null;
        }
        private void RoundNotSelfRoute(BasicShape nextPivot)
        {
            if (lastDot.DotType != "Start") lastDot.SetType("Simple");
            if (nextPivot.DotType != "End") nextPivot.SetType("Simple");
            if (nextPivot.NextDot == null) nextPivot.SetType("End");

            Route selfRoute = this.GetRouteById(lastDot.RouteId);
            Route notSelfRoute = this.GetRouteById(nextPivot.RouteId);

            lastDot.NextDot = nextPivot;
            nextPivot.PrevDot = lastDot;

            selfRoute.ConcatRoute(notSelfRoute);
            SelectRouteId = -1;
            VisStartRoutePanel();

            selfRoute.AddSector(designer.AddConnection(nextPivot, lastDot, NextConnectorId, lastDot.RouteId));

            RemoveRouteById(notSelfRoute.RouteId);

            NextConnectorId++;

            lastDot = null;
        }

        #endregion

        #endregion

        #region Map Image
        private void SetMapImage(string fileName)
        {
            try
            {
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(fileName, UriKind.Relative));

                WorkAreaCanvas.Height = ib.ImageSource.Height;
                WorkAreaCanvas.Width = ib.ImageSource.Width;


                WorkAreaCanvas.Background = ib;
                WorkAreaCanvas.Background.Opacity = opacitySlider.Value;

                projectMeta.ImagePath = fileName;                             
             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить изображение. Текст ошибки: " + ex.Message);
            }
        }
        private bool OpenImage()
        {
            if (projectMeta == null) return false;

            Stream myStream = null;
            Microsoft.Win32.OpenFileDialog openMapDialog = new Microsoft.Win32.OpenFileDialog();

            openMapDialog.InitialDirectory = "";
            openMapDialog.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|TIF (*.tif)|*.tif";
            openMapDialog.FilterIndex = 2;
            openMapDialog.RestoreDirectory = true;

            if (openMapDialog.ShowDialog() != null)
            {
                try
                {
                    if (openMapDialog.FileName != "" && openMapDialog.FileName != null && (myStream = openMapDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            SetMapImage(openMapDialog.FileName);
                            
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при открытии изображения. Текст ошибки: " + ex.Message);
                    return false;
                }
            }

            return false;
        }
        private void SetOpacity(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WorkAreaCanvas != null && WorkAreaCanvas.Background != null)
            {
                WorkAreaCanvas.Background.Opacity = e.NewValue;
            }
        }

        #endregion  

        #region GUI
        private void HideStartPage()
        {
            startPage.Visibility = System.Windows.Visibility.Hidden;
        }
        private void FadeStatrPage()
        {
            startPage.Visibility = System.Windows.Visibility.Visible;
        }
        
        private void EnableDefault()
        {
            isNewPivot = false;
            isDeletePivot = false;
            isSimalation = false;

            this.WorkAreaCanvas.Cursor = Cursors.Arrow;
        }
        
        private void SetSimulationStateGUI()
        {
            bottomTools.Cursor = Cursors.Wait;
            RoutePanel.Cursor = Cursors.Wait;
            ProjectMenu.Cursor = Cursors.Wait;
            MapMenu.Cursor = Cursors.Wait;
            ProjectToolBar.Cursor = Cursors.Wait;

            bottomTools.IsEnabled = false;
            RoutePanel.IsEnabled = false;
            ProjectMenu.IsEnabled = false;
            MapMenu.IsEnabled = false;
            ProjectToolBar.IsEnabled = false;
            SettingsRouteToolBar.IsEnabled = false;

            bottomTools.Opacity = 0.6;
            RoutePanel.Opacity = 0.6;
            ProjectMenu.Opacity = 0.6;
            MapMenu.Opacity = 0.6;
            ProjectToolBar.Opacity = 0.6;
            SettingsRouteToolBar.Opacity = 0.6;

        }
        private void SetDefaultStateGUI()
        {
            bottomTools.IsEnabled = true;
            RoutePanel.IsEnabled = true;
            ProjectMenu.IsEnabled = true;
            MapMenu.IsEnabled = true;
            ProjectToolBar.IsEnabled = true;
            SettingsRouteToolBar.IsEnabled = true;

            bottomTools.Cursor = Cursors.Arrow;
            RoutePanel.Cursor = Cursors.Arrow;
            ProjectMenu.Cursor = Cursors.Arrow;
            MapMenu.Cursor = Cursors.Arrow;
            ProjectToolBar.Cursor = Cursors.Arrow;

            bottomTools.Opacity = 1;
            RoutePanel.Opacity = 1;
            ProjectMenu.Opacity = 1;
            MapMenu.Opacity = 1;
            ProjectToolBar.Opacity = 1;
            SettingsRouteToolBar.Opacity = 1;
        }
        
        private void HideStartRoutePanel()
        {
            RouteStartPanel.Visibility = System.Windows.Visibility.Hidden;
            RoutePanel.Visibility = System.Windows.Visibility.Visible;
        }
        private void VisStartRoutePanel()
        {
            RouteStartPanel.Visibility = System.Windows.Visibility.Visible;
            RoutePanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateSectorFromTable()
        {
            foreach (PresentationSector sector in tableSectorsRoute.Items)
            {
                HomeDiagramming.Connectors.ShapeConnectorBase selectSector = GetRouteById(sector.RouteId).GetSectorById(sector.SectorId);

                selectSector.Time = sector.SectorTime;
                selectSector.LineName = sector.SectorName;
                selectSector.ToolTip = sector.SectorName;
            }
        }
        private void UpdatePivotFromTable()
        {
            foreach (PresentationPivot pivot in tablePivotRoute.Items)
            {
                Route selectRoute = GetRouteById(pivot.RouteId);
                BasicShape selectPivot = selectRoute.GetPivotById(pivot.PivotId);

                selectPivot.DotName = pivot.PivotName;
                selectPivot.ToolTip = pivot.PivotName;

                HomeDiagramming.Connectors.ShapeConnectorBase nextSector = selectRoute.GetSectorHavingStartPivot(selectPivot);
                HomeDiagramming.Connectors.ShapeConnectorBase prevSector = selectRoute.GetSectorHavingStartPivot(selectPivot);

                if (nextSector != null)
                {
                    nextSector.LineName = "";
                    nextSector.ToolTip = nextSector.LineName;
                }

                if (prevSector != null)
                {
                    prevSector.LineName = "";
                    prevSector.ToolTip = prevSector.LineName;
                }

            }
        }

        private void SetRouteInfo(int routeId)
        {
            SelectRouteId = routeId;

            if (routeId != -1)
            {
                Route selectRoute = GetRouteById(routeId);

                lbRouteNmae.Content = selectRoute.RouteName;               
                tbRouteName.Text = selectRoute.RouteName;
                tbComment.Text = selectRoute.RouteComment;

                presentationPivot = selectRoute.GetPresentationPivot();
                presentationSector = selectRoute.GetPresentationSectors();

                tableSectorsRoute.ItemsSource = presentationSector;
                tablePivotRoute.ItemsSource = presentationPivot;
            }
        }
        #endregion
        
        #region Other Events
        private void loadMapImage(object sender, RoutedEventArgs e)
        {
            if (projectMeta == null)
            {
                MessageBoxResult mr = MessageBox.Show("Для загрузки карты необходимо открыть проект.", "Предупреждение!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (projectMeta.ImagePath != null)
                {
                    MessageBoxResult mr = MessageBox.Show("Карта уже загружена, открыть новую?", "Предупреждение!",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

                    if (mr == MessageBoxResult.Yes)
                    {
                        OpenImage();
                    }
                }
                else
                {
                    OpenImage();
                }
            }  
        }
        private void updateRouteInfo(object sender, RoutedEventArgs e)
        {
            if (SelectRouteId == -1) return;

            Route selectRoute = GetRouteById(SelectRouteId);
            selectRoute.RouteName = tbRouteName.Text;
            selectRoute.RouteComment = tbComment.Text;

            UpdatePivotFromTable();
            UpdateSectorFromTable();
        }
        private void refreshRouteInfo(object sender, RoutedEventArgs e)
        {
            if (SelectRouteId != -1)
            {
                SetRouteInfo(SelectRouteId);
            }
        }
        private void start_page_image_load(object sender, RoutedEventArgs e)
        {
            if (OpenImage())
            {
                HideStartPage();
            }
        }
        #endregion

        #region Tools
        private void createDot(object sender, RoutedEventArgs e)
        {
            lastDot = null;

            EnableDefault();

            isNewPivot = true;
            this.WorkAreaCanvas.Cursor = Cursors.Pen;
        }
        private void deletePivot(object sender, RoutedEventArgs e)
        {
            EnableDefault();
            isDeletePivot = true;

            this.WorkAreaCanvas.Cursor = Cursors.Hand;
        }
        private void arrow(object sender, RoutedEventArgs e)
        {
            EnableDefault();
        }                       
        #endregion

        #region Animations Methods
        private bool IsAllSimulationsStoped()
        {
            foreach (Route item in routes)
            {
                if (item.AnimateSectorIndex != 0)
                {
                    return false;
                }
            }

            return true;
        }        
        private void PauseAllAnimation()
        {
            if (!isSimalation) return;

            foreach (Route item in routes)
            {
                if (item.AnimationStoryBoard != null)
                {
                    item.AnimationStoryBoard.Pause();
                }
            }

            isSimalationPause = true;
        }
        private void ContinueAllAnimation()
        {
            if (!isSimalationPause) return;

            foreach (Route item in routes)
            {
                if (item.AnimationStoryBoard != null)
                {
                    item.AnimationStoryBoard.Resume();
                }
            }

            isSimalationPause = false;
        }
        private void FastAllAnimation(double value = -1.0)
        {
            FasterCoef = value;

            if (FasterCoef == -1.0)
            {
                FasterCoef = sliderSpeed.Value;
            }

            if (!isSimalation) return;

            foreach (Route item in routes)
            {
                if (item.AnimationStoryBoard != null)
                {
                    item.AnimationStoryBoard.SetSpeedRatio(FasterCoef);
                }
            }
        }
        private void StopAllAnimation(double value = -1.0)
        {            
            if (!isSimalation) return;
            DisplayAllSectors();

            foreach (Route item in routes)
            {
                if (item.AnimationStoryBoard != null)
                {
                    item.AnimationStoryBoard.Stop();

                    WorkAreaCanvas.Children.Remove(item.AnimationObject);
                    item.AnimationObject = null;
                    item.AnimateSectorIndex = 0;
                    item.AnimationSectors = null;
                    item.AnimationStoryBoard = null;
                }
            }

            SetDefaultStateGUI();
            isSimalation = false;
        }
        private void StartNextSectorAnimate(Route item)
        {

            if (item.AnimateSectorIndex >= item.AnimationSectors.Count)
            {
                WorkAreaCanvas.Children.Remove(item.AnimationObject);
                item.AnimateSectorIndex = 0;

                if (IsAllSimulationsStoped())
                {
                    if (visiblyRoutes.IsChecked.Value)
                    {
                        DisplayAllSectors();
                    }
                    SetDefaultStateGUI();
                    isSimalation = false;
                }                

                return;
            }

            item.AnimationStoryBoard = new Storyboard();
            item.UpdateTemplate();

            ThicknessAnimation animation = new ThicknessAnimation();
            animation.From = item.AnimationObject.Margin = item.AnimationSectors[item.AnimateSectorIndex].StartPosition;
            animation.To = item.AnimationObject.Margin = item.AnimationSectors[item.AnimateSectorIndex].EndPosition;
            animation.Duration = TimeSpan.FromSeconds(item.AnimationSectors[item.AnimateSectorIndex].TimeAnimation);
            item.AnimateSectorIndex++;

            Storyboard.SetTarget(animation, item.AnimationObject);
            Storyboard.SetTargetProperty(animation, new PropertyPath(MarginProperty));

            item.AnimationStoryBoard.Children.Add(animation);

            item.AnimationStoryBoard.Completed += delegate { StartNextSectorAnimate(item); };
            item.AnimationStoryBoard.Begin();
            item.AnimationStoryBoard.SetSpeedRatio(FasterCoef);
        }

        private void HideAllSectors()
        {
            foreach (var item in WorkAreaCanvas.Children)
            {
                if (item is BasicShape)
                {
                    (item as BasicShape).Visibility = System.Windows.Visibility.Hidden;
                }

                if (item is HomeDiagramming.Connectors.ShapeConnectorBase)
                {
                    (item as HomeDiagramming.Connectors.ShapeConnectorBase).Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }
        private void DisplayAllSectors()
        {
            foreach (var item in WorkAreaCanvas.Children)
            {
                if (item is BasicShape)
                {
                    (item as BasicShape).Visibility = System.Windows.Visibility.Visible;
                }

                if (item is HomeDiagramming.Connectors.ShapeConnectorBase)
                {
                    (item as HomeDiagramming.Connectors.ShapeConnectorBase).Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        #endregion

        #region Simulation Events
        private void stopSimulation(object sender, RoutedEventArgs e)
        {
            StopAllAnimation();

            isSimalation = false;
        }
        private void startSimulator(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                FasterCoef = 1.0;
            }

            if (isSimalationPause)
            {
                FasterCoef = sliderSpeed.Value;
                ContinueAllAnimation();
                return;
            }

            if (isSimalation)
            {
                FasterCoef = 1.0;
                FastAllAnimation(FasterCoef);
                return;
            }

            if (projectMeta == null) return;

            isSimalation = true;

            if (visiblyRoutes.IsChecked.Value)
            {
                HideAllSectors();
            }


            SetSimulationStateGUI();

            foreach (Route item in routes)
            {
                if (item.Sectors.Count < 1) continue;

                item.AnimationSectors = item.GetAnimationSectors(WorkAreaCanvas);
                item.AnimateSectorIndex = 0;
                item.AnimationStoryBoard = new Storyboard();
                item.SetAnimationObject();

                WorkAreaCanvas.Children.Add(item.AnimationObject);

                StartNextSectorAnimate(item);
            }
        }
        private void simulationPause(object sender, RoutedEventArgs e)
        {
            PauseAllAnimation();

            isSimalation = true;
        }
        private void btnFast(object sender, RoutedEventArgs e)
        {
            if (!isSimalation)
            {
                FasterCoef = sliderSpeed.Value;
                startSimulator(null, null);
                isSimalation = true;
            }
            else
            {
                FastAllAnimation();
            }


        }
        #endregion

        #region Serilization
        private List<BasicShape> GetAllReestablishPivots(List<PivotSerialize> pivots)
        {
            List<BasicShape> reestablishPivots = new List<BasicShape>();

            foreach (PivotSerialize pivot in pivots)
            {
                BasicShape newPivot = new BasicShape(new TemplateDot(pivot.DotName), pivot.Position, pivot.DotId, pivot.RouteId);
                newPivot.DotType = pivot.Type;

                reestablishPivots.Add(newPivot);
            }

            return reestablishPivots;
        }
        private BasicShape GetPivotById(List<BasicShape> reestablishPivots, int id)
        {
            foreach (BasicShape item in reestablishPivots)
            {
                if (item.DotId == id)
                {
                    return item;
                }
            }

            return null;
        }

        private void RouteReestablish(RouteSerialize routeSerialize)
        {
            Route route = new Route(routeSerialize.RouteId);            
            route.RouteName = routeSerialize.RouteName;
            route.RouteComment = routeSerialize.RouteComment;

            List<BasicShape> reestablishPivots = GetAllReestablishPivots(routeSerialize.Pivots);
            
            int index = 0;
            foreach (PivotSerialize pivotSerialize in routeSerialize.Pivots)
            {                
                reestablishPivots[index].SetType(pivotSerialize.Type);

                if (pivotSerialize.NextDotId == -1)
                {
                    reestablishPivots[index].NextDot = null;
                }
                else
                {
                    reestablishPivots[index].NextDot = GetPivotById(reestablishPivots, pivotSerialize.NextDotId);
                }

                if (pivotSerialize.PrevDotId == -1)
                {
                    reestablishPivots[index].PrevDot = null;
                }
                else
                {
                    reestablishPivots[index].PrevDot = GetPivotById(reestablishPivots, pivotSerialize.PrevDotId);
                }

                route.AddPivot(reestablishPivots[index]);
                designer.AddShape(reestablishPivots[index]);

                index++;
            }

            foreach (SectorSerialize sectorSerialize in routeSerialize.Sectors)
            {
                BasicShape startPivot = GetPivotById(reestablishPivots, sectorSerialize.StartDotId);
                BasicShape endPivot = GetPivotById(reestablishPivots, sectorSerialize.EndDotId);

                HomeDiagramming.Connectors.ShapeConnectorBase sector = designer.AddConnection(endPivot, startPivot, sectorSerialize.ConnectorId, sectorSerialize.RouteId);
                sector.LineName = sectorSerialize.LineName;
                sector.Time = sectorSerialize.Time;
                sector.ConnectorId = sectorSerialize.ConnectorId;
                sector.RouteId = sectorSerialize.RouteId;
                               
                route.AddSector(sector);
            }

            routes.Add(route);
        }
        private void ProjectReestablish(Tuple<ProjectSerialize, string> data, string filePath)
        {
            routes = new List<Route>();

            NextConnectorId = data.Item1.NextConnectorId;
            NextDotId = data.Item1.NextDotId;
            NextRouteId = data.Item1.NextRouteId;
            projectMeta = new MetaInfo();
            projectMeta.ImagePath = data.Item2;
            projectMeta.ProjectPath = filePath;

            SetMapImage(data.Item2);

            foreach (RouteSerialize route in data.Item1.Routes)
            {
                RouteReestablish(route);
            }

            VisStartRoutePanel();
        }
            
        private ProjectSerialize GetProjectSerialize()
        {
            ProjectSerialize projectSerialize = new ProjectSerialize();
            projectSerialize.Routes = GetAllRouteSerialize();
            projectSerialize.NextConnectorId = NextConnectorId;
            projectSerialize.NextDotId = NextDotId;
            projectSerialize.NextRouteId = NextRouteId;

            return projectSerialize;

        }
        private List<RouteSerialize> GetAllRouteSerialize()
        {
            List<RouteSerialize> listRouteSerialize = new List<RouteSerialize>();

            foreach (Route route in routes)
	        {
		        listRouteSerialize.Add(GetRouteSerialize(route));
	        }

            return listRouteSerialize;
        }
        private RouteSerialize GetRouteSerialize(Route route)
        {
            RouteSerialize routeSerialize = new RouteSerialize();
            routeSerialize.Pivots = new List<PivotSerialize>();
            routeSerialize.Sectors = new List<SectorSerialize>();
            routeSerialize.RouteId = route.RouteId;
            routeSerialize.RouteName = route.RouteName;
            routeSerialize.RouteComment = route.RouteComment;
            
            foreach (BasicShape pivot in route.Pivots)
	        {
                int prevDotId = -1;
                int nextDotId = -1;

                if (pivot.NextDot != null)
                {
                    nextDotId = pivot.NextDot.DotId;
                }

                if (pivot.PrevDot != null)
                {
                    prevDotId = pivot.PrevDot.DotId;
                }

                Point corectPosition = new Point(pivot.GetPosition().X + pivot.DotTemplate.DotRadius / 2, pivot.GetPosition().Y + pivot.DotTemplate.DotRadius / 2);
                
                routeSerialize.Pivots.Add(new PivotSerialize(corectPosition, pivot.DotId, nextDotId, prevDotId, pivot.RouteId, pivot.DotName, pivot.DotType));
	        }

            foreach (HomeDiagramming.Connectors.ShapeConnectorBase sector in route.Sectors)
	        {
		        routeSerialize.Sectors.Add(new SectorSerialize(sector.StartDot.DotId, sector.EndDot.DotId, sector.Time, sector.LineName, sector.ConnectorId, sector.RouteId));
	        }

            return routeSerialize;
        }

        private void SerializeProject(string fileName)
        {
            ProjectSerialize projectSerialize = new ProjectSerialize();

            projectSerialize = GetProjectSerialize();

            Tuple<ProjectSerialize, string> data = new Tuple<ProjectSerialize, string>(projectSerialize, projectMeta.ImagePath);

            Stream TestFileStream = File.Create(fileName);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(TestFileStream, data);
            TestFileStream.Close();
        }
        private void DeSerializeProject(string fileName)
        {
            Tuple<ProjectSerialize, string> data;
            Stream FileStream = File.OpenRead(fileName);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            data = (Tuple<ProjectSerialize, string>)deserializer.Deserialize(FileStream);
            FileStream.Close();

            ProjectReestablish(data, fileName);
        }
        #endregion

        #region Project File Tools
        private void ClearProject()
        {
            WorkAreaCanvas.Children.Clear();
            routes = null;
            NextConnectorId = 0;
            NextDotId = 0;
            NextRouteId = 0;

            projectMeta = null;
            designer = null;            
        }
        
        private delegate bool IsNeedSavingProjectDelegat();

        private bool IsNeedSavingProject(IsNeedSavingProjectDelegat withSaving, IsNeedSavingProjectDelegat withoutSaving)
        {
            if (projectMeta != null)
            {
                MessageBoxResult mr = MessageBox.Show("Сохранить открытый проект?", "Предупреждение!",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                               
                if (mr == MessageBoxResult.Yes)
                {
                    if (withSaving()) return true;                                        
                }

                if (mr == MessageBoxResult.No)
                {
                    if (withoutSaving()) return true;
                }                               
            }
            else
            {
                if (withoutSaving()) return true;
            }

            return false;
        }

        private bool CreateNewProject(string projectName)
        {
            try
            {
                ClearProject();

                projectMeta = new MetaInfo();
                projectMeta.ProjectPath = projectName;

                designer = new DiagramDesigner(WorkAreaCanvas);
                routes = new List<Route>();
                startProject.Visibility = System.Windows.Visibility.Hidden;

                status.Content = "Проект создан";

                return true;
            }
            catch
            {
                status.Content = "Проект не был создан";

                return false;
            }
        }
        private bool CreateProjectWithSaving()
        {
            if (SaveProject())
            {
                if (CreateProjectWithoutSaving())
                {
                    return true;
                }                
            }

            return false;
        }
        private bool CreateProjectWithoutSaving()
        {
            Microsoft.Win32.SaveFileDialog createDialog = new Microsoft.Win32.SaveFileDialog();

            createDialog.InitialDirectory = "";
            createDialog.Filter = "mps (*.mps*)|*.mps";
            createDialog.FilterIndex = 1;
            createDialog.RestoreDirectory = true;

            if (createDialog.ShowDialog() != null)
            {
                try
                {
                    if (createDialog.FileName == "" || createDialog.FileName == null) return false;
                    if (CreateNewProject(createDialog.FileName)) return true;

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании проекта! " + ex.Message);
                }
            }

            return false;
        }
        
        private bool CloseProjectWithSaving()
        {
            if (SaveProject())
            {
                if (CloseProjectWithoutSaving())
                {
                    return true;
                }
            }

            return false;
        }
        private bool CloseProjectWithoutSaving()
        {
            try
            {
                ClearProject();

                startProject.Visibility = System.Windows.Visibility.Visible;
                HideStartPage();

                status.Content = "Проект закрыт";
                return true;
            }
            catch
            {
                status.Content = "Проект не удалось закрыть";
                return false;
            }
        }
        
        private bool CloseProgramWithSaving()
        {
            if (SaveProject())
            {
                if (CloseProgramWithoutSaving())
                {
                    return true;
                }
            }

            return false;
        }
        private bool CloseProgramWithoutSaving()
        {
            try
            {
                ClearProject();

                startProject.Visibility = System.Windows.Visibility.Visible;
                HideStartPage();

                return true;
            }
            catch
            {
                status.Content = "Проект не удалось закрыть";
                return false;
            }
        }
               
        private bool OpenProject(string projectName)
        {
            try
            {
                ClearProject();

                projectMeta = new MetaInfo();
                projectMeta.ProjectPath = projectName;

                designer = new DiagramDesigner(WorkAreaCanvas);
                routes = new List<Route>();
                DeSerializeProject(projectName);

                startProject.Visibility = System.Windows.Visibility.Hidden;
                HideStartPage();

                status.Content = "Проект \"" + projectName + "\" открыт";

                return true;
            }
            catch
            {
                status.Content = "Проект не был открыт";
                return false;
            }
        }
        private bool OpenProjectWithSaving()
        {
            if (SaveProject())
            {
                if (OpenProjectWithoutSaving())
                {
                    return true;
                }
            }

            return false;
        }
        private bool OpenProjectWithoutSaving()
        {
            Stream myStream = null;
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();

            openDialog.InitialDirectory = "";
            openDialog.Filter = "mps (*.mps*)|*.mps";
            openDialog.FilterIndex = 2;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() != null)
            {
                try
                {
                    if (openDialog.FileName == "" || openDialog.FileName == null) return false;

                    if ((myStream = openDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            if (OpenProject(openDialog.FileName)) return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Файл не найден! " + ex.Message);
                }
            }
            return false;
        }

        private bool SaveProject()
        {
            try
            {
                if (projectMeta != null)
                {
                    SerializeProject(projectMeta.ProjectPath);

                    status.Content = "Проект сохранен";
                    return true;
                }

                return false;                
            }
            catch
            {
                status.Content = "Проект не был сохранен";
                return false;
            }
        } 

        private bool SaveAsProject()
        {
            try
            {
                System.Windows.Forms.SaveFileDialog directoryDialog = new System.Windows.Forms.SaveFileDialog();
                directoryDialog.InitialDirectory = "";
                directoryDialog.Filter = "mps (*.mps*)|*.mps";
                directoryDialog.FilterIndex = 1;
                directoryDialog.RestoreDirectory = true;
                
                directoryDialog.ShowDialog();

                if (directoryDialog.FileName != "" && directoryDialog.FileName != null)
                {
                    SerializeProject(directoryDialog.FileName);

                    status.Content = "Проект сохранен";
                    return true;
                }
            }
            catch
            {
                status.Content = "Проект не был сохранен";                
            }

            return false;
        }
    

        private void createProject(object sender, RoutedEventArgs e)
        {
            IsNeedSavingProject(CreateProjectWithSaving, CreateProjectWithoutSaving);
        }
        private void openProject(object sender, RoutedEventArgs e)
        {
            IsNeedSavingProject(OpenProjectWithSaving, OpenProjectWithoutSaving);
        }        
        private void saveProject(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }
        private void saveAsProject(object sender, RoutedEventArgs e)
        {
            SaveAsProject();
        }
        private void closeProject(object sender, RoutedEventArgs e)
        {
            IsNeedSavingProject(CloseProjectWithSaving, CloseProjectWithoutSaving);
        }
        private void closeProgram(object sender, RoutedEventArgs e)
        {
            if (IsNeedSavingProject(CloseProgramWithSaving, CloseProgramWithoutSaving))
            {
                this.Close();
            }
        }
        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {            
            if (IsNeedSavingProject(CloseProgramWithSaving, CloseProgramWithoutSaving))
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

    }
}

using System.Windows;

namespace HomeDiagramming
{
    public interface IDraggable
    {
        bool CanDrag { get; }
        void SetPosition(Point value);
        Point GetPosition();
    }
}

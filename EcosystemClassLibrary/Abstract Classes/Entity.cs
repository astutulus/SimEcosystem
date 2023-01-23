using System.Drawing;

namespace EcosystemClassLibrary;

public abstract class Entity : IComparable
{
    /* 
     * Fields
     */
    private Point _position;



    /*
     * Properties
     */

    public Point Position { get => _position; set => _position = value; }

    public int PositionX
    {
        get
        {
            return _position.X;
        }
        set
        {
            int minX = World.Instance.Extents.X;
            int maxX = World.Instance.Extents.X + World.Instance.Extents.Width;

            if (value < minX) value = minX;
            if (value > maxX) value = maxX;

            _position.X = value;
        }
    }

    public int PositionY
    {
        get
        {
            return _position.Y;
        }
        set
        {
            int minY = World.Instance.Extents.Y;
            int maxY = World.Instance.Extents.Y + World.Instance.Extents.Height;

            if (value < minY) value = minY;
            if (value > maxY) value = maxY;

            _position.Y = value;
        }
    }



    /*
     * Constructors
     */

    public Entity(Point position = default)
    {
        Position = position;
        World.Instance.CreateEntity(this);
    }



    /*
     * Methods
     */

    /*
     * pt   -   where to go
     * dist -   step size
     * stop -   this distance away
     * 
     * returns true when arrived
     */
    public void MoveTowardsPoint(Point pt, double dist)
    {
        if (GetDistanceToPoint(pt) > dist)
        {
            double adjacent = pt.X - Position.X;
            double opposite = pt.Y - Position.Y;

            double angle = Math.Atan(opposite / adjacent);
            if (Position.X > pt.X)
            {
                angle += Math.PI;
            }

            double dx = dist * Math.Cos(angle);
            double dy = dist * Math.Sin(angle);

            Position = new(Position.X + (int)dx, Position.Y + (int)dy);
        }
    }


    public int CompareTo(object? obj)
    {
        return 0;
    }


    // Calculation methods

    public double GetDistanceToPoint(Point other)
    {
        double xDiff = Math.Abs(Position.X - other.X);
        double yDiff = Math.Abs(Position.Y - other.Y);
        return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
    }


}

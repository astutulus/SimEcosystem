using System.Drawing;

namespace EcosystemClassLibrary;

public class World
{
    /*
     * Fields
     */
    private HashSet<Entity> _entities = new();


    /*
     * Properties
     */

    /// <summary>
    /// All entities in the world
    /// </summary>
    public HashSet<Entity> Entities { get => _entities; set => _entities = value; }

    /*
     * Properties withouf Fields and which are implictly "readonly" (set by constructor)
     */
    public string WorldName { get; }

    public int MinX { get; }
    public int MinY { get; }
    public int MaxX { get; }
    public int MaxY { get; }

    public int SpriteDia { get; }



    /*
     * Constructors
     */
    public World(string worldName, Rectangle extents, int spriteDiameter)
    {
        WorldName = worldName;

        MinX = extents.X;
        MinY = extents.Y;

        MaxX = MinX + extents.Width;
        MaxY = MinY + extents.Height;

        SpriteDia = spriteDiameter;
    }



    /*
     * Methods
     */
    public void CreateEntity(Entity e)
    {
        Entities.Add(e);
    }

    public void SmiteEntity(Entity e)
    {
        Entities.Remove(e);
    }

    /// <summary>
    /// Returns all Entities within a specified ratius of a given Point
    /// including any entity at the centre of the search
    /// </summary>
    /// <param name="centre"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public HashSet<Entity> GetEntitiesWithinRadiusOfPoint(Point centre, double radius)
    {
        HashSet<Entity> results = new();
        foreach (Entity e in Entities)
        {
            if (e.GetDistanceToPoint(centre) < radius)
            {
                results.Add(e);
            }
        }
        return results;
    }

    /*
     * Shortcut alias for GetNearestEntityToPointFromSet()
     * 
     * More idiomatic when interrogating whole world.
     * Animals can't do that, but "God" functions of the GUI can.
     */
    public static Entity GetNearestEntityToPointFromWorld(Point centre, World world)
    {
        return GetNearestEntityToPointFromSet(centre, world.Entities);
    }

    /// <summary>
    /// Given a set of Entities, return the Entity closest to a specified Point.
    /// This is useful because the absolute nearest might not be the one needed.
    /// e.g. A rabbit looking for grass ignores rabbit closer than grass.
    /// </summary>
    /// <param name="centre"></param>
    /// <param name="set"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Entity GetNearestEntityToPointFromSet(Point centre, HashSet<Entity> set)
    {
        if (set.Count == 0) return null;

        Entity nearest;
        try
        {
            nearest = set.First();
        }
        catch
        {
            throw new Exception("No element found");
        }

        double distanceToNearest = double.MaxValue;

        foreach (Entity entity in set)
        {
            double dist = entity.GetDistanceToPoint(centre);
            if (dist < distanceToNearest)
            {
                distanceToNearest = dist;
                nearest = entity;
            }
        }
        return nearest;
    }



    /// <summary>
    /// 'Meta' info for initial output of what is happening in the sim
    /// </summary>
    public void Census()
    {
        int ct = Entities.Count;
        Console.WriteLine("World contains {0} entities{1}", ct, (ct > 0 ? ": " : ". "));
        foreach (Entity e in Entities)
            Console.WriteLine(" - {0}", e);
        Console.WriteLine(); // Blank row after ToString.
    }
}

using System.Drawing;

namespace EcosystemClassLibrary;

public class Grass : Plant
{
    private static int _count = 0;
    public static int Count { get => _count; set => _count = value; }

    public Grass(Point position) : base(position, Constants.kTypMassGrass, Constants.kLifespanGrass)
    {
        Species = ESpecies.grass;
        Count++;
    }
    public override void PassAway()
    {
        Count--;
    }

}
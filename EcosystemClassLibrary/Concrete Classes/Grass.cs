using System.Drawing;

namespace EcosystemClassLibrary;

public class Grass : Plant
{
    public Grass(World world, Point position) : base(world, position, Constants.kTypMassGrass, Constants.kLifespanGrass)
    {
        Species = ESpecies.grass;
        SpeciesTally++;
    }
}
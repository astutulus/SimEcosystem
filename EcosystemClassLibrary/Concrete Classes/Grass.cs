using System.Drawing;

namespace EcosystemClassLibrary;

public class Grass : Plant
{
    public Grass(Point position) : base(position, Constants.kTypMassGrass, Constants.kLifespanGrass)
    {
        Species = ESpecies.grass;
        SpeciesTally++;
    }
}
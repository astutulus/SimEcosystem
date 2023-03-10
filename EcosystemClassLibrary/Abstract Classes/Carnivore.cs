using System.Drawing;

namespace EcosystemClassLibrary;

public abstract class Carnivore : Animal
{
    /* Fields */
    private HashSet<ESpecies> _preySpecies;

    /* Properties */
    protected HashSet<ESpecies> PreySpecies { get => _preySpecies; set => _preySpecies = value; }


    /* Constructors */
    protected Carnivore(Point position, double typMass, TimeSpan lifespan, HashSet<ESpecies> prey) 
        : base(position, typMass, lifespan)
    { 
        PreySpecies = prey;
    }


    /* Methods */
    protected override List<LivingThing> LookForFood()
    {
        return LookForSpeciesOfInterest(PreySpecies);
    }

}

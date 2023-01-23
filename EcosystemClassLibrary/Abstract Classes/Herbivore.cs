using System.Collections.Generic;
using System.Drawing;

namespace EcosystemClassLibrary;

public abstract class Herbivore : Animal
{
    /* Fields */
    private HashSet<ESpecies> _foodSpecies = new();


    /* Properties */
    protected HashSet<ESpecies> FoodSpecies { get => _foodSpecies; set => _foodSpecies = value; }



    /* Constructors */
    protected Herbivore(Point position, double typMass, TimeSpan lifespan, HashSet<ESpecies> food) : 
        base(position, typMass, lifespan) 
    {
        FoodSpecies = food;
    }



    /* Methods */
    protected override List<LivingThing> LookForFood()
    {
        return LookForSpeciesOfInterest(FoodSpecies);
    }

}

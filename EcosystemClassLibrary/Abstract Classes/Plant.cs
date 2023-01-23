using System.Drawing;

namespace EcosystemClassLibrary;

public abstract class Plant : LivingThing
{
    /* Constructors */
    protected Plant(Point position, double typMass, TimeSpan lifespan) :
        base(position, typMass, lifespan) { }


    /* Methods */
    protected override void Behaviour()
    {
        // Grow(); To add after adding "eating"
    } 

    protected void Grow()
    {
        if (Mass < TypMass)
        {
            Mass += Constants.kSizeOfPlantGrowth;
        }
    }
}
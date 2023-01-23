using System.Drawing;

namespace EcosystemClassLibrary;

public abstract class Animal : LivingThing
{
    /* Fields */

    private HashSet<Animal> _predatorSpecies = new();

    private double _stepSize;
    private double _eyesight;
    private double _energyLevel;



    /* Properties */
    internal HashSet<Animal> PredatorSpecies { get => _predatorSpecies; }

    protected double StepSize { get => _stepSize; set => _stepSize = value; }
    protected double Eyesight { get => _eyesight; set => _eyesight = value; }
    protected double EnergyPercent { get => _energyLevel; set => _energyLevel = value; }
    
    protected EActivity Activity { get; set; }



    /* Constructors */
    protected Animal(Point position, double typMass, TimeSpan lifespan) :
        base(position, typMass, lifespan)
    {
        Activity = EActivity.resting;
    }



    /* Methods */

    protected override void Behaviour()
    {
        if (IsNextToFood())
        {
            Eat();
        }
        else
        {
            SeekFood();
        }
    }

    private bool IsNextToFood()
    {
        return false;
    }

    private void Eat()
    {
        Activity = EActivity.eating;
    }

    private void SeekFood()
    {
        List<LivingThing> SeenFood = LookForFood();
        if (SeenFood.Any()) // tests if not empty
        {
            LivingThing food = SeenFood.First();

            double touching = Mass / 2 + food.Mass / 2;

            if (GetDistanceToPoint(food.Position) > touching)
            {
                MoveTowardsPoint(food.Position, StepSize);
                Activity = EActivity.goingToFood;
            }
        }
        else
        {
            Nap();
            Activity = EActivity.resting;
        }
    }

    protected HashSet<Animal> GetPredatorsSeen()
    {
        return new HashSet<Animal>();
        //LookFor(PredatorSpecies);
    }

    protected abstract List<LivingThing> LookForFood();

    /*
     * Simulates being able to see a certain distance,
     * looking out for individuals of another species,
     * be they predator, prey, or food.
     * 
     * Should returns results in order of distance for ease of prioritisation.
     * Actually returns just the nearest
     * 
     * Returns an empty List (not, null) if nothing of interest within eyesight.
     */
    public List<LivingThing> LookForSpeciesOfInterest(HashSet<ESpecies> speciesOfInterest)
    {
        HashSet<Entity> found = new();
        foreach (Entity seen in World.Instance.GetEntitiesWithinRadiusOfPoint(Position, Eyesight))
        {
            if (seen is LivingThing thing && speciesOfInterest.Contains(thing.Species)) // Pattern matching
            {
                found.Add(seen);
            }
        }

        LivingThing? nearestYet = null;
        double shortestDist = double.MaxValue;
        foreach (LivingThing thing in found)
        {
            double dist = thing.GetDistanceToPoint(Position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                nearestYet = thing;
            }
        }

        List<LivingThing> result = new();
        if (nearestYet != null)
        {
            result.Add(nearestYet);
        }
        return result;
    }

    protected void Eat(LivingThing food)
    {
        if (food.Mass > Constants.kSizeOfBite)
        {
            Mass += Constants.kSizeOfBite;
            food.Mass -= Constants.kSizeOfBite;
        }
        else // eat it all
        {
            Mass += food.Mass;
            food.Death();
        }
    }


    public override string ToString()
    {
        return base.ToString() + " -> " + Activity;
    }



    /* -----------------  ANIMAL BEHAVIOUR METHODS  ---------------- 

    public void consumeFood(LivingThing lt)
    {
        lt.SetMass(lt.getMass() - 1);
        this.SetMass(this.getMass() + 1);
    }

    // The less heavy, the more hungry
    public int hungerPercent()
    {
        return (int)(this.getTypMass() - this.getMass());// Math.abs(this.energyLevel - 100);
    }


    // The more predators known about, the more fear felt.
    // Ten is as bad as anything more than ten.
    public int fearPercent()
    {
        return this.predatorsSeen.size();
    }

    public void solveHunger()
    {
        if (getFoodSeen().size() > 0)
        {
            LivingThing nearestFood = nearestFood();

            moveTowards(nearestFood, false);
            if (at(nearestFood))
            {
                consumeFood(nearestFood);
            }
        }
    }

    // Not sophisticated -- need to make good for circles not squares
    public boolean at(LivingThing lt)
    {
        double xDiff = (lt.getX() - this.getX());
        double yDiff = (lt.getY() - this.getY());

        return (
              (Math.abs(xDiff) < ((this.getMass() / 2) + (lt.getMass() / 2)))
              &&
              (Math.abs(yDiff) < ((this.getMass() / 2) + (lt.getMass() / 2))));
    }

    public void solveFear()
    {
        if (getPredatorsSeen().size() > 0)
        {
            moveTowards(nearestPredator(), true);
        }
    }


    #region--  THREADING METHODS -

    // Will go towards food or evade predators,
    // depending on which is most important.
    public void run()
    {
        while (age() < getLifespan())
        {
            if (hungerPercent() > fearPercent())
            {
                solveHunger();
            }
            else
            {
                solveFear();
            }
            // TO DO  grow; reproduce
        }
        Death();
    }

    */
}

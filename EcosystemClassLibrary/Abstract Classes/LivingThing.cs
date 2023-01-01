using System.Drawing;
using System.Text;

namespace EcosystemClassLibrary;

public abstract class LivingThing : Entity
{

    /*
     * Class Fields
     */

    private static Dictionary<ESpecies, int> _speciesTally = new()
    {
        { ESpecies.grass  , 0 },
        { ESpecies.rabbit , 0 },
        { ESpecies.fox    , 0 }
    };



    /*
     * Instance Fields
     */
    private double _mass;
    private double _typMass;

    private TimeSpan _lifespan;
    private DateTime _birthday;


    /*
     * Properties
     */
    internal double Mass { get => _mass; set => _mass = value; }
    protected double TypMass { get => _typMass; private set => _typMass = value; }

    protected TimeSpan Lifespan { get => _lifespan; private set => _lifespan = value; }
    protected DateTime Birthday { get => _birthday; private set => _birthday = value; }
    protected TimeSpan Age { get => DateTime.Now - Birthday; }

    public bool IsAlive { get; set; }

    public ESpecies Species { get; set; }

    // Wow, the following automatically selects the Species of this LivingThing
    public int SpeciesTally { get => _speciesTally[Species]; set => _speciesTally[Species] = value; }



    /* Constructors */
    public LivingThing(World world, Point position, double typMass, TimeSpan lifespan) : base(world, position)
    {
        Mass = typMass * Constants.kStartMassMultiple;
        TypMass = typMass;

        Lifespan = lifespan;
        Birthday = DateTime.Now;

        Thread life = new(LifeStory);
        life.Start();
    }

    ~LivingThing()
    {
        SpeciesTally--;
    }


    /* Methods */

    private void LifeStory()
    {
        Birth();
        while (IsAlive && (Age < Lifespan))  // May die of other causes before old ago
        {
            Behaviour();
            Thread.Sleep(1000 / Constants.kSimFreqHz);
        }
        Death();
    }

    protected void Birth()
    {
        HostWorld.CreateEntity(this);
        IsAlive = true;
    }

    protected abstract void Behaviour();

    /*
     * Take a break before carrying on
     */
    protected void Nap()
    {
        Thread.Sleep(Constants.kNapTimeMs);
    }

    internal void Death()
    {
        IsAlive = false;
        Thread.Sleep(Constants.kDecayTime);
        HostWorld.SmiteEntity(this);
    }

    public static int GetSpeciesCount(ESpecies species)
    {
        return _speciesTally[species];
    }

    public override string ToString()
    {
        StringBuilder info = new(GetType().Name);

        info.Append(" (mass " + Math.Round(Mass, 2));
        info.Append(" / " + TypMass + ")");

        info.Append(" (age " + Math.Round(Age.TotalMilliseconds, 0));
        info.Append(" / " + Lifespan.TotalMilliseconds + ")");

        info.Append(IsAlive ? " Alive " : " Dead ");

        info.Append("\nPosition X: " + Position.X);
        info.Append("\nPosition Y: " + Position.Y);

        return info.ToString();
    }
}

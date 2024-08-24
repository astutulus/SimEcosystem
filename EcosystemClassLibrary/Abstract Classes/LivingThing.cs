using System.Drawing;
using System.Text;

namespace EcosystemClassLibrary;

public abstract class LivingThing : Entity
{
    private double _mass;
    private double _typMass;

    private TimeSpan _lifespan;
    private DateTime _birthday;


    /*
     * Properties
     */
    public double Mass { get => _mass; set => _mass = value; }
    protected double TypMass { get => _typMass; private set => _typMass = value; }

    protected TimeSpan Lifespan { get => _lifespan; private set => _lifespan = value; }
    protected DateTime Birthday { get => _birthday; private set => _birthday = value; }
    protected TimeSpan Age { get => DateTime.Now - Birthday; }

    public bool IsAlive { get; set; }

    public ESpecies Species { get; set; }

    /* Constructors */
    public LivingThing(Point position, double typMass, TimeSpan lifespan) : base(position)
    {
        Mass = typMass * Constants.kStartMassMultiple;
        TypMass = typMass;

        Lifespan = lifespan;
        Birthday = DateTime.Now;

        Thread life = new(LifeStory);
        life.Start();
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
        if (World.Instance != null)
        {
            World.Instance.CreateEntity(this);
            IsAlive = true;
        }
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
        // Null propagation (terse null check)
        this.PassAway();
        World.Instance?.SmiteEntity(this);
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
    public abstract void PassAway();
}

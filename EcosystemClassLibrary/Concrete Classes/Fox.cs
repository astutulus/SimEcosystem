using System.Drawing;

namespace EcosystemClassLibrary;

public class Fox : Carnivore
{
    private static int _count = 0;
    public static int Count {  get => _count; set => _count = value; }

    public Fox(Point position) : 
        base(position, Constants.kTypMassFox, Constants.kLifespanFox, Constants.kFoodOfFox)
    {
        Species = ESpecies.fox;
        Count++;

        StepSize = Constants.kStepSizeFox;
        Eyesight = Constants.kEyesightFox;
        EnergyPercent = Constants.kStartEnergyPercent;
    }
    public override void PassAway()
    {
        Count--;
    }

}

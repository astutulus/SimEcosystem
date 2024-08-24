using System;
using System.Drawing;
using System.Security.Principal;
namespace EcosystemClassLibrary;

public class Rabbit : Herbivore
{
    private static int _count = 0;
    public static int Count { get => _count; set => _count = value; }

    public Rabbit(Point position) : 
        base(position, Constants.kTypMassRabbit, Constants.kLifespanRabbit, Constants.kFoodOfRabbit)
    {
        Species = ESpecies.rabbit;
        Count++;
        StepSize = Constants.kStepSizeRabbit;
        Eyesight = Constants.kEyesightRabbit;
        EnergyPercent = Constants.kStartEnergyPercent;
    }

    public override void PassAway()
    {
        Count--;
    }

}
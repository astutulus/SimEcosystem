using System.Drawing;

namespace EcosystemClassLibrary;

public class Rabbit : Herbivore
{
    public Rabbit(Point position) : 
        base(position, Constants.kTypMassRabbit, Constants.kLifespanRabbit, Constants.kFoodOfRabbit)
    {
        Species = ESpecies.rabbit;
        SpeciesTally++;

        StepSize = Constants.kStepSizeRabbit;
        Eyesight = Constants.kEyesightRabbit;
        EnergyPercent = Constants.kStartEnergyPercent;
    }
}
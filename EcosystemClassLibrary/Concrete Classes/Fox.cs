using System.Drawing;

namespace EcosystemClassLibrary;

public class Fox : Carnivore
{
    public Fox(World world, Point position) : 
        base(world, position, Constants.kTypMassFox, Constants.kLifespanFox, Constants.kFoodOfFox)
    {
        Species = ESpecies.fox;
        SpeciesTally++;       

        StepSize = Constants.kStepSizeFox;
        Eyesight = Constants.kEyesightFox;
        EnergyPercent = Constants.kStartEnergyPercent;
    }
}

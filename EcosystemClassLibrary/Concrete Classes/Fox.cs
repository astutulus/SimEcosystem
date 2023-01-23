using System.Drawing;

namespace EcosystemClassLibrary;

public class Fox : Carnivore
{
    public Fox(Point position) : 
        base(position, Constants.kTypMassFox, Constants.kLifespanFox, Constants.kFoodOfFox)
    {
        Species = ESpecies.fox;
        SpeciesTally++;       

        StepSize = Constants.kStepSizeFox;
        Eyesight = Constants.kEyesightFox;
        EnergyPercent = Constants.kStartEnergyPercent;
    }
}

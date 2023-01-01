namespace EcosystemClassLibrary;

internal class Constants
{
    /*
     * Speed (Hz) of simulator
     */
    internal static int kSimFreqHz = 20;


    /*
     * Distance travelled (pixels)
     */
    internal static double kStepSizeFox = 4.0d;
    internal static double kStepSizeRabbit = 1.8d;


    /*
     * Time (days, hours, mins, seconds)
     */
    internal static TimeSpan kLifespanFox = new(0, 0, 0, 50);
    internal static TimeSpan kLifespanRabbit = new(0, 0, 0, 30);
    internal static TimeSpan kLifespanGrass = new(0, 0, 0, 10);

    internal static TimeSpan kDecayTime = new(0, 0, 0, 1);

    internal static int kNapTimeMs = 1000;


    #region Mass

    /*
     * Mass: when fully grown
     */
    internal static double kTypMassFox = 10d;
    internal static double kTypMassRabbit = 4d;
    internal static double kTypMassGrass = 2d;

    /*
     * Mass: proportion of maximum when spawning new life
     */
    internal static double kStartMassMultiple = 0.5;

    /*
     * Mass: Ammount transferred when eating
     */
    internal static double kSizeOfBite = 0.1;
    internal static double kSizeOfPlantGrowth = 0.1;

    #endregion

    /*
     * Food (ESpecies) that each animal seeks
     */
    internal static HashSet<ESpecies> kFoodOfFox = new() { ESpecies.rabbit };
    internal static HashSet<ESpecies> kFoodOfRabbit = new() { ESpecies.grass };

    /*
     * Predator (ESpecies) that each animal flees
     */
    internal static HashSet<ESpecies> kPredatorOfFox = new() { };
    internal static HashSet<ESpecies> kPredatorOfRabbit = new() { ESpecies.fox };

    /*
     * Eyesight (pixels) for seeing food and predators
     */
    internal static int kEyesightFox = 400;
    internal static int kEyesightRabbit = 460;

    /*
     * Energy (Percent) - Not yet clear how used
     */
    internal static double kStartEnergyPercent = 100.0;
}

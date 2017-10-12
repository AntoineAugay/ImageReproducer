

namespace GeneticAlgorithm
{
    public enum GASelectionMethod{
        WheelSelection,
        TournamentSelection
    }


    public class GAGeneralParams
    {
        public int PopulationSize;
        public int AdnSize;
        public int GeneSize;
        public int NumberOfGeneration;
    }

    public class GASelectionParams
    {
        public double PartOfPopulationSelected;
        public bool KeepBest;
        public GASelectionMethod SelectionMethod;
        public int TournamentSize;
        public int NumberOfWinners;
    }

    public class GAReproductionParams
    {
        public double ReproductionProbability;
    }

    public class GAMutationParams
    {
        public double MutationProbability;
    }

    public class GAParameters
    {
        public GAGeneralParams General;
        public GASelectionParams Selection;
        public GAReproductionParams Reproduction;
        public GAMutationParams Mutation;
        public bool AddNewRandomInd;
        public int NumberOfNewRandomInds;

        
        public GAParameters()
        {
            General = new GAGeneralParams();
            Selection = new GASelectionParams();
            Reproduction = new GAReproductionParams();
            Mutation = new GAMutationParams();
        }
        
    }
    
}

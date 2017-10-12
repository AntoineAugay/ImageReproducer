using System.Linq;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    class GAGenerationManager
    {

        private GAParameters Params;
        private IGAIndividuEvaluator Evaluator;
        private IGASelector Selector;
        readonly GAPopulation Population;
        public int GenerationNumber { get; private set; }

        public GAGenerationManager(GAParameters Params, IGAIndividuEvaluator Evaluator)
        {
            this.Params = Params;
            this.Evaluator = Evaluator;

            switch (Params.Selection.SelectionMethod) {
                case GASelectionMethod.WheelSelection:
                    Selector = new GASelectorWheelMethod(Params.Selection.KeepBest);
                break;
                case GASelectionMethod.TournamentSelection:
                    Selector = new GASelectorTournamentMethod(Params.Selection.KeepBest, Params.Selection.TournamentSize, Params.Selection.NumberOfWinners);
                    break;

            }
            GenerationNumber = 0;
            Population = new GAPopulation(Selector, Params.General.PopulationSize, Params.General.AdnSize, Params.General.GeneSize);
        }

        public void NextGeneration()
        {
            if (GenerationNumber == 0)
            {
                Population.GenerateRandomPopuluation();
                EvaluatePopulation();
                GenerationNumber++;
            }
            else
            {
                Population.SelectionForReproduction(Params.Selection.KeepBest, Params.Selection.PartOfPopulationSelected);
                Population.Reproduce(Params.Reproduction.ReproductionProbability);
                Population.Mutate(Params.Mutation.MutationProbability);
                EvaluatePopulation();
                Population.SelectionForNextGeneration(Params.Selection.KeepBest);
                GenerationNumber++;
            }
            System.Console.WriteLine("Best fitness : {0}", Population.BestInd.Fitness);
        }

        private void EvaluatePopulation()
        {
            Population.BestInd = Population.ListInd[0];
            
            Population.ListInd.ForEach(Ind => {
                Ind.Fitness = Evaluator.EvaluateIndividu(Ind);
                if (Ind.Fitness > Population.BestInd.Fitness)
                {
                    Population.BestInd = Ind;
                }
            });
        }

        public void SaveBestInd()
        {
            Evaluator.Save(Population.BestInd);
        }

        public GAIndividu GetBestInd()
        {
            return Population.BestInd;
        }
    }
}

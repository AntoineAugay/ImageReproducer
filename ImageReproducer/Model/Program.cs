using System.Diagnostics;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            GAParameters Params = new GAParameters();

            Params.PopulationSize = 100;
            Params.AdnSize = 60;
            Params.GeneSize = 2;

            Params.SelectionParameter = GASelectionParameter.WheelSelection;
            Params.PartOfPopulationSelected = 0.4;
            Params.ReproductionProbability = 0.5;
            Params.MutationProbability = 0.05;
            Params.AddNewRandomInd = false;

            GAEvaluatorImageReproducer Evaluator = new GAEvaluatorImageReproducer("target.bmp");
            GAGenerationManager Manager = new GAGenerationManager(Params, Evaluator);

            int n = 0;
            Stopwatch timer = new Stopwatch();
            while (++n < 2000)
            {
                System.Console.WriteLine("----------------------------");
                System.Console.WriteLine("Génération {0}", n);
                timer.Restart();
                timer.Start();
                Manager.NextGeneration();
                timer.Stop();
                System.Console.WriteLine("Elapse Time : {0}", timer.ElapsedMilliseconds);
            }
            Manager.SaveBestInd();

        }
    }
}

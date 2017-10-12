
namespace GeneticAlgorithm
{
    interface IGAIndividuEvaluator
    {
        int EvaluateIndividu(GAIndividu Ind);
        void Save(GAIndividu Ind);
    }
}

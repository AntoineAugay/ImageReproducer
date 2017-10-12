using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GASelectorTournamentMethod : IGASelector
    {
        private int NumberOfGladiators = 2;
        private int NumberOfWinners = 1;
        private bool KeepBest = true;



        public GASelectorTournamentMethod(bool KeepBest, int NumberOfGladiators, int NumberOfWinners)
        {
            if (NumberOfGladiators <= NumberOfWinners)
                throw new System.ArgumentException("The size of a tournament as to be greater than the number of winner");
            this.NumberOfGladiators = NumberOfGladiators;
            this.NumberOfWinners = NumberOfWinners;
            this.KeepBest = KeepBest;
        }

        public List<GAIndividu> Select(List<GAIndividu> List, GAIndividu Best, double SelectedPartOfPop)
        {
            int SizeNewList = (int)(List.Count * SelectedPartOfPop);
            List<GAIndividu> NewSelectedInds = new List<GAIndividu>(SizeNewList);
            
            if (KeepBest)
            {
                NewSelectedInds.Add(Best);
            }

            for (int i = 0; NewSelectedInds.Count < SizeNewList; i += NumberOfWinners)
            {
                List<GAIndividu> Winners = Tournament(List);
                for (int j = 0; j < Winners.Count; j++)
                {
                    if (NewSelectedInds.Count < SizeNewList)
                    {
                        NewSelectedInds.Add(Winners[j].Clone());
                    }
                }
            }

            return NewSelectedInds;
        }

        private List<GAIndividu> Tournament(List<GAIndividu> List)
        {
            Random Rand = new Random(Guid.NewGuid().GetHashCode());
            List<GAIndividu> Gladiators = new List<GAIndividu>(NumberOfGladiators);
            while (Gladiators.Count < NumberOfGladiators)
            {
                bool Different = true;
                GAIndividu NewGladiator = List[Rand.Next(0, List.Count)];

                Gladiators.ForEach(Gladiator =>
                {
                    if (Gladiator == NewGladiator)
                    {
                        Different = false;
                    }
                });

                if (Different)
                {
                    Gladiators.Add(NewGladiator);
                }
            }
            List<GAIndividu> SortedGladiators = Gladiators.OrderByDescending(g => g.Fitness).ToList();
            return SortedGladiators.GetRange(0, NumberOfWinners);
        }

    }
}

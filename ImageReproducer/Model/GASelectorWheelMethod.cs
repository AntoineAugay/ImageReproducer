using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GASelectorWheelMethod : IGASelector
    {

        private readonly bool KeepBest;
        
        public GASelectorWheelMethod(bool KeepBest)
        {
            this.KeepBest = KeepBest;
        }

        public List<GAIndividu> Select(List<GAIndividu> List, GAIndividu Best, double SelectedPartOfPop)
        {
            List<GAIndividu> NewSelectedInds = new List<GAIndividu>((int)(List.Count * SelectedPartOfPop)); ;
            int SizeNewList = (int)(List.Count * SelectedPartOfPop);

            if (KeepBest)
            {
                SizeNewList--;
                NewSelectedInds.Add(Best);
            }
            
            int[] CumulativeWeights = new int[List.Count];
            int CurrentSum = 0;

            // Built the cumulative wheight array
            for (int i = 0; i < List.Count; i++)
            {
                CurrentSum += List[i].Fitness;
                CumulativeWeights[i] = CurrentSum;
            }

            // Selection of individus based on their fitness 
            for (int i = 0; i < SizeNewList; i++)
            {
                int index = WheightedSelection(CumulativeWeights);
                if (index < List.Count)
                    NewSelectedInds.Add(List[index].Clone());
            }

            return NewSelectedInds;
        }

        private int WheightedSelection(int[] CumulWeights)
        {
            Random Rand = new Random(Guid.NewGuid().GetHashCode());

            int index = Array.BinarySearch(CumulWeights, Rand.Next(0, CumulWeights[CumulWeights.Length - 1]));
            if (index < 0)
            {
                index = ~index;
            }
            return index;
        }
    }
}

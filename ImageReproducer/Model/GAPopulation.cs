using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GAPopulation
    {
        private int PopulationSize;
        private int AdnSize;
        private int GeneSize;
        private List<GAIndividu> SelectedInds;
        private IGASelector Selector;
        public List<GAIndividu> ListInd { get; set; }
        public GAIndividu BestInd { get; set; }


        public GAPopulation(IGASelector Selector, int PopulationSize, int AdnSize, int GeneSize)
        {
            this.AdnSize = AdnSize;
            this.GeneSize = GeneSize;
            this.PopulationSize = PopulationSize;
            this.Selector = Selector;
            this.ListInd = new List<GAIndividu>(PopulationSize);
            this.BestInd = null;
        }

        public void GenerateRandomPopuluation()
        {
            ListInd.Clear();
            for (int i = 0; i < PopulationSize; i++)
            {
                GAIndividu NewInd = new GAIndividu(AdnSize, GeneSize);
                NewInd.GenerateRandomInd();
                ListInd.Add(NewInd);
            }
        }

        public void SelectionForReproduction(bool KeepBest, double SelectedPartOfPop)
        {
            if (KeepBest)
            {
                this.SelectedInds = Selector.Select(ListInd, BestInd, SelectedPartOfPop);
            }
            else
            {
                this.SelectedInds = Selector.Select(ListInd, null, SelectedPartOfPop);
            }
        }

        public void SelectionForNextGeneration(bool KeepBest)
        {
            List<GAIndividu> NewGeneration;
            if (KeepBest)
            {
                NewGeneration = Selector.Select(ListInd, BestInd, 1.0);
            }
            else
            {
                NewGeneration = Selector.Select(ListInd, null, 1.0);
            }
            ListInd.Clear();
            ListInd = NewGeneration;
        }

        public void Reproduce(double ProbReproduction)
        {
            if (SelectedInds == null)
            {
                throw new System.ArgumentException("List of selected individu is empty");
            }

            List<GAIndividu> NewInds = new List<GAIndividu>();

            for (int i = 0; i < SelectedInds.Count; i++)
            {
                Random Rand = new Random(Guid.NewGuid().GetHashCode());
                if (Rand.NextDouble() < ProbReproduction)
                {
                    GAIndividu Ind1 = ListInd[Rand.Next(0, ListInd.Count)];
                    GAIndividu Ind2 = Ind1;
                    while (Ind2 == Ind1)
                    {
                        Ind2 = ListInd[Rand.Next(0, ListInd.Count)];
                    }
                    NewInds.AddRange(Ind1.Reproduce(Ind2));
                }
            }
            ListInd.AddRange(NewInds);
        }

        public void Mutate(double ProbMutation)
        {
            Random Rand = new Random(Guid.NewGuid().GetHashCode());
            ListInd.ForEach(Ind => {
                if (Ind != BestInd && Rand.NextDouble() < ProbMutation)
                    Ind.Mutate();
            });
        }  
    }
} 

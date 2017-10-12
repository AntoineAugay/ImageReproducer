using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    class GAIndividu
    {
        public readonly int AdnSize;
        public readonly int GeneSize;
        public List<GAGene> Adn { get; set; }
        public int Fitness { get; set; }

        //public int GetFitness() { return Fitness; }

        public GAIndividu(int AdnSize, int GeneSize)
        {
            this.AdnSize = AdnSize;
            this.GeneSize = GeneSize;
            this.Fitness = 0;
            this.Adn = new List<GAGene>(this.AdnSize);
            for (int i=0; i<AdnSize; i++)
            {
                this.Adn.Add(new GAGene(GeneSize));
            }            
        }

        public void GenerateRandomInd()
        {
            foreach(GAGene Gene in Adn)
            {
                Gene.GenerateRandomGene();
            }
        }

        public List<GAIndividu> Reproduce(GAIndividu Individu2)
        {
            if(AdnSize ==1)
            {
                return null;
            }

            List<GAIndividu> NewInds = new List<GAIndividu>();
            GAIndividu NewInd1 = new GAIndividu(AdnSize, GeneSize);
            GAIndividu NewInd2 = new GAIndividu(AdnSize, GeneSize);
            NewInd1.Adn = this.Cross(Individu2);
            NewInd2.Adn = Individu2.Cross(this);

            return NewInds;
        }

        public List<GAGene> Cross(GAIndividu Individu2)
        {
            Random Rand = new Random(Guid.NewGuid().GetHashCode());
            int SplitIndex = 1 + (int)(Rand.NextDouble() * AdnSize);
            List<GAGene> NewAdn = new List<GAGene>(this.AdnSize);
            for (int i = 0; i < SplitIndex; i++)
            {
                NewAdn.Add(this.Adn[i]);
            }
            for (int i = SplitIndex; i < AdnSize; i++)
            {
                NewAdn.Add(Individu2.Adn[i]);
            }
            return NewAdn;
        }   

        public GAIndividu Clone()
        {
            GAIndividu Clone = new GAIndividu(AdnSize, GeneSize);
            Clone.Fitness = Fitness;
            for (int i = 0; i < AdnSize; i++)
            {
                Clone.Adn[i] = this.Adn[i].Clone();
            }
            return Clone;
        }

        public void Mutate()
        {
            Random Rand = new Random();
            int SelectedGene = Rand.Next(AdnSize);
            Adn[SelectedGene].GenerateRandomGene();
        }

    }
}

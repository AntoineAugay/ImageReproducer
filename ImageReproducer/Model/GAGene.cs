using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    class GAGene
    {
        private int GeneSize;
        public double[] ListValue { get; set; }            

        public GAGene(int GeneSize)
        {
            this.GeneSize = GeneSize;
            ListValue = new double[GeneSize];
            for (int i = 0; i < GeneSize; i++)
            {
                ListValue[i] = 0.0;
            }
        }

        public void GenerateRandomGene()
        {
            Random Rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i=0; i<GeneSize ; i++) 
            {
                ListValue[i] = Rand.NextDouble();
            }
        }

        public GAGene Clone()
        {
            GAGene Clone = new GAGene(GeneSize);
            for(int i=0 ; i<GeneSize ; i++)
            {
                Clone.ListValue[i] = this.ListValue[i];
            }
            return Clone;
        }

    }
}

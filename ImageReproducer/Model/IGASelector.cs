using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    interface IGASelector
    {
        List<GAIndividu> Select(List<GAIndividu> List, GAIndividu Best, double SelectedPartOfPop);
    }
}

using System.Drawing;
using Emgu.CV;


namespace GeneticAlgorithm
{
    class GAEvaluatorImageReproducer : IGAIndividuEvaluator
    {
        private Mat TargetMat;
        private int Width;
        private int Height;
        private const int PixWidth = 5;
        private const int PixHeight = 5;

        public GAEvaluatorImageReproducer(string TargetPath)
        {
            TargetMat = CvInvoke.Imread(TargetPath, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            Width = TargetMat.Width;
            Height = TargetMat.Height;
        }

        public int EvaluateIndividu(GAIndividu Ind)
        {
            int Fitness = 0;

            if (Ind.GeneSize == 2)
            {
                Mat IndMat = IndToMat(Ind);

                byte[] TargetBytes = TargetMat.GetData();
                byte[] IndBytes = IndMat.GetData();
                
                for (int i = 0; i < IndBytes.Length; i++)
                {
                    if (IndBytes[i] == TargetBytes[i] && IndBytes[i] == 0)
                    {
                        Fitness++;
                    }
                }
            }

            return Fitness*Fitness;
        }

        public Mat IndToMat(GAIndividu Ind)
        {
            Mat IndMat = new Mat(TargetMat.Width, TargetMat.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 1);

            Emgu.CV.Structure.MCvScalar Black = new Emgu.CV.Structure.MCvScalar(0);
            Emgu.CV.Structure.MCvScalar White = new Emgu.CV.Structure.MCvScalar(255);

            IndMat.SetTo(White);

            Ind.Adn.ForEach(Gene => {
                CvInvoke.Rectangle(IndMat
                                , new Rectangle((int)(Gene.ListValue[0] * Width) - 3
                                               , (int)(Gene.ListValue[1] * Height) - 3
                                               , PixWidth
                                               , PixHeight)
                                , Black
                                , -1);
            });
            return IndMat;
        }

        public void Save(GAIndividu Ind)
        {
            Mat IndMat = IndToMat(Ind);
            IndMat.Save(@"C:\Users\Antoine\Pictures\Ind.bmp");
        }

    }
}

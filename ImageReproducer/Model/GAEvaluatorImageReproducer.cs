using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace GeneticAlgorithm
{

    public enum GAColorType
    {
        BlackAndWhite,
        GrayScale
    }

    public enum GAGeneForm
    {
        Rectangle,
        Ellipse
    }

    public class GAEvaluatorParameters
    {
        public GAInterpretationParameters Interpretation;
        public GAEvaluationParameters Evaluation;

        public GAEvaluatorParameters()
        {
            Interpretation = new GAInterpretationParameters();
            Evaluation = new GAEvaluationParameters();
        }
    }

    public class GAInterpretationParameters
    {
        public GAColorType Color;
        public GAGeneForm Form;
        public bool IsSizeVariable;
        public int MaxSize;
        public int MinSize;
        public int Height;
        public int Width;
    }

    public class GAEvaluationParameters
    {

    }

    class GAEvaluatorImageReproducer : IGAIndividuEvaluator
    {
        private GAEvaluatorParameters Params;
        private Mat TargetMat;
        private MCvScalar Background;
        private int Width;
        private int Height;

        public GAEvaluatorImageReproducer(string TargetPath, GAEvaluatorParameters Params)
        {
            this.Params = Params;
            TargetMat = CvInvoke.Imread(TargetPath, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            Background = FindBackground(TargetMat);
            Width = TargetMat.Width;
            Height = TargetMat.Height;
        }

        private MCvScalar FindBackground(Mat Img)
        {
            byte[] TargetBytes = TargetMat.GetData();
            int cpt = 0;
            for (int i = 0; i < TargetBytes.Length; i++)
            {
                if (TargetBytes[i] > 127) 
                {
                    cpt++;
                }
            }

            if (cpt > TargetBytes.Length / 2)
            {
                return new MCvScalar(255);
            }
            else
            {
                return new MCvScalar(0);
            }

            
        }

        public int EvaluateIndividu(GAIndividu Ind)
        {
            int Fitness = 0;

            Mat IndMat = IndToMat(Ind);

            byte[] TargetBytes = TargetMat.GetData();
            byte[] IndBytes = IndMat.GetData();
                
            for (int i = 0; i < IndBytes.Length; i++)
            {
                    
                if(IndBytes[i] == TargetBytes[i])
                {
                    Fitness++;
                }
            }

            return Fitness*Fitness;
        }

        public Mat IndToMat(GAIndividu Ind)
        {
            
            Mat IndMat = new Mat(TargetMat.Width, TargetMat.Height, Emgu.CV.CvEnum.DepthType.Cv8U, 1);

            IndMat.SetTo(Background);

            Ind.Adn.ForEach(Gene =>
            {

                MCvScalar Color;
                int IndexColor, PixWidth, PixHeight;
                int DeltaSize = Params.Interpretation.MaxSize + 1 - Params.Interpretation.MinSize;

                if (Params.Interpretation.IsSizeVariable)
                {
                    PixWidth   = Params.Interpretation.MinSize + (int)(DeltaSize * Gene.ListValue[2]);
                    PixHeight  = Params.Interpretation.MinSize + (int)(DeltaSize * Gene.ListValue[3]);
                    IndexColor = 4;
                }
                else
                {
                    PixWidth   = Params.Interpretation.Width;
                    PixHeight  = Params.Interpretation.Height;
                    IndexColor = 2;
                }

                if (Params.Interpretation.Color == GAColorType.BlackAndWhite)
                {
                    Color = new MCvScalar(0);
                }
                else
                {
                    Color = new MCvScalar((int)(Gene.ListValue[IndexColor] * 256));
                }


                if (Params.Interpretation.Form == GAGeneForm.Rectangle)
                {
                    CvInvoke.Rectangle(IndMat
                                , new Rectangle((int)(Gene.ListValue[0] * Width) - (int)PixWidth/2
                                               , (int)(Gene.ListValue[1] * Height) - (int)PixHeight/2
                                               , PixWidth
                                               , PixHeight)
                                , Color
                                , -1);
                }
                else
                {
                    CvInvoke.Ellipse(IndMat
                                    , new RotatedRect(new PointF((int)(Gene.ListValue[0] * Width)
                                                                , (int)(Gene.ListValue[1] * Height))
                                                     , new SizeF(PixWidth
                                                                , PixHeight)
                                                     , 0)
                                    , Color
                                    , -1
                                    );
                }    
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

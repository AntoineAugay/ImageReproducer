using Emgu.CV;

namespace ImageReproducer.ViewModel
{
    class ThreadMessage
    {
        public Mat Image { get; set; }
        public int GenerationNumber { get; set; }
        public int BestFitness { get; set; }


        public ThreadMessage(Mat Image, int GenerationNumber, int BestFitness)
        {
            this.Image = Image;
            this.GenerationNumber = GenerationNumber;
            this.BestFitness = BestFitness;
        }
    }
}

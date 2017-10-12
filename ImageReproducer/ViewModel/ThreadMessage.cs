using Emgu.CV;

namespace ImageReproducer.ViewModel
{
    class ThreadMessage
    {
        public Mat Image { get; set; }
        public int GenerationNumber { get; set; }

        public ThreadMessage(Mat Image, int GenerationNumber)
        {
            this.Image = Image;
            this.GenerationNumber = GenerationNumber;
        }
    }
}

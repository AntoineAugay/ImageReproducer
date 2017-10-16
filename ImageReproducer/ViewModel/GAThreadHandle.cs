using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using GeneticAlgorithm;


namespace ImageReproducer.ViewModel
{
    class GAThreadHandle
    {
        private GAParameters GAParams;
        private GAEvaluatorParameters GAEvalParams;
        private ConcurrentQueue<ThreadMessage> ImageQueue;
        private CancellationToken Token;
        private string TargetPath;

        public GAThreadHandle(GAParameters Params, GAEvaluatorParameters EvalParams,string TargetPath, ConcurrentQueue<ThreadMessage> ImageQueue, CancellationToken CancelToken)
        {
            this.GAParams = Params;
            this.GAEvalParams = EvalParams;
            this.TargetPath = TargetPath;
            this.ImageQueue = ImageQueue;
            this.Token = CancelToken;
        }

        // Méthode boucle du thread
        public void ThreadLoop()
        {
            GAEvaluatorImageReproducer Evaluator = new GAEvaluatorImageReproducer(TargetPath, GAEvalParams);
            GAGenerationManager Manager = new GAGenerationManager(GAParams, Evaluator);

            int n = 0;

            while (++n <= GAParams.General.NumberOfGeneration && !Token.IsCancellationRequested)
            {
                Manager.NextGeneration();
                ImageQueue.Enqueue(new ThreadMessage(Evaluator.IndToMat(Manager.GetBestInd()), Manager.GenerationNumber, Manager.GetBestInd().Fitness));
            }
            Manager.SaveBestInd();

        }
    }
}

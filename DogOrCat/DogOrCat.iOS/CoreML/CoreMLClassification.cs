using System;
using System.Collections.Generic;
using System.Linq;
using CoreML;
using DogOrCat.Events;
using DogOrCat.Interfaces;
using Foundation;
using ImageIO;
using Vision;

namespace DogOrCat.iOS.CoreML
{
    public class CoreMLClassification : IClassification
    {
        public void Classification(byte[] bytes)
        {
            var modelUrl = NSBundle.MainBundle.GetUrlForResource("model", "mlmodel");

            var compiledUrl = MLModel.CompileModel(modelUrl, out var error);

            var compiledModel = MLModel.Create(compiledUrl, out error);

            var vncCoreModel = VNCoreMLModel.FromMLModel(compiledModel, out error);

            var classificationRequest = new VNCoreMLRequest(vncCoreModel, VNRequestHandler);

            var data = NSData.FromArray(bytes);

            var handler = new VNImageRequestHandler(data, CGImagePropertyOrientation.Up, new VNImageOptions());

            handler.Perform(new VNRequest[] {classificationRequest}, out error);
        }

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;

        private void VNRequestHandler(VNRequest request, NSError error)
        {
            if (error != null)
                ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(new Dictionary<string, float>()));

            var result = request.GetResults<VNClassificationObservation>();
            var classifications = result.OrderByDescending(s => s.Confidence)
                .ToDictionary(s => s.Identifier, s => s.Confidence);
            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(classifications));
        }
    }
}
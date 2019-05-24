using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Graphics;
using DogOrCat.Events;
using DogOrCat.Interfaces;
using Org.Tensorflow.Contrib.Android;

namespace DogOrCat.Droid.TensorFlowClassfication
{
    public class TensorFlowClassification : IClassification
    {
        public async void Classification(byte[] bytes)
        {
            var assets = Application.Context.Assets;
            var inferenceInterface = new TensorFlowInferenceInterface(assets, "model.pb");

            var inputSize = (int) inferenceInterface.GraphOperation("Placeholder").Output(0).Shape().Size(1);

            List<string> labels;
            using (var streamReader = new StreamReader(assets.Open("labels.txt")))
            {
                labels = streamReader.ReadToEnd().Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s))
                    .ToList();
            }

            var bitmap = await BitmapFactory.DecodeByteArrayAsync(bytes, 0, bytes.Length);
            var resizedMap = Bitmap.CreateScaledBitmap(bitmap, inputSize, inputSize, false)
                .Copy(Bitmap.Config.Argb8888, false);

            var floatValues = new float[inputSize * inputSize * 3];
            var intValues = new int[inputSize * inputSize];

            resizedMap.GetPixels(intValues, 0, inputSize, 0, 0, inputSize, inputSize);

            for (var i = 0; i < intValues.Length; ++i)
            {
                var intValue = intValues[i];
                floatValues[i * 3 + 0] = (intValue & 0xFF) - 105f;
                floatValues[i * 3 + 1] = ((intValue >> 8) & 0xFF) - 117f;
                floatValues[i * 3 + 2] = ((intValue >> 16) & 0xFF) - 124f;
            }

            var operation = inferenceInterface.GraphOperation("loss");

            var outputs = new float[labels.Count];
            inferenceInterface.Feed("Placeholder", floatValues, 1, inputSize, inputSize, 3);
            inferenceInterface.Run(new[] {"loss"}, true);
            inferenceInterface.Fetch("loss", outputs);

            var result = new Dictionary<string, float>();

            for (var i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                result.Add(label, outputs[i]);
            }

            var maxConf = 0f;

            for (var i = 0; i < outputs.Length; ++i)
                if (outputs[i] > maxConf)
                {
                    maxConf = outputs[i];
                }

            ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));
        }

        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;
    }
}
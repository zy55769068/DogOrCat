using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.AI.MachineLearning;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using DogOrCat.Events;
using DogOrCat.Interfaces;

namespace DogOrCat.UWP.WindowsML
{
    public class OnnxClassification : IClassification
    {
        private readonly string modelFileName = "PetModel.onnx";

        private PetModelModel petModel;

        public async void Classification(byte[] bytes)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{modelFileName}"));
            petModel = await PetModelModel.CreateFromStreamAsync(file);

            try
            {
                var newBitmap = new WriteableBitmap(255, 255);

                using (var stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await newBitmap.SetSourceAsync(stream);
                }

                var outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(
                    newBitmap.PixelBuffer,
                    BitmapPixelFormat.Bgra8,
                    newBitmap.PixelWidth,
                    newBitmap.PixelHeight
                );


                var frame = VideoFrame.CreateWithSoftwareBitmap(outputBitmap);


                if (frame != null)
                    try
                    {
                        var inputData = new PetModelInput();
                        inputData.data = ImageFeatureValue.CreateFromVideoFrame(frame);
                        var results = await petModel.EvaluateAsync(inputData);
                        var loss = results.loss.ToList();

                        var catValue = loss.FirstOrDefault()["cat"];
                        var dogValue = loss.FirstOrDefault()["dog"];

                        var result = new Dictionary<string, float>();
                        if (catValue > dogValue)
                            result.Add("cat", catValue);
                        else
                            result.Add("dog", dogValue);

                        ClassificationCompleted?.Invoke(this, new ClassificationEventArgs(result));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"error: {ex.Message}");
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public event EventHandler<ClassificationEventArgs> ClassificationCompleted;
    }
}
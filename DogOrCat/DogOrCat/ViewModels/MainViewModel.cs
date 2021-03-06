﻿using System.IO;
using System.Linq;
using System.Windows.Input;
using DogOrCat.Events;
using DogOrCat.Framework;
using DogOrCat.Interfaces;
using DogOrCat.Models;
using DogOrCat.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DogOrCat.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private byte[] bytes;
        private readonly IClassification classification;

        public MainViewModel(IClassification classification)
        {
            this.classification = classification;
        }

        /// <summary>
        /// Take photo command
        /// </summary>
        /// <value>The take photo command.</value>
        public ICommand TakePhotoCommand => new Command(async () =>
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Rear
            });
            HandlePhoto(photo);
        });

        /// <summary>
        /// Pick photo command.
        /// </summary>
        /// <value>The pick photo command.</value>
        public ICommand PickPhotoCommand => new Command(async () =>
        {
            var photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                SaveMetaData = false
            });
            HandlePhoto(photo);
        });

        /// <summary>
        /// Handles the photo.
        /// </summary>
        /// <param name="photo">Photo.</param>
        private void HandlePhoto(MediaFile photo)
        {
            if (photo == null) return;

            var stream = photo.GetStream();
            bytes = ReadPhoto(stream);

            classification.ClassificationCompleted += Classification_ClassificationCompleted;
            classification.Classification(bytes);
        }

        /// <summary>
        /// Reads the photo.
        /// </summary>
        /// <returns>The photo.</returns>
        /// <param name="stream">Stream.</param>
        private byte[] ReadPhoto(Stream stream)
        {
            var buffer = new byte[16 * 1024];
            using (var memoryStream = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0) memoryStream.Write(buffer, 0, read);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Classifications the classification completed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Classification_ClassificationCompleted(object sender, ClassificationEventArgs e)
        {
            classification.ClassificationCompleted -= Classification_ClassificationCompleted;

            Result result = null;

            if (e.Classification.Any())
            {
                var classificationResult = e.Classification.OrderByDescending(x => x.Value).First();

                result = new Result
                {
                    IsDog = classificationResult.Key == "dog",
                    IsCat = classificationResult.Key == "cat",
                    Confidence = classificationResult.Value,
                    PhotoBytes = bytes
                };
            }
            else
            {
                result = new Result
                {
                    IsDog = false,
                    IsCat = false,
                    Confidence = 1,
                    PhotoBytes = bytes
                };
            }

            var view = Resolver.Resolve<ResultView>();
            ((ResultViewModel) view.BindingContext).Initialize(result);

            Navigation.PushAsync(view);
        }
    }
}
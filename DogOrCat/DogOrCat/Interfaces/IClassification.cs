using System;
using DogOrCat.Events;

namespace DogOrCat.Interfaces
{
    public interface IClassification
    {
        void Classification(byte[] bytes);

        event EventHandler<ClassificationEventArgs> ClassificationCompleted;
    }
}

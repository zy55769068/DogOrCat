using System;
using System.Collections.Generic;

namespace DogOrCat.Events
{
    public class ClassificationEventArgs : EventArgs
    {
        public ClassificationEventArgs(Dictionary<string, float> classification)
        {
            Classification = classification;
        }

        public Dictionary<string, float> Classification { get; set; }
    }
}
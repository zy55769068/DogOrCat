using System;
using System.Collections.Generic;
using System.Text;
using DogOrCat.Models;

namespace DogOrCat.ViewModels
{
    public class ResultViewModel : ViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private byte[] _photoBytes;
        public byte[] PhotoBytes
        {
            get => _photoBytes;
            set
            {
                _photoBytes = value;
                OnPropertyChanged();
            }
        }

        public void Initialize(Result result)
        {
            PhotoBytes = result.PhotoBytes;
            if (result.IsDog && result.Confidence > 0.9)
            {
                Title = "汪星人";
                Description = "这是一只汪星人";
            }
            else if (result.IsDog)
            {
                Title = "不确定";
                Description = "这应该是一只汪星人";
            }
            else if (result.IsCat && result.Confidence > 0.9)
            {
                Title = "喵星人";
                Description = "这是一只喵星人";
            }
            else if (result.IsCat)
            {
                Title = "不确定";
                Description = "这应该是一只喵星人";
            }
            else
            {
                Title = "识别失败";
                Description = "识别失败";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewerLogic.Model
{
    public class ImageData // : INotifyPropertyChanged
    {
        private string _imageName;
        private string _imagePath;

        public string ImageName
        {
            get
            {
                return _imageName;
            }
            set
            {
                _imageName = value;  
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;   
            }
        }

       
    }
}

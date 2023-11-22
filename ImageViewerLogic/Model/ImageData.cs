using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageViewerLogic.Model
{
    public class ImageData
    {
        private string _name;
        private string _path;
        private ObservableCollection<PageData> _pages = new ObservableCollection<PageData>();

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Path
        {
            get => _path;
            set => _path = value;
        }
        public ObservableCollection<PageData> Pages 
        { 
            get => _pages;
            set => _pages = value;
        }
    }
}

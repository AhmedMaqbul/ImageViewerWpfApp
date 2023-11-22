using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageViewerLogic.Model
{
    public class PageData
    {
        private string _documentName;
        private string _pageName;
        private int _index;
        private BitmapFrame _frame;

        public string DocumentName
        {
            get => _documentName;
            set => _documentName = value;
        }
        public string PageName 
        { 
            get => _pageName; 
            set => _pageName = value; 
        }
        public int Index
        {
            get => _index;
            set => _index = value;
        }
        public BitmapFrame Frame
        {
            get => _frame;
            set => _frame = value;
        }
    }
}

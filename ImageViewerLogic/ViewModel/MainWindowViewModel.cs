using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using ImageViewerLogic.Commands;
using ImageViewerLogic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;
using Path = System.IO.Path;

namespace ImageViewerLogic.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            BrowseCommand = new BaseCommand(BrowseFolderCommand, CanBrowseCommand);
            NextImageCommand = new BaseCommand(NavigateNextImageCommand, CanNavigateNextImageCommand);
            PreviousImageCommand = new BaseCommand(NavigatePreviousImageCommand, CanNavigatePreviousImageCommand);

            ImageFileExtensions = InitializeImageFileExtensionHashSet();
        }

        public ObservableCollection<ImageData> Images { get; set; } = new ObservableCollection<ImageData>();

        private string _selectedFolderPath = string.Empty;
        public string SelectedFolderPath
        {
            get => _selectedFolderPath;
            set
            {
                _selectedFolderPath = value;
                OnPropertyChanged(nameof(SelectedFolderPath));
            }
        }

        private ImageData _selectedImage;
        public ImageData SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private BitmapImage _selectedImageBitmap;
        public BitmapImage SelectedImageBitmap
        {
            get => _selectedImageBitmap;
            set
            {
                _selectedImageBitmap = value;
            }
        }
        private HashSet<string> ImageFileExtensions
        {
            get;
        }

        public ICommand BrowseCommand { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }

        public HashSet<string> InitializeImageFileExtensionHashSet()
        {
            HashSet<string> fileExtensions = new HashSet<string>();
            fileExtensions.Add(".jpg");
            fileExtensions.Add(".jpeg");
            fileExtensions.Add(".png");
            fileExtensions.Add(".gif");
            fileExtensions.Add(".bmp");
            fileExtensions.Add(".tiff");
            fileExtensions.Add(".tif");
            fileExtensions.Add(".jfif"); 

            return fileExtensions;
        }


        public void SetSelectedFolderPath(string folderPath)
        {
            SelectedFolderPath = folderPath;
            PrepareImageCollection(folderPath);
            SetFirstImageSelected();
        }

        public bool IsImageFile(string extension)
        {
            if (ImageFileExtensions.Contains(extension))
                return true;
            return false;
        }


        //getting file list from folder
        //iterate files
        //checking the extension each file
        //if file is contains image extension , add the image path to extension
        public void PrepareImageCollection(string folderPath)
        {
            Images.Clear();

            if (Directory.Exists(folderPath) == false)
            {
                MessageBox.Show("Folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Folder does not exist" + folderPath);
            }

            string[] allFiles = Directory.GetFiles(folderPath);

            //Add image file to collection
            foreach (var filePath in allFiles)
            {
                string extension = Path.GetExtension(filePath).ToLower();
                if (IsImageFile(extension))
                {
                    Images.Add(new ImageData { ImageName = Path.GetFileName(filePath), ImagePath = filePath });
                }
            }

            if (Images.Count == 0)
                MessageBox.Show("Folder does not contain any image file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Folder contains {Images.Count} image file(s).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SetFirstImageSelected()
        {
            SelectedImage = Images.FirstOrDefault();
        }

        #region Commands
        public bool CanBrowseCommand(Object parameter = null)
        {
            return true;
        }
        public void BrowseFolderCommand(Object parameter = null)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                SetSelectedFolderPath(folderBrowserDialog.SelectedPath);
            }
        }

        public bool CanNavigatePreviousImageCommand(Object parameter = null)
        {
            if (Images.Count > 1 && SelectedImage == Images[0] == false)
                return true;
            return false;
        }
        public void NavigatePreviousImageCommand(Object parameter = null)
        {
            var currentIndex = Images.IndexOf(SelectedImage);
            SelectedImage = Images[currentIndex - 1];
        }

        public bool CanNavigateNextImageCommand(Object parameter = null)
        {
            if (Images.Count > 1 && SelectedImage == Images[Images.Count - 1] == false)
                return true;
            return false;
        }
        public void NavigateNextImageCommand(Object parameter = null)
        {
            var currentIndex = Images.IndexOf(SelectedImage);
            SelectedImage = Images[currentIndex + 1];
        }
        #endregion 


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

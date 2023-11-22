using ImageViewerLogic.Commands;
using ImageViewerLogic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xamarin.Forms.Xaml;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using Point = System.Windows.Point;

namespace ImageViewerLogic.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public const double MaxZoomLevel = 200.0;

        public const double MinZoomLevel = 100.0;

        public const double ZoomIncrementOrDecrement = 5.0;

        private double _zoomLevel = 100.0;

        private string _selectedFolderPath = string.Empty;

        private ImageData _selectedImage = new ImageData();

        private PageData _selectedPage = new PageData();

        private Point _mousePosition = new Point(0, 0);

        private HashSet<string> ImageFileExtensions { get; }

        public string SelectedFolderPath
        {
            get => _selectedFolderPath;
            set
            {
                _selectedFolderPath = value;
                OnPropertyChanged(nameof(SelectedFolderPath));
            }
        }

        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                if (value < MinZoomLevel)
                {
                    _zoomLevel = MinZoomLevel;
                }
                else if (value > MaxZoomLevel)
                {
                    _zoomLevel = MaxZoomLevel;
                }
                else
                {
                    _zoomLevel = value;
                }
                OnPropertyChanged(nameof(ZoomLevel));
            }
        }

        public ImageData SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (_selectedImage == value)
                    return;

                _selectedImage = value;
                ZoomLevelResetCommand();
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        public PageData SelectedPage
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
                ZoomLevelResetCommand();
                SelectedImage = GetImage(_selectedPage.DocumentName);
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        public Point MousePosition
        {
            get => _mousePosition;
            set => _mousePosition = value;
        }

        public ObservableCollection<ImageData> Images { get; set; } = new ObservableCollection<ImageData>();

        public MainWindowViewModel()
        {
            BrowseCommand = new BaseCommand(BrowseFolderCommand, CanBrowseCommand);
            NextImageCommand = new BaseCommand(NavigateNextImageCommand, CanNavigateNextImageCommand);
            PreviousImageCommand = new BaseCommand(NavigatePreviousImageCommand, CanNavigatePreviousImageCommand);

            SelectedItemCommand = new BaseCommand(SetSelectedItemCommand, CanSelectedItemCommand);
            PreviousPageCommand = new BaseCommand(NavigatePreviousPageCommand, CanNavigatePreviousPageCommand);
            NextPageCommand = new BaseCommand(NavigateNextPageCommand, CanNavigateNextPageCommand);
            FirstPageCommand = new BaseCommand(NavigateFirstPageCommand, CanNavigateFirstPageCommand);
            LastPageCommand = new BaseCommand(NavigateLastPageCommand, CanNavigateLastPageCommand);

            ZoomInCommand = new BaseCommand(ZoomInExecuteCommand, CanZoomInCommand);
            ZoomOutCommand = new BaseCommand(ZoomOutExecuteCommand, CanZoomOutCommand);
            ResetCommand = new BaseCommand(ZoomLevelResetCommand, CanZoomLevelResetCommand);

            ImageFileExtensions = InitializeImageFileExtensionHashSet();
        }

        public ICommand BrowseCommand { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand ZoomInCommand { get; set; }
        public ICommand ZoomOutCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SelectedItemCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand FirstPageCommand { get; set; }
        public ICommand LastPageCommand { get; set; }

        public void SetSelectedFolderPath(string folderPath)
        {
            SelectedFolderPath = folderPath;
            PrepareImageCollection(folderPath);
            PageFrameSetUpForEmptyImages();
            MessageBoxServices(folderPath);
            SetFirstImageSelected();
        }

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

        public ImageData GetImage(string documentName)
        {
            foreach (var image in Images)
            {
                if (image.Name == documentName)
                    return image;
            }

            return null;
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
                throw new Exception("Folder does not exist" + folderPath);
            }

            string[] allFiles = Directory.GetFiles(folderPath);

            //Add image file to collection
            foreach (var filePath in allFiles)
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (IsImageFile(extension))
                {
                    ImageData imageDatas = LoadImageInfo(filePath);
                    Images.Add(imageDatas);
                }
            }
        }

        public void PageFrameSetUpForEmptyImages()
        {
            if (Images.Count == 0)
            {
                SelectedPage.Frame = null;
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        public void MessageBoxServices(string folderPath)
        {
            if (Directory.Exists(folderPath) == false)
            {
                MessageBox.Show("Folder does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (Images.Count == 0)
            {
                MessageBox.Show("Folder does not contain any image file.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Folder contains {Images.Count} image file(s).", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SetFirstImageSelected()
        {
            SelectedImage = Images.FirstOrDefault();
        }

        public ImageData LoadImageInfo(string imagePath)
        {
            ImageData imageData = new ImageData();
            imageData.Path = imagePath;
            imageData.Name = Path.GetFileName(imagePath);

            var bitmapDecoder = BitmapDecoder.Create(new Uri(imagePath), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            var noOfPages = bitmapDecoder.Frames.Count;

            for (int pageNumber = 0; pageNumber < noOfPages; pageNumber++)
            {
                PageData page = new PageData
                {
                    PageName = $"Page {pageNumber + 1}",
                    DocumentName = imageData.Name,
                    Index = pageNumber,
                    Frame = bitmapDecoder.Frames[pageNumber]
                };

                imageData.Pages.Add(page);
            }

            return imageData;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                SetSelectedFolderPath(folderBrowserDialog.SelectedPath);
        }

        public bool CanNavigatePreviousImageCommand(Object parameter = null)
        {
            if (Images.Count > 1 && SelectedImage == Images[0] == false)
                return true;
            else
                return false;
        }
        public void NavigatePreviousImageCommand(Object parameter = null)
        {
            ZoomLevelResetCommand();
            var currentIndex = Images.IndexOf(SelectedImage);
            SelectedImage = Images[currentIndex - 1];
        }

        public bool CanNavigateNextImageCommand(Object parameter = null)
        {
            if (Images.Count > 1 && SelectedImage == Images[Images.Count - 1] == false)
                return true;
            else
                return false;
        }
        public void NavigateNextImageCommand(Object parameter = null)
        {
            ZoomLevelResetCommand();
            var currentIndex = Images.IndexOf(SelectedImage);
            SelectedImage = Images[currentIndex + 1];
        }

        public bool CanNavigatePreviousPageCommand(Object parameter)
        {
            if (parameter is ImageData imageData)
            {
                if (imageData.Pages.Count > 1 && _selectedPage == imageData.Pages[0] == false)
                    return true;
            }

            return false;
        }
        public void NavigatePreviousPageCommand(Object parameter)
        {
            ZoomLevelResetCommand();
            var currentIndexOfPage = SelectedPage.Index;
            SelectedPage = SelectedImage.Pages[currentIndexOfPage - 1];
        }

        public bool CanNavigateNextPageCommand(Object parameter)
        {
            if (parameter is ImageData imageData)
            {
                if (imageData.Pages.Count > 1 && _selectedPage == imageData.Pages[imageData.Pages.Count - 1] == false)
                    return true;
            }

            return false;
        }
        public void NavigateNextPageCommand(Object parameter)
        {
            ZoomLevelResetCommand();
            var currentIndexOfPage = SelectedPage.Index;
            SelectedPage = SelectedImage.Pages[currentIndexOfPage + 1];
        }

        public bool CanNavigateFirstPageCommand(object parameter)
        {
            if (parameter is ImageData imageData)
            {
                if (imageData.Pages.Count > 1 && _selectedPage == imageData.Pages[0] == false)
                    return true;
            }

            return false;
        }
        public void NavigateFirstPageCommand(object parameter)
        {
            ZoomLevelResetCommand();
            SelectedPage = SelectedImage.Pages[0];
        }

        public bool CanNavigateLastPageCommand(object parameter)
        {
            if (parameter is ImageData imageData)
            {
                if (imageData.Pages.Count > 1 && _selectedPage == imageData.Pages[imageData.Pages.Count - 1] == false)
                    return true;
            }

            return false;
        }
        public void NavigateLastPageCommand(object parameter)
        {
            ZoomLevelResetCommand();
            SelectedPage = SelectedImage.Pages[SelectedImage.Pages.Count - 1];
        }

        public bool CanZoomInCommand(object parameter = null)
        {
            if (ZoomLevel >= MinZoomLevel && ZoomLevel < MaxZoomLevel && Images.Count > 0)
                return true;
            else
                return false;
        }
        public void ZoomInExecuteCommand(Object parameter = null)
        {
            ZoomLevel += ZoomIncrementOrDecrement;
        }

        public bool CanZoomOutCommand(object parameter = null)
        {
            if (ZoomLevel > MinZoomLevel && Images.Count > 0)
                return true;
            else
                return false;
        }
        public void ZoomOutExecuteCommand(Object parameter = null)
        {
            ZoomLevel -= ZoomIncrementOrDecrement;
        }

        public bool CanZoomLevelResetCommand(Object parameter = null)
        {
            if (ZoomLevel > MinZoomLevel && Images.Count > 0)
                return true;
            else
                return false;
        }
        public void ZoomLevelResetCommand(Object parameter = null)
        {
            ZoomLevel = MinZoomLevel;
        }

        public bool CanSelectedItemCommand(object parameter)
        {
            return true;
        }
        public void SetSelectedItemCommand(object parameter)
        {
            if (parameter is PageData page)
            {
                SelectedPage = page;
            }
            else if (parameter is ImageData imageData)
            {
                SelectedPage = imageData.Pages.FirstOrDefault();
            }
        }
        #endregion
    }
}

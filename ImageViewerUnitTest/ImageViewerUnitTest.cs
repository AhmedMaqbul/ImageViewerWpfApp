using ImageViewerLogic.Model;
using ImageViewerLogic.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ImageViewerUnitTest
{
    [TestClass]
    public class ImageViewerUnitTest
    {
        public string EnvironmentVariableName = "CommonDataTestPath";
        public string EnvironmentVariableValue = @"C:\Ahmed\ImageViewerWpfApp\ImageViewerUnitTest\DataTest\";

        public string EnvironmentVariable()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableValue);
            string commonDataTestPath = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            return commonDataTestPath;
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_CollectionOfImagesCountZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnTrue_CollectionOfImagesCountNotZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_ImageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });

            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_SelectedImageAtZerothIndexOfImageCollection()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });

            viewModel.SelectedImage = viewModel.Images[0];
            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_CollectionOfImagesCountZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnTrue_CollectionOfImagesCountNotZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_ImageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });

            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_SelectedImageAtLastIndexOfImageCollection()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });

            viewModel.SelectedImage = viewModel.Images[viewModel.Images.Count - 1];
            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void NavigatePreviousImageCommand_AlwaysGoImmediatePrevious_ImageOfSelectedImage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            viewModel.Images.Add(new ImageData { Name = "Test2", Path = "test2" });


            viewModel.SelectedImage = viewModel.Images[viewModel.Images.Count - 1];
            viewModel.NavigatePreviousImageCommand();

            Assert.AreEqual("Test1", viewModel.SelectedImage.Name);
            Assert.AreEqual("test1", viewModel.SelectedImage.Path);
        }

        [TestMethod]
        [DataRow(120)]
        [DataRow(150)]
        [DataRow(200)]
        public void NavigatePreviousImageCommand_ResetsZoomLevel_WhileMovingPreviousImage(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            viewModel.Images.Add(new ImageData { Name = "Test2", Path = "test2" });

            viewModel.SelectedImage = viewModel.Images[viewModel.Images.Count - 1];

            viewModel.ZoomLevel = arbitaryZoomLevel;

            viewModel.NavigatePreviousImageCommand();

            Assert.AreEqual(100, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void NaviagteNextImageCommand_AlwaysGoImmediateNext_ImageOfSelectedImage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            viewModel.Images.Add(new ImageData { Name = "Test2", Path = "test2" });

            viewModel.SelectedImage = viewModel.Images[0];
            viewModel.NavigateNextImageCommand();

            Assert.AreEqual("Test1", viewModel.SelectedImage.Name);
            Assert.AreEqual("test1", viewModel.SelectedImage.Path);
        }

        [TestMethod]
        [DataRow(120)]
        [DataRow(150)]
        [DataRow(200)]
        public void NaviagteNextImageCommand_ResetsZoomLevel_WhileGoingNextImage(int arbitoryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });

            viewModel.SelectedImage = viewModel.Images[0];

            viewModel.ZoomLevel = arbitoryZoomLevel;

            viewModel.NavigateNextImageCommand();

            Assert.AreEqual(100, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void CanNavigatePreviousPageCommand_ReturnsFalse_PageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Mixed\Grass_png-green-image.png";
            ImageData imageData = viewModel.LoadImageInfo(imagePath);

            Assert.AreEqual(false, viewModel.CanNavigatePreviousPageCommand(imageData));
        }

        [TestMethod]
        public void CanNavigatePreviousPageCommand_ReturnsFalse_PageCollectionCountIsGreaterThanOneAndSelectedPageIsFirstPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[0];

            Assert.AreEqual(false, viewModel.CanNavigatePreviousPageCommand(imageData));
        }

        [TestMethod]
        public void CanNavigatePreviousPageCommand_ReturnsTrue_PageCollectionCountIsGreaterThanOneAndSelectedPageIsNotFirstPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];

            Assert.AreEqual(true, viewModel.CanNavigatePreviousPageCommand(imageData));
        }

        [TestMethod]
        public void NavigatePreviousPageCommand_Navigates_ToPreviousPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];
            viewModel.SelectedImage = imageData;

            viewModel.NavigatePreviousPageCommand(imageData);

            Assert.AreEqual(1, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        public void NavigatePreviousPageCommand_Resets_ZoomLevel()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];
            viewModel.SelectedImage = imageData;

            viewModel.ZoomLevel = 120.0;
            viewModel.NavigatePreviousPageCommand(imageData);

            Assert.AreEqual(100.0, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void CanNavigateNextPageCommand_ReturnsFalse_PageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Mixed\Grass_png-green-image.png";
            ImageData imageData = viewModel.LoadImageInfo(imagePath);

            Assert.AreEqual(false, viewModel.CanNavigateNextPageCommand(imageData));
        }

        [TestMethod]
        public void CanNavigateNextPageCommand_ReturnsFalse_PageCollectionCountIsGreaterThanOneAndSelectedPageIsLastPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[imageData.Pages.Count - 1];

            Assert.AreEqual(false, viewModel.CanNavigateNextPageCommand(imageData));
        }

        [TestMethod]
        public void CanNavigateNextPageCommand_ReturnsTrue_PageCollectionCountIsGreaterThanOneAndSelectedPageIsNotLastPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];

            Assert.AreEqual(true, viewModel.CanNavigateNextPageCommand(imageData));
        }

        [TestMethod]
        public void NavigateNextPageCommand_NavigateTo_NextPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];
            viewModel.SelectedImage = imageData;

            viewModel.NavigateNextPageCommand(imageData);

            Assert.AreEqual(3, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        public void NavigateNextPageCommand_Resets_ZoomLevel()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[0];
            viewModel.SelectedImage = imageData;

            viewModel.ZoomLevel = 120.0;
            viewModel.NavigateNextPageCommand(imageData);

            Assert.AreEqual(100.0, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void PrepareImageCollection_PrepareImageCollection_FolderContainsImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string folderPath = EnvironmentVariable() + @"Pictures\";
            viewModel.PrepareImageCollection(folderPath);

            Assert.AreEqual(12, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_FilteredAndPrepareOnlyImageCollection_WhetherFolderContainsMixedFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string folderPath = EnvironmentVariable() + @"Mixed\";
            viewModel.PrepareImageCollection(folderPath);

            Assert.AreEqual(3, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_NotPrepareImageCollection_FolderNotContainsImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string anotherFolderPath = EnvironmentVariable() + @"NoPictures\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(0, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_NotPrepareImageCollection_FolderIsEmpty()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string anotherFolderPath = EnvironmentVariable() + @"Empty\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(0, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_OnlyPrepareImageCollectionOfSelectedFolder_IfFolderContainsFolderAndMixedFilesBoth()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string anotherFolderPath = EnvironmentVariable() + @"ContainsFolderAndImageFiles\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(3, viewModel.Images.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Folder does not exist :" + "***")]
        public void PrepareImageCollection_ThrowException_IfGivenPathRandomStringInsteadOfValidDirectory()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string randomPath = "***";
            viewModel.PrepareImageCollection(randomPath);
        }

        [TestMethod]
        public void PrepareImageCollectionAndSetSelectedFolderPath_ThrowException_IfGivenPathEmpty()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string emptyPath = string.Empty;

            Assert.ThrowsException<Exception>(() => viewModel.PrepareImageCollection(emptyPath));
            Assert.ThrowsException<Exception>(() => viewModel.SetSelectedFolderPath(emptyPath));
        }

        [TestMethod]
        [DataRow("C:\\xyz\\Image")]
        [DataRow("___")]
        [DataRow(" ")]
        [DataRow("abc")]
        [DataRow(null)]
        public void PrepareImageCollectionAndSetSelectedFolderPath_ThrowException_IfGivenRandomDirectoryPathWhichNotExist(string randomNonExistingDirectoryPath)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            Assert.ThrowsException<Exception>(() => viewModel.PrepareImageCollection(randomNonExistingDirectoryPath));
            Assert.ThrowsException<Exception>(() => viewModel.SetSelectedFolderPath(randomNonExistingDirectoryPath));
        }

        [TestMethod]
        public void PrepareImageCollection_AlwaysClearImageCollectionFirst_WhetherImageCollectionContainsImagesOrNot()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string previousFolderPath = EnvironmentVariable() + @"Pictures\";

            viewModel.PrepareImageCollection(previousFolderPath);
            int imagesCount = viewModel.Images.Count;

            string newFolderPath = EnvironmentVariable() + @"NoPictures\";

            viewModel.PrepareImageCollection(newFolderPath);

            Assert.AreNotEqual(imagesCount, viewModel.Images.Count);
            Assert.AreEqual(0, viewModel.Images.Count);
        }

        [TestMethod]
        public void IsImageFile_ReturnFalse_InValidExtension()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string wrongExtension = ".cs";

            Assert.AreEqual(false, viewModel.IsImageFile(wrongExtension));

            string anotherWrongExtension = ".txt";

            Assert.AreEqual(false, viewModel.IsImageFile(anotherWrongExtension));
        }

        [TestMethod]
        public void IsImageFile_ReturnTrue_ForValidExtension()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string correctExtension = ".png";

            Assert.AreEqual(true, viewModel.IsImageFile(correctExtension));

            string anotherCorrectExtension = ".tif";

            Assert.AreEqual(true, viewModel.IsImageFile(anotherCorrectExtension));
        }

        [TestMethod]
        public void IsImageFile_ReturnTrue_ForCollectionOfValidExtension()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            List<string> correctExtensions = new List<string>();
            correctExtensions.Add(".jpeg");
            correctExtensions.Add(".tiff");
            correctExtensions.Add(".png");

            foreach (var extension in correctExtensions)
            {
                Assert.AreEqual(true, viewModel.IsImageFile(extension));
            }
        }

        [TestMethod]
        public void IsImageFile_ReturnFalse_ForCollectionOfInvalidExtension()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            List<string> correctExtensions = new List<string>();
            correctExtensions.Add(".txt");
            correctExtensions.Add(".cs");
            correctExtensions.Add(".xaml");

            foreach (var extension in correctExtensions)
            {
                Assert.AreEqual(false, viewModel.IsImageFile(extension));
            }
        }

        [TestMethod]
        public void SetFirstImageSelected_AlwaysSetFirstOrDefault_ImageFileInSelectedFolder()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images.Add(new ImageData { Name = "Test", Path = "test" });
            viewModel.Images.Add(new ImageData { Name = "Test1", Path = "test1" });
            viewModel.Images.Add(new ImageData { Name = "Test2", Path = "test2" });

            viewModel.SetFirstImageSelected();

            Assert.AreEqual(viewModel.Images[0], viewModel.SelectedImage);
        }

        [TestMethod]
        public void SetFirstImageSelected_AlwaysSetFirstOrDefault_IfSelectedFolderContainsEmptyImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.SetFirstImageSelected();

            Assert.AreEqual(null, viewModel.SelectedImage);
        }

        [TestMethod]
        public void InitializeImageFileExtensionHashSet_OnlyContainsAndReturnsHashSetCollections_OnlyImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".jpg"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".jpeg"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".png"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".gif"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".bmp"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".tiff"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".tif"));
            Assert.IsTrue(viewModel.InitializeImageFileExtensionHashSet().Contains(".jfif"));
        }

        [TestMethod]
        public void InitializeImageFileExtensionHashSet_OnlyContains_ImageFilesExtensionsNotOtherExtensions()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            Assert.IsFalse(viewModel.InitializeImageFileExtensionHashSet().Contains(".cs"));
            Assert.IsFalse(viewModel.InitializeImageFileExtensionHashSet().Contains(".txt"));
            Assert.IsFalse(viewModel.InitializeImageFileExtensionHashSet().Contains(".pdf"));
        }

        [TestMethod]
        [DataRow(100.0)]
        [DataRow(199.0)]
        [DataRow(150.0)]
        public void CanZoomIn_ReturnTrue_IfZoomLevelWithinTheZoomRange(double randomArbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
            }

            viewModel.ZoomLevel = randomArbitaryZoomLevel;
            Assert.AreEqual(true, viewModel.CanZoomInCommand());
        }

        [TestMethod]
        [DataRow(200.0)]
        public void CanZoomIn_ReturnFalse_IfZoomLevelAtMaximumZoomLevel(double randomArbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
            }

            viewModel.ZoomLevel = randomArbitaryZoomLevel;
            Assert.AreEqual(false, viewModel.CanZoomInCommand());
        }

        [TestMethod]
        [DataRow(201.0)]
        [DataRow(250.0)]
        public void CanZoomIn_ReturnFalse_IfZoomLevelNotInZoomRange(double randomArbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
            }

            viewModel.ZoomLevel = randomArbitaryZoomLevel;
            Assert.AreEqual(false, viewModel.CanZoomInCommand());
        }

        [TestMethod]
        [DataRow(110.0)]
        [DataRow(150.0)]
        [DataRow(190.0)]
        public void ZoomIn_Increament_ZoomLevel(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
                viewModel.ZoomLevel = 0.0;
            }

            viewModel.ZoomLevel = arbitaryZoomLevel;
            viewModel.ZoomInExecuteCommand();

            Assert.AreEqual(arbitaryZoomLevel + 5, viewModel.ZoomLevel);
        }

        [TestMethod]
        [DataRow(0.0)]
        [DataRow(99.0)]
        [DataRow(100.0)]
        public void CanZoomOut_ReturnFalse_IfZoomLevelNotWithinZoomRange(double randomArbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
                viewModel.ZoomLevel = 0;
            }

            viewModel.ZoomLevel = randomArbitaryZoomLevel;
            Assert.AreEqual(false, viewModel.CanZoomOutCommand());
        }

        [TestMethod]
        [DataRow(101.0)]
        [DataRow(199.0)]
        [DataRow(150.0)]
        public void CanZoomOut_ReturnTrue_IfZoomLevelWithinTheZoomRange(double randomArbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
                viewModel.ZoomLevel = 0;
            }

            viewModel.ZoomLevel = randomArbitaryZoomLevel;
            Assert.AreEqual(true, viewModel.CanZoomOutCommand());
        }

        [TestMethod]
        [DataRow(110.0)]
        [DataRow(150.0)]
        [DataRow(190.0)]
        public void ZoomOut_Decreament_ZoomLevel(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
                viewModel.ZoomLevel = 0.0;
            }

            viewModel.ZoomLevel = arbitaryZoomLevel;
            viewModel.ZoomOutExecuteCommand();

            Assert.AreEqual(arbitaryZoomLevel - 5, viewModel.ZoomLevel);
        }

        [TestMethod]
        [DataRow(101.0)]
        [DataRow(150.0)]
        [DataRow(190.0)]
        [DataRow(200.0)]
        public void CanZoomLevelReset_ReturnsTrue_ZoomLevelGreaterThan100(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
            }

            viewModel.ZoomLevel = arbitaryZoomLevel;

            Assert.AreEqual(true, viewModel.CanZoomLevelResetCommand());
        }

        [TestMethod]
        [DataRow(91.0)]
        [DataRow(99.0)]
        public void CanZoomLevelReset_ReturnsFalse_ZoomLevelLessThan100(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
            }

            viewModel.ZoomLevel = arbitaryZoomLevel;
            Assert.AreEqual(false, viewModel.CanZoomLevelResetCommand());
        }

        [TestMethod]
        [DataRow(101.0)]
        [DataRow(150.0)]
        [DataRow(199.0)]
        [DataRow(200.0)]
        public void ZoomLevelReset_Reset_ZoomLevelToMinZoomLevel(double arbitaryZoomLevel)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.ZoomLevel >= 100.0 || viewModel.ZoomLevel < 200.0)
            {
                viewModel.Images.Add(viewModel.Images.FirstOrDefault());
                viewModel.ZoomLevel = 0.0;
            }

            viewModel.ZoomLevel = arbitaryZoomLevel;
            viewModel.ZoomLevelResetCommand();

            Assert.AreEqual(100.0, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void CanSelectedItem_AlwaysReturns_True()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedImage = imageData;

            bool actualResult = viewModel.CanSelectedItemCommand(viewModel.SelectedImage);

            Assert.AreEqual(true, actualResult);

            bool anotherActualResult = viewModel.CanSelectedItemCommand(viewModel.SelectedPage);

            Assert.AreEqual(true, anotherActualResult);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void SetSelectedItem_SetsPage_FromTreeViewWhichPageIsSelected(int pageIndex)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedImage = imageData;

            viewModel.SelectedPage = imageData.Pages[pageIndex];

            viewModel.SetSelectedItemCommand(viewModel.SelectedPage);

            Assert.AreEqual(pageIndex, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        public void SetSelectedItem_AlwaysSetsFirstPage_FromTreeViewWhenAnyRootNodeIsSelected()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedImage = imageData;

            viewModel.SetSelectedItemCommand(viewModel.SelectedImage);

            Assert.AreEqual(0, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        [DataRow(@"C:\Ahmed\ImageViewerWpfApp\ImageViewerUnitTest\DataTest\Multipage Document\Combined_Document.tiff")]
        public void LoadImageInfo_Returns_GivenImageData(string imagePath)
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage != null)
                viewModel.SelectedImage = null;

            var actualImageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedImage = actualImageData;

            Assert.AreEqual("Combined_Document.tiff", viewModel.SelectedImage.Name);
            Assert.AreEqual(imagePath, viewModel.SelectedImage.Path);
            Assert.AreEqual(5, viewModel.SelectedImage.Pages.Count);
        }

        [TestMethod]
        public void GetImage_WhenDocumentNameExists_ShouldReturnMatchingImage()
        {
            var viewModel = new MainWindowViewModel();

            var imageData1 = new ImageData { Name = "Document1" };
            var imageData2 = new ImageData { Name = "Document2" };
            var imageData3 = new ImageData { Name = "Document3" };

            viewModel.Images.Add(imageData1);
            viewModel.Images.Add(imageData2);
            viewModel.Images.Add(imageData3);

            string documentNameToFind = "Document2";
            ImageData actualResult = viewModel.GetImage(documentNameToFind);

            Assert.AreEqual(imageData2, actualResult);
        }

        [TestMethod]
        public void GetImage_WhenDocumentNameNotExists_ShouldReturnNull()
        {
            var viewModel = new MainWindowViewModel();

            var imageData1 = new ImageData { Name = "Document1" };
            var imageData2 = new ImageData { Name = "Document2" };
            var imageData3 = new ImageData { Name = "Document3" };

            viewModel.Images.Add(imageData1);
            viewModel.Images.Add(imageData2);
            viewModel.Images.Add(imageData3);

            string documentNameToFind = "DocumentNotExixt";
            ImageData result = viewModel.GetImage(documentNameToFind);

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void CanNavigateFirstPage_ReturnFlase_PageCountNotGreaterThanOne()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var pageData = new PageData { PageName = "Page 1" };
            viewModel.SelectedImage.Pages.Add(pageData);

            Assert.AreEqual(false, viewModel.CanNavigateFirstPageCommand(viewModel.SelectedImage));
        }

        [TestMethod]
        public void CanNavigateFirstPage_ReturnTrue_PageCountGreaterThanOne()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var pageData1 = new PageData { PageName = "Page 1" };
            var pageData2 = new PageData { PageName = "Page 2" };

            viewModel.SelectedImage.Pages.Add(pageData1);
            viewModel.SelectedImage.Pages.Add(pageData2);

            Assert.AreEqual(true, viewModel.CanNavigateFirstPageCommand(viewModel.SelectedImage));
        }

        [TestMethod]
        public void NavigateFirstPage_Navigate_ToFisrtPage()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);

            viewModel.SelectedPage = imageData.Pages[3];

            viewModel.SelectedImage = imageData;

            viewModel.NavigateFirstPageCommand(viewModel.SelectedImage);

            Assert.AreEqual(0, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        public void NavigateFirstPage_ResetsZoomLevel_WhileMoveToFirstPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[2];
            viewModel.SelectedImage = imageData;

            viewModel.ZoomLevel = 120.0;
            viewModel.NavigateFirstPageCommand(imageData);

            Assert.AreEqual(100.0, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void CanNavigateLastPage_ReturnFlase_PageCountNotGreaterThanOne()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var pageData = new PageData { PageName = "Page 1" };
            viewModel.SelectedImage.Pages.Add(pageData);

            Assert.AreEqual(false, viewModel.CanNavigateLastPageCommand(viewModel.SelectedImage));
        }

        [TestMethod]
        public void CanNavigateLaststPage_ReturnTrue_PageCountGreaterThanOne()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var pageData1 = new PageData { PageName = "Page 1" };
            var pageData2 = new PageData { PageName = "Page 2" };

            viewModel.SelectedImage.Pages.Add(pageData1);
            viewModel.SelectedImage.Pages.Add(pageData2);

            Assert.AreEqual(true, viewModel.CanNavigateLastPageCommand(viewModel.SelectedImage));
        }

        [TestMethod]
        public void NavigateLastPage_Navigate_ToLastPage()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);

            viewModel.SelectedPage = imageData.Pages[3];

            viewModel.SelectedImage = imageData;

            viewModel.NavigateLastPageCommand(viewModel.SelectedImage);

            Assert.AreEqual(4, viewModel.SelectedPage.Index);
        }

        [TestMethod]
        public void NavigateLastPage_ResetsZoomLevel_WhileMovingLastPage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.SelectedImage.Pages.Count > 0)
                viewModel.SelectedImage.Pages.Clear();

            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document.tiff";

            ImageData imageData = viewModel.LoadImageInfo(imagePath);
            viewModel.SelectedPage = imageData.Pages[1];
            viewModel.SelectedImage = imageData;

            viewModel.ZoomLevel = 120.0;
            viewModel.NavigateLastPageCommand(imageData);

            Assert.AreEqual(100.0, viewModel.ZoomLevel);
        }

        [TestMethod]
        public void LoadImageInfo_ValidImage_PathAndPageCount()
        {
            var viewModel = new MainWindowViewModel();
            var imagePath = EnvironmentVariable() + @"Multipage Document\Combined_Document 2.tiff";

            var imageData = viewModel.LoadImageInfo(imagePath);

            Assert.AreEqual(imagePath, imageData.Path);
            Assert.AreEqual("Combined_Document 2.tiff", imageData.Name);
            Assert.AreEqual(3, imageData.Pages.Count);
        }

        [TestMethod]
        public void PageFrameSetUpForEmptyImages_NoImages_SelectedPageFrameIsSetToNull()
        {
            var viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            viewModel.Images = new ObservableCollection<ImageData>();

            viewModel.PageFrameSetUpForEmptyImages();

            Assert.IsNull(viewModel.SelectedPage.Frame);
        }
    }
}
using ImageViewerLogic.Model;
using ImageViewerLogic.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ImageViewerUnitTest
{
    [TestClass]
    public class ImageViewerUnitTest
    {
        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_CollectionOfImagesCountZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }
            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnTrue_CollectionOfImagesCountNotZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });
            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_ImageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }
            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });

            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigatePreviousImageCommand_ReturnFalse_SelectedImageAtZerothIndexOfImageCollection()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });

            viewModel.SelectedImage = viewModel.Images[0];
            bool actualResult = viewModel.CanNavigatePreviousImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_CollectionOfImagesCountZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }
            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnTrue_CollectionOfImagesCountNotZero()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });
            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(true, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_ImageCollectionCountNotGreaterThanOne()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });

            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void CanNavigateNextImageCommand_ReturnFalse_SelectedImageAtLastIndexOfImageCollection()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });

            viewModel.SelectedImage = viewModel.Images[viewModel.Images.Count - 1];
            bool actualResult = viewModel.CanNavigateNextImageCommand();

            Assert.AreEqual(false, actualResult);
        }

        [TestMethod]
        public void NavigatePreviousImageCommand_AlwaysGoImmediatePrevious_ImageOfSelectedImage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });
            viewModel.Images.Add(new ImageData { ImageName = "Test2", ImagePath = "test2" });


            viewModel.SelectedImage = viewModel.Images[viewModel.Images.Count - 1];
            viewModel.NavigatePreviousImageCommand();

            Assert.AreEqual("Test1", viewModel.SelectedImage.ImageName);
            Assert.AreEqual("test1", viewModel.SelectedImage.ImagePath);
        }

        [TestMethod]
        public void NaviagteNextImageCommand_AlwaysGoImmediateNext_ImageOfSelectedImage()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });
            viewModel.Images.Add(new ImageData { ImageName = "Test2", ImagePath = "test2" });

            viewModel.SelectedImage = viewModel.Images[0];
            viewModel.NavigateNextImageCommand();

            Assert.AreEqual("Test1", viewModel.SelectedImage.ImageName);
            Assert.AreEqual("test1", viewModel.SelectedImage.ImagePath);
        }

        [TestMethod]
        public void PrepareImageCollection_PrepareImageCollection_FolderContainsImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string folderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\Pictures\\";
            viewModel.PrepareImageCollection(folderPath);

            Assert.AreEqual(12, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_FilteredAndPrepareOnlyImageCollection_WhetherFolderContainsMixedFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string folderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\Mixed\\";
            viewModel.PrepareImageCollection(folderPath);

            Assert.AreEqual(3, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_NotPrepareImageCollection_FolderNotContainsImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string anotherFolderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\NoPictures\\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(0, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_NotPrepareImageCollection_FolderIsEmpty()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string anotherFolderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\Empty\\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(0, viewModel.Images.Count);
        }

        [TestMethod]
        public void PrepareImageCollection_OnlyPrepareImageCollectionOfSelectedFolder_IfFolderContainsFolderAndMixedFilesBoth()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            string anotherFolderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\ContainsFolderAndImageFiles\\";
            viewModel.PrepareImageCollection(anotherFolderPath);

            Assert.AreEqual(3, viewModel.Images.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Folder does not exist :" + "***")]
        public void PrepareImageCollection_ThrowException_IfGivenPathRamdomStringInsteadOfValidDirectory()
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
            
            if(viewModel.Images.Count > 0)
                viewModel.Images.Clear();

            string previousFolderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\Pictures\\";

            viewModel.PrepareImageCollection(previousFolderPath);
            int imagesCount = viewModel.Images.Count;

            string newFolderPath = "C:\\Ahmed\\ImageViewerWpfApp\\ImageViewerUnitTest\\DataTest\\NoPictures\\";

            viewModel.PrepareImageCollection(newFolderPath);

            Assert.AreNotEqual(imagesCount,viewModel.Images.Count);
            Assert.AreEqual(0,viewModel.Images.Count);
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
        public void IsImageFile_ReturnTrue_ForCollectionofValidExtension()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            List<string> correctExtensions = new List<string>();
            correctExtensions.Add(".jpeg");
            correctExtensions.Add(".tiff");
            correctExtensions.Add(".png");

            foreach(var extension in correctExtensions)
            {
                Assert.AreEqual(true, viewModel.IsImageFile(extension));
            }
        }

        [TestMethod]
        public void IsImageFile_ReturnFalse_ForCollectionoOfInvalidExtension()
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
            {
                viewModel.Images.Clear();
            }
            viewModel.Images.Add(new ImageData { ImageName = "Test", ImagePath = "test" });
            viewModel.Images.Add(new ImageData { ImageName = "Test1", ImagePath = "test1" });
            viewModel.Images.Add(new ImageData { ImageName = "Test2", ImagePath = "test2" });

            viewModel.SetFirstImageSelected();

            Assert.AreEqual(viewModel.Images[0], viewModel.SelectedImage);
        }

        [TestMethod]
        public void SetFirstImageSelected_AlwaysSetFirstOrDefault_IfSelectedFolderContainsEmptyImageFiles()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();

            if (viewModel.Images.Count > 0)
            {
                viewModel.Images.Clear();
            }

            viewModel.SetFirstImageSelected();

            Assert.AreEqual(null, viewModel.SelectedImage);
        }

        [TestMethod]
        public void InitializeImageFileExtensionHashSet_OnlyContainsAndRetunsHashSetCollections_OnlyImageFiles()
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
    }
}

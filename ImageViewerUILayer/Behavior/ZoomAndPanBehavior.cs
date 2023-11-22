using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Xaml.Behaviors;
using ImageViewerLogic.ViewModel;
using System.Windows.Media;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;
using System.Runtime.CompilerServices;

namespace ImageViewerUILayer.Behavior
{
    public class ZoomAndPanBehavior : Behavior<Image>
    {
        public const double MaxZoomLevel = 200.0;

        public const double MinZoomLevel = 100.0;

        public const double ZoomIncrementOrDecrement = 5.0;

        public ScaleTransform ScaleTransform { get; set; } = new ScaleTransform();

        public TranslateTransform TranslateTransform { get; set; } = new TranslateTransform();

        public Point LastMousePosition { get; set; } = new Point(0, 0);

        public Point ZoomOriginPosition { get; set; } = new Point(0, 0);

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double),
                typeof(ZoomAndPanBehavior), new PropertyMetadata(100.0, OnZoomLevelChanged));

        public double ZoomLevel
        {
            get => (double)GetValue(ZoomLevelProperty);
            set => SetValue(ZoomLevelProperty, value);
        }
  
        private static void OnZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var zoomAndPanBehavior = d as ZoomAndPanBehavior;

            if (zoomAndPanBehavior == null)
                return;

            zoomAndPanBehavior.ZoomLevel = (double) e.NewValue;

            zoomAndPanBehavior.ScaleImage();
        }

        private void MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomOriginPosition = e.GetPosition(this.AssociatedObject);

            if (ZoomOriginPosition == null)
                return;

            if (e.Delta > 0)
            {
                if (ZoomLevel >= MaxZoomLevel)
                    return;
                ZoomLevel += ZoomIncrementOrDecrement;
            }
            else if (e.Delta < 0)
            {
                if (ZoomLevel <= MinZoomLevel)
                    return;
                ZoomLevel -= ZoomIncrementOrDecrement;
            }
            else
            {
                ZoomLevel = ZoomLevel;
            }

            ScaleImage();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LastMousePosition = e.GetPosition(this.AssociatedObject);

                this.AssociatedObject.CaptureMouse();

                if (IsImagePanning())
                    Mouse.OverrideCursor = Cursors.Cross;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point newMousePosition = e.GetPosition(this.AssociatedObject);

            if (e.LeftButton == MouseButtonState.Pressed && IsImagePanning())
            {
                double deltaX = newMousePosition.X - LastMousePosition.X;
                double deltaY = newMousePosition.Y - LastMousePosition.Y;

                TranslateTransform.X = deltaX;
                TranslateTransform.Y = deltaY;

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(this.AssociatedObject.RenderTransform);
                transformGroup.Children.Add(new TranslateTransform(deltaX,deltaY));

                this.AssociatedObject.RenderTransform = transformGroup;
            }
        }

        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                this.AssociatedObject.ReleaseMouseCapture();
                Mouse.OverrideCursor = null;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseWheel += MouseWheel;
            this.AssociatedObject.MouseDown += MouseDown;
            this.AssociatedObject.MouseMove += MouseMove;
            this.AssociatedObject.MouseUp += MouseUp;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.MouseWheel -= MouseWheel;
                this.AssociatedObject.MouseDown -= MouseDown;
                this.AssociatedObject.MouseMove -= MouseMove;
                this.AssociatedObject.MouseUp -= MouseUp;
            }
        }

        public void ScaleImage()
        {
            ScaleTransform.CenterX = ZoomOriginPosition.X;
            ScaleTransform.CenterY = ZoomOriginPosition.Y;
            ScaleTransform.ScaleX = ZoomLevel / 100.0;
            ScaleTransform.ScaleY = ZoomLevel / 100.0;

            this.AssociatedObject.RenderTransform = ScaleTransform;
        }

        public bool IsImagePanning()
        {
            if (ZoomLevel > MinZoomLevel)
                return true;
            else
                return false;
        }
    }
}

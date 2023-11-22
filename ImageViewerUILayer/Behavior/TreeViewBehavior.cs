using ImageViewerLogic.Model;
using ImageViewerUILayer.Virtualization;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageViewerUILayer.Behavior
{
    public class TreeViewBehavior : Behavior<TreeView>
    {
        private TreeView _treeView;

        public object SelectedImage
        {
            get { return (object)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }
        public object SelectedPage
        {
            get { return (object)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }

        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(object),
            typeof(TreeViewBehavior),
                new PropertyMetadata(null, OnSelectedImageChanged));     

        public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register("SelectedPage", typeof(object),
            typeof(TreeViewBehavior),
                new PropertyMetadata(null, OnSelectedPageChanged));

        private static void OnSelectedImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeViewBehaviour = (TreeViewBehavior)d;

            object dataItem = e.NewValue;
            TreeViewItem treeViewItem = GetTreeViewItem(treeViewBehaviour._treeView, dataItem);

            if (treeViewItem != null)
                treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        private static void OnSelectedPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var treeViewBehavior = (TreeViewBehavior)d;

            object dataItem = e.NewValue;
            TreeViewItem treeViewItem = GetTreeViewItem(treeViewBehavior._treeView, dataItem);

            if (treeViewItem != null)
                treeViewItem.SetValue(TreeViewItem.IsSelectedProperty, true);
        }

        private static TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem)container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the
                // virtualizing case even if the item is marked
                // expanded we still need to do this step in order to
                // regenerate the visuals because they may have been virtualized away.
                container.ApplyTemplate();
                ItemsPresenter itemsPresenter = (ItemsPresenter)container.Template.FindName("ItemsHost", container);

                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter,
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();

                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                Panel itemsHostPanel = (Panel)VisualTreeHelper.GetChild(itemsPresenter, 0);

                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;

                //MyVirtualizingStackPanel virtualizingPanel = itemsHostPanel as MyVirtualizingStackPanel;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;

                    //if (virtualizingPanel != null)
                    //{
                    //    // Bring the item into view so
                    //    // that the container will be generated.
                    //    virtualizingPanel.BringIntoView(i);

                    //    subContainer =
                    //        (TreeViewItem)container.ItemContainerGenerator.
                    //        ContainerFromIndex(i);
                    //}
                    //else
                    {
                        subContainer =
                            (TreeViewItem)container.ItemContainerGenerator.
                            ContainerFromIndex(i);

                        // Bring the item into view to maintain the
                        // same behavior as with a virtualizing panel.
                        subContainer.BringIntoView();
                    }

                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);

                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        private static T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    T correctlyTyped = child as T;
                    if (correctlyTyped != null)
                    {
                        return correctlyTyped;
                    }

                    T descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedImage = e.NewValue;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            _treeView = this.AssociatedObject;
            this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }
    }
}

using chkam05.PackIconToImage.Data;
using chkam05.PackIconToImage.Tools;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chkam05.PackIconToImage.Windows
{
    public partial class MainWindow : Window
    {

        //  VARIABLES

        private string _saveInitialDirectory;

        public ColorsView ColorsView { get; private set; }
        public PackIconKindView PackIconKindView { get; private set; }
        public PageCountView PageCountView { get; private set; }
        public SizeView SizeView { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MainWindow class constructor. </summary>
        public MainWindow()
        {
            //  Initialize data.
            ColorsView = new ColorsView();
            PackIconKindView = new PackIconKindView();
            PageCountView = new PageCountView();
            SizeView = new SizeView();

            //  Initialize components.
            InitializeComponent();
        }

        #endregion CLASS METHODS

        #region COMPONENTS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after moving scroll in component. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Mouse Wheel Event Arguments. </param>
        private void ListView_PreviewMouseWheelToParent(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        #endregion COMPONENTS MANAGEMENT METHODS

        #region EXPORT MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Add Size Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddSizeButton_Click(object sender, RoutedEventArgs e)
        {
            var size = new Data.Models.Size(
                SizeView.EditableSizeWidth, SizeView.EditableSizeHeight);

            SizeView.AddSize(size);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Save Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SizeView.Items.Any())
            {
                var initialDirectory = !string.IsNullOrEmpty(_saveInitialDirectory) && Directory.Exists(_saveInitialDirectory)
                    ? _saveInitialDirectory : Environment.GetEnvironmentVariable("USERPROFILE");

                var dialog = new SaveFileDialog()
                {
                    DefaultExt = ".png",
                    FileName = Enum.GetName(typeof(PackIconKind), PackIconKindView.Selected) + ".png",
                    Filter = "PNG (*.png) | *.png",
                    InitialDirectory = initialDirectory,
                    Title = "Save image as",
                };

                if (dialog.ShowDialog() == true)
                {
                    var filePath = dialog.FileName;
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var fileExt = Path.GetExtension(filePath);
                    var path = Path.GetDirectoryName(filePath);

                    _saveInitialDirectory = path;

                    foreach (var size in SizeView.Items)
                    {
                        string newFilePath = Path.Combine(path, $"{fileName}_{size.Width}x{size.Height}{fileExt}");

                        var imageSource = DrawingUtilities.DrawImageFromPackIconKind(
                            PackIconKindView.Selected, size.Width, size.Height,
                            new SolidColorBrush(ColorsView.BackgroundSelected),
                            new SolidColorBrush(ColorsView.ForegroundSelected));

                        DrawingUtilities.SaveImageSource(imageSource, new PngBitmapEncoder(), newFilePath);
                    }
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Remove Size Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void RemoveSizeButton_Click(object sender, RoutedEventArgs e)
        {
            var size = (e.Source as Control)?.DataContext as Data.Models.Size;

            if (size != null)
                SizeView.RemoveSize(size);
        }

        #endregion EXPORT MANAGEMENT METHODS

        #region ITEMS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing key in focused Search TextBox. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Key Event Arguments. </param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PackIconKindView.SearchPhrase = (sender as TextBox)?.Text;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Search Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PackIconKindView.SearchPhrase = SearchTextBox.Text;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Previous Page Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            PackIconKindView.PageIndex--;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking on Next Page Button. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            PackIconKindView.PageIndex++;
        }

        //  --------------------------------------------------------------------------------
        private void PageCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PackIconKindView.ItemsPerPage = PageCountView.PageCount;
        }

        #endregion ITEMS MANAGEMENT METHODS

    }
}

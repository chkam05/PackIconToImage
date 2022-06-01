using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace chkam05.PackIconToImage.Data
{
    public class ColorsView : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler PropertyChanged;


        //  VARIABLES

        private ObservableCollection<Color> _colorsItems;
        private Color _backgroundColor = Colors.Transparent;
        private Color _foregroundColor = Colors.Black;


        //  GETTERS & SETTERS

        public ObservableCollection<Color> Items
        {
            get => _colorsItems;
            set
            {
                _colorsItems = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public Color BackgroundSelected
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundSelected));
            }
        }

        public Color ForegroundSelected
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged(nameof(ForegroundSelected));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ColorsView class constructor. </summary>
        public ColorsView()
        {
            Items = new ObservableCollection<Color>(typeof(Colors)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(p => (Color)p.GetValue(null)));
        }

        #endregion CLASS METHODS

        #region NOTIFY PROPERTIES CHANGED INTERFACE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method for invoking PropertyChangedEventHandler external method. </summary>
        /// <param name="propertyName"> Changed property name. </param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion NOTIFY PROPERTIES CHANGED INTERFACE METHODS
    }
}

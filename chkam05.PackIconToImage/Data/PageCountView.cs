using chkam05.PackIconToImage.Data.Static;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chkam05.PackIconToImage.Data
{
    public class PageCountView : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler PropertyChanged;


        //  VARIABLES

        private ObservableCollection<PageCount> _pageCountItems;
        private PageCount _selectedPageCount = Static.PageCount.PageCount100;


        //  GETTERS & SETTERS

        public ObservableCollection<PageCount> Items
        {
            get => _pageCountItems;
            set
            {
                _pageCountItems = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public int PageCount
        {
            get => (int)Selected;
        }

        public PageCount Selected
        {
            get => _selectedPageCount;
            set
            {
                _selectedPageCount = value;
                OnPropertyChanged(nameof(Selected));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> PageCountView class constructor. </summary>
        public PageCountView()
        {
            Items = new ObservableCollection<PageCount>(
                Enum.GetValues(typeof(PageCount)).Cast<PageCount>());
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

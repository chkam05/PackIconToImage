using chkam05.PackIconToImage.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chkam05.PackIconToImage.Data
{
    public class SizeView : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler PropertyChanged;


        //  VARIABLES

        private double _editableSizeHeight = 0;
        private double _editableSizeWidth = 0;
        private ObservableCollection<Size> _sizeItems;


        //  GETTERS & SETTERS

        public double EditableSizeHeight
        {
            get => _editableSizeHeight;
            set
            {
                _editableSizeHeight = Math.Max(0, value);
                OnPropertyChanged(nameof(EditableSizeHeight));
            }
        }

        public double EditableSizeWidth
        {
            get => _editableSizeWidth;
            set
            {
                _editableSizeWidth = Math.Max(0, value);
                OnPropertyChanged(nameof(EditableSizeWidth));
            }
        }

        public ObservableCollection<Size> Items
        {
            get => _sizeItems;
            set
            {
                _sizeItems = value;
                OnPropertyChanged(nameof(Size));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> SizeView class constructor. </summary>
        public SizeView()
        {
            Items = new ObservableCollection<Size>();
            _sizeItems.CollectionChanged += ContentCollectionChanged;
        }

        #endregion CLASS METHODS

        #region ITEMS INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Add size item to collection. </summary>
        /// <param name="size"> Size item to add. </param>
        public void AddSize(Size size)
        {
            Items.Add(size);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Remove size item from collection. </summary>
        /// <param name="size"> Item to remove. </param>
        public void RemoveSize(Size size)
        {
            if (Items.Any(s => s.Equals(size)))
                Items.Remove(size);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Remove size item from collection at specified index. </summary>
        /// <param name="sizeIndex"> Index of size item to remove. </param>
        public void RemoveSizeAt(int sizeIndex)
        {
            if (sizeIndex >= 0 && sizeIndex < Items.Count)
                Items.RemoveAt(sizeIndex);
        }

        #endregion ITEMS INTERACTION METHODS

        #region NOTIFY PROPERTIES CHANGED INTERFACE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing data collection. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Size item in e.OldItems)
                    item.PropertyChanged -= OnItemPropertyChanged;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Size item in e.NewItems)
                    item.PropertyChanged += OnItemPropertyChanged;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method for invoking PropertyChangedEventHandler external method. </summary>
        /// <param name="propertyName"> Changed property name. </param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after chaning any value in item. </summary>
        /// <param name="sender"> Object that invoked method. </param>
        /// <param name="e"> Property Changed Event Arguments. </param>
        protected void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //
        }

        #endregion NOTIFY PROPERTIES CHANGED INTERFACE METHODS

    }
}

using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chkam05.PackIconToImage.Data
{
    public class PackIconKindView : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler PropertyChanged;


        //  VARIABLES

        private List<PackIconKind> _data;
        private int _itemsPerPage = 100;
        private bool _lockUpdate = false;
        private ObservableCollection<PackIconKind> _packIconKindsItems;
        private int _pageIndex = 1;
        private int _pages = 0;
        private string _searchPhrase = string.Empty;
        private PackIconKind _selectedIconKind;


        //  GETTERS & SETTERS

        public ObservableCollection<PackIconKind> Items
        {
            get => _packIconKindsItems;
            set
            {
                _packIconKindsItems = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                _itemsPerPage = Math.Min(Math.Max(10, value), int.MaxValue);
                OnPropertyChanged(nameof(ItemsPerPage));

                if (!_lockUpdate)
                    UpdateView();
            }
        }

        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                _pageIndex = Math.Max(1, value);
                OnPropertyChanged(nameof(PageIndex));

                if (!_lockUpdate)
                    UpdateView();
            }
        }

        public int Pages
        {
            get => _pages;
            set
            {
                _pages = Math.Max(0, value);
                OnPropertyChanged(nameof(Pages));
            }
        }

        public string SearchPhrase
        {
            get => _searchPhrase;
            set
            {
                _searchPhrase = value;
                OnPropertyChanged(nameof(SearchPhrase));

                if (!_lockUpdate)
                    InvokeLock(() =>
                    {
                        PageIndex = 1;
                        UpdateView();
                    });
            }
        }

        public PackIconKind Selected
        {
            get => _selectedIconKind;
            set
            {
                _selectedIconKind = value;
                OnPropertyChanged(nameof(Selected));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> PackIconKindView class constructor. </summary>
        public PackIconKindView()
        {
            Items = new ObservableCollection<PackIconKind>();

            SetupData();
            UpdateView();
        }

        #endregion CLASS METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Update items view. </summary>
        private void UpdateView()
        {
            if (!string.IsNullOrEmpty(SearchPhrase))
            {
                var foundItems = _data.Where(i => Enum.GetName(typeof(PackIconKind), i).Contains(SearchPhrase)).ToList();
                UpdateView(foundItems);
                return;
            }

            UpdateView(_data);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update items view from specified list of items. </summary>
        /// <param name="packIcons"> List of items to show. </param>
        private void UpdateView(List<PackIconKind> packIcons)
        {
            int count = packIcons.Count;
            Pages = count / ItemsPerPage +
                ((double)count / ItemsPerPage > (int)count / ItemsPerPage ? 1 : 0);

            //  Calculate start item index.
            int startIndex = (PageIndex - 1) * ItemsPerPage;

            if (startIndex >= count)
                startIndex = count - count % ItemsPerPage;

            //  Calculate amount of items to show.
            bool isLast = startIndex + ItemsPerPage > count;
            int grabCount = isLast ? count - startIndex : ItemsPerPage;

            //  Show items.
            Items.Clear();
            packIcons.GetRange(startIndex, grabCount).ForEach(i => Items.Add(i));

            //  Update page with update lock.
            InvokeLock(() => {
                if (PageIndex > Pages)
                    PageIndex = Pages;
            });
        }

        #endregion INTERACTION METHODS

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

        #region SETUP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Setup data method. </summary>
        private void SetupData()
        {
            _data = Enum.GetValues(typeof(PackIconKind)).Cast<PackIconKind>().ToList();

            /*_data = Enum.GetValues(typeof(PackIconKind)).Cast<PackIconKind>()
                .GroupBy(i => Enum.GetName(typeof(PackIconKind), i))
                .Select(g => g.First())
                .ToList();*/
        }

        #endregion SETUP METHODS

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Invoke update action locking rest of updates. </summary>
        /// <param name="action"> Update action to invoke. </param>
        private void InvokeLock(Action action)
        {
            _lockUpdate = true;
            action?.Invoke();
            _lockUpdate = false;
        }

        #endregion UTILITY METHODS

    }
}

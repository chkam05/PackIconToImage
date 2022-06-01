using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chkam05.PackIconToImage.Data.Models
{
    public class Size : INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler PropertyChanged;


        //  VARIABLES

        private double _height;
        private double _width;


        //  GETTERS & SETTERS

        public double Height
        {
            get => _height;
            set
            {
                _height = Math.Max(0, value);
                OnPropertyChanged(nameof(Height));
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = Math.Max(0, value);
                OnPropertyChanged(nameof(Width));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Size class constructor. </summary>
        /// <param name="width"> Width. </param>
        /// <param name="height"> Hegith. </param>
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        #endregion CLASS METHODS

        #region COMPARE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <returns> True - specified object is equal to the current; False - otherwise. </returns>
        public override bool Equals(object obj)
        {
            Size size = obj as Size;

            if (size != null)
                return size.Width == Width && size.Height == Height;

            return false;
        }

        #endregion COMPARE METHODS

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

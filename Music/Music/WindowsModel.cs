using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Windows.Model
{
    class Windows : INotifyPropertyChanged

    {
        private int _width;

        private int _height;

        private int Width
        {
            get
            {
                return _width;
            } 
            set
            {
                if (value != _width)
                {
                    value = _width;
                    NotifyChanged("Width");
                }
            }
        }

        private int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value != _height)
                {
                    value = _height;
                    NotifyChanged("Height");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanged(string propertyName)

        {

        if (PropertyChanged != null)

            {

              PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }

        }

    }
}

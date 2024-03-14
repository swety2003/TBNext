using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TB.Shared.Utils;

namespace TB.Shared.Controls
{
    public class MyViewBase<T> : UserControl where T : ViewModelBase, new()
    {
        
        private T? viewmodel;

        public T? VM => viewmodel;

        public MyViewBase()
        {
            viewmodel = new T();
            viewmodel.SetView(this);
            DataContext = viewmodel;


            
        }


    }

    public class MyViewBase : UserControl
    {
        public MyViewBase()
        {
            throw new NotSupportedException();
        }
    }
}

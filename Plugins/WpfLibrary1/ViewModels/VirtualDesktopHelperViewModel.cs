using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TB.Shared.Utils;
using WindowsDesktop;
using WpfLibrary1.Views;

namespace WpfLibrary1.ViewModels
{

    public partial class VirtualDesktopHelperViewModel : ViewModelBase
    {
        public override void Init()
        {
            base.Init();

            OnEnabled();
        }
        public override void Update()
        {
            //UpdateVD();

        }

        private List<VirtualDesktop> desktops;

        [ObservableProperty] private ObservableCollection<VirtualDesktop> _virtualDesktops;

        [ObservableProperty] private VirtualDesktop _focusedDesktop;

        [ObservableProperty] private int _focusedIndex;


        private void VirtualDesktopOnCreated(object? sender, VirtualDesktop e)
        {
            //UpdateVD();
            GetView<VirtualDesktopHelper>().Dispatcher.Invoke(() =>
            {
                VirtualDesktops.Add(e);

            });
            e.Switch();

        }

        private void VirtualDesktopOnCurrentChanged(object? sender, VirtualDesktopChangedEventArgs e)
        {
            FocusedDesktop = e.NewDesktop;
        }
        [RelayCommand]
        public void NewVD()
        {
            VirtualDesktop.Create();
        }


        internal void OnEnabled()
        {

            UpdateVD();
                FocusedDesktop = VirtualDesktop.Current??throw new Exception();
                VirtualDesktop.CurrentChanged += VirtualDesktopOnCurrentChanged;
                VirtualDesktop.Created += VirtualDesktopOnCreated;
            VirtualDesktop.Destroyed += VirtualDesktop_Destroyed;
            VirtualDesktop.Moved += VirtualDesktop_Moved;
            //});

        }

        private void VirtualDesktop_Moved(object? sender, VirtualDesktopMovedEventArgs e)
        {
            GetView<VirtualDesktopHelper>().Dispatcher.Invoke(() =>
            {
                VirtualDesktops.Move(e.OldIndex, e.NewIndex);

            });
        }

        private void VirtualDesktop_Destroyed(object? sender, VirtualDesktopDestroyEventArgs e)
        {
            //foreach (VirtualDesktop desktop in VirtualDesktops.ToArray())
            //{
            //    if (desktop.Id == e.Destroyed.Id) VirtualDesktops.Remove(desktop);
            //}
            GetView<VirtualDesktopHelper>().Dispatcher.Invoke(() =>
            {
                VirtualDesktops.Remove(e.Destroyed);

            });

        }

        void UpdateVD()
        {

            VirtualDesktop.Configure();
            var d = VirtualDesktop.GetDesktops();
            VirtualDesktops = new ObservableCollection<VirtualDesktop>(d);
            //foreach (var item in d)
            //{
            //    if (VirtualDesktops.Contains(item))
            //    {
            //        continue;
            //    }
            //    VirtualDesktops.Add(item);
            //}

            //foreach (var item in VirtualDesktops.ToList())
            //{
            //    if (!d.Contains(item))
            //    {
            //        VirtualDesktops.Remove(item);
            //    }
            //}
            //VirtualDesktops = new ObservableCollection<VDItem>(desktops.Select(x =>
            //    new VDItem(desktops.IndexOf(x).ToString(),
            //        VDItemType.Normal, x
            //    )).ToList());
        }
    }
}

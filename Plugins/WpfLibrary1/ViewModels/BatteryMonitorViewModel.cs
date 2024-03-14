using CommunityToolkit.Mvvm.ComponentModel;
using System.Text;
using TB.Shared.Utils;
using Windows.Win32;
using Windows.Win32.System.Power;

namespace WpfLibrary1.ViewModels
{
    public partial class BatteryMonitorViewModel : ViewModelBase
    {
        [ObservableProperty] private int lifePercent;

        [ObservableProperty] private string icon;
        public override void Update()

        {
            TimerOnTick();
        
        }

        private void TimerOnTick()
        {
            SYSTEM_POWER_STATUS status;
            PInvoke.GetSystemPowerStatus(out status);
            LifePercent = status.BatteryLifePercent;
            var utf8 = Encoding.UTF8;
            switch (status.ACLineStatus)
            {
                case 0:
                    {   // offline
                        int index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 10));
                        // Icon = offlineIcon[index]
                        char c = '\uF5F2';
                        Icon = ((char)(c + index)).ToString();


                    }
                    break;
                case 1:
                    {
                        //online
                        int index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 10));
                        // Icon = onlineIcon[index];
                        char c = '\uF5FD';

                        Icon = ((char)(c + index)).ToString();

                    }
                    break;
                case 255:
                    {
                        Icon = "&#xe1a6;";
                    }
                    break;
                default:
                    {
                        Icon = "&#xe1a6;";

                    }
                    break;
            }

        }
    }
}
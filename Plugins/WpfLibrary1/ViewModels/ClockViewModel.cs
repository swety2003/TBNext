using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Utils;

namespace WpfLibrary1.ViewModels
{

    public partial class ClockViewModel : ViewModelBase
    {
        public override void Init()
        {
            base.Init();

            Update();
        }
        [ObservableProperty] private DateTime nowTime;

        [ObservableProperty] private double hourDeg;

        [ObservableProperty] private double minDeg;

        [ObservableProperty] private double secondDeg;

        public override void Update()
        {

            NowTime = DateTime.Now;

            HourDeg = NowTime.Hour * 30 + NowTime.Minute * 30 / 60 - 90;

            MinDeg = NowTime.Minute * 6 + NowTime.Second * 6 / 60 - 90;

            SecondDeg = NowTime.Second * 6 - 90;
        }
    }
}

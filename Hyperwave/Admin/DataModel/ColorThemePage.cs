using Hyperwave.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hyperwave.Config;

namespace Hyperwave.Admin.DataModel
{
    class ColorThemePage : ConfigPage
    {
        SkinStyle mColorScheme;

        public ColorThemePage(ConfigPage parent)
            :base(parent)
        {
            mColorScheme = Settings.Default.ColorScheme;
        }

        public override string Name
        {
            get
            {
                return "Themes & Colors";
            }
        }

        protected override Uri ImageUri
        {
            get
            {
                return new Uri("pack://application:,,,/Images/Icons/32/Themes.png");
            }
        }

        public override void Apply()
        {
            Settings.Default.ColorScheme = mColorScheme;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (HasChanged)
                GetApp().SetStyle(Settings.Default.ColorScheme);
        }

        public SkinStyle ColorScheme
        {
            get
            {
                return mColorScheme;
            }
            set
            {
                if (mColorScheme == value)
                    return;

                mColorScheme = value;
                GetApp().SetStyle(mColorScheme);

                OnPropertyChanged("ColorScheme");
                OnPropertyChanged("GallenteSelected");
                OnPropertyChanged("AmarrSelected");
                OnPropertyChanged("MinmatarSelected");
                OnPropertyChanged("CaldariSelected");
            }
        }

        public bool GallenteSelected
        {
            get { return ColorScheme == SkinStyle.Gallente; }
            set
            {
                if (value)
                {
                    ColorScheme = SkinStyle.Gallente;
                }
            }
        }
        public bool AmarrSelected
        {
            get { return ColorScheme == SkinStyle.Amarr; }
            set
            {
                if (value)
                {
                    ColorScheme = SkinStyle.Amarr;
                }
            }
        }
        public bool MinmatarSelected
        {
            get { return ColorScheme == SkinStyle.Minmatar; }
            set
            {
                if (value)
                {
                    ColorScheme = SkinStyle.Minmatar;
                }
            }
        }
        public bool CaldariSelected
        {
            get { return ColorScheme == SkinStyle.Caldari; }
            set
            {
                if (value)
                {
                    ColorScheme = SkinStyle.Caldari;
                }
            }
        }

        public override bool HasChanged
        {
            get
            {
                return Settings.Default.ColorScheme != mColorScheme;
            }
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace NFRuntime.Managements.OptionManagement
{
    public class OptionSound : ISoundVolume, INotifyPropertyChanged
    {
        public OptionSound(IOptionStorage configdb)
        {
            this.Configdb = configdb;
        }

        private IOptionStorage Configdb { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public float VolumeMaster
        {
            get { return this.Configdb.GetFloat(nameof(this.VolumeMaster), 1); }
            set
            {
                Assert.IsTrue(value >= 0, $"value upper zero : {value}");
                Assert.IsTrue(value <= 1, $"value under one : {value}");
                this.Configdb.SetFloat(nameof(this.VolumeMaster), value);
                OnPropertyChanged();
            }
        }

        public float VolumeBgm
        {
            get { return this.Configdb.GetFloat(nameof(this.VolumeBgm), 1); }
            set
            {
                Assert.IsTrue(value >= 0, $"value upper zero : {value}");
                Assert.IsTrue(value <= 1, $"value under one : {value}");
                this.Configdb.SetFloat(nameof(this.VolumeBgm), value);
                OnPropertyChanged();
            }
        }

        public float VolumeEffect
        {
            get { return this.Configdb.GetFloat(nameof(this.VolumeEffect), 1); }
            set
            {
                Assert.IsTrue(value >= 0, $"value upper zero : {value}");
                Assert.IsTrue(value <= 1, $"value under one : {value}");
                this.Configdb.SetFloat(nameof(this.VolumeEffect), value);
                OnPropertyChanged();
            }
        }

        public bool IsMute
        {
            get { return this.Configdb.GetBool(nameof(this.IsMute)); }
            set
            {
                this.Configdb.SetBool(nameof(this.IsMute), value);
                OnPropertyChanged();
            }
        }

        public float GetVolumeBgm()
        {
            if (this.IsMute)
            {
                return 0;
            }

            return this.VolumeMaster * this.VolumeBgm;
        }

        public float GetVolumeEffect()
        {
            if (this.IsMute)
            {
                return 0;
            }

            return this.VolumeMaster * this.VolumeEffect;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler == null)
            {
                return;
            }

            Assert.IsTrue(!string.IsNullOrEmpty(propertyName), $"name error : {propertyName}");
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return
                $"IsMute : {this.IsMute} | Master : {this.VolumeMaster} | BGM : {this.VolumeBgm} | Effect : {this.VolumeEffect}";
        }
    }
}
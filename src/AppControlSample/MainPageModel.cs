/*
 * Copyright (c) 2019 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tizen.Applications;
using Tizen.Applications.Messages;
using Xamarin.Forms;

namespace AppControlSample
{
    public class MainPageModel : INotifyPropertyChanged
    {
        MessagePort _rmtPort;
        /// <summary>
        /// Constructor
        /// </summary>
        public MainPageModel()
        {
            LocationServiceEnabled = true;
            SensorServiceEnabled = true;
        }

        public void Init()
        {
            Mylabel = "Hello Tizen.";
            LocationServiceEnabled = false;
            SensorServiceEnabled = false;
            _rmtPort = new MessagePort("my_port", false);
            _rmtPort.MessageReceived += _rmtPort_MessageReceived;
            _rmtPort.Listen();
        }

        /// <summary>
        /// Invoked when a message has been received
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">MessageReceivedEventArgs</param>
        private void _rmtPort_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Mylabel = e.Message.GetItem<string>(Utility.LocationKey) + "\n\n" + Mylabel;
        }

        /// <summary>
        /// Result text
        /// </summary>
        string _label;
        public string Mylabel
        {
            get => _label;
            set => SetProperty(ref _label, value, "Mylabel");
        }

        /// <summary>
        /// Indicate whether location service is started or not.
        /// </summary>
        bool _locationServiceEnabled;
        public bool LocationServiceEnabled
        {
            get => _locationServiceEnabled;
            set => SetProperty(ref _locationServiceEnabled, value, "LocationServiceEnabled");
        }

        /// <summary>
        /// Indicate whether sensor service is started or not.
        /// </summary>
        bool _sensorServiceEnabled;
        public bool SensorServiceEnabled
        {
            get => _sensorServiceEnabled;
            set => SetProperty(ref _sensorServiceEnabled, value, "SensorServiceEnabled");
        }

        /// <summary>
        /// </summary>
        public ICommand StartLocationService => new Command(StartLocation);

        /// <summary>
        /// </summary>
        void StartLocation()
        {
            Tizen.Log.Info(Program.LOG_TAG, "<< StartLocation()");

            AppControl appcontrol = new AppControl()
            {
                ApplicationId = Utility.ServiceAppID,
                Operation = Utility.LocationOn,
            };
            Mylabel = "StartLocation : appcontrol " + appcontrol.Operation;
            AppControl.SendLaunchRequest(appcontrol);

            //AppControl appcontrol = new AppControl()
            //{
            //    ApplicationId = Utility.ServiceAppID,
            //};
            //appcontrol.ExtraData.Add("location", "start");
            //Tizen.Log.Info(Program.LOG_TAG, "####   ExtraData : " + appcontrol.ExtraData.Count());
            //if (appcontrol.ExtraData != null)
            //{
            //    foreach (string s in appcontrol.ExtraData.GetKeys())
            //    {
            //        Tizen.Log.Info(Program.LOG_TAG, "####   key = : " + s);
            //    }
            //}
            //Mylabel = "StartLocation : appcontrol " + appcontrol.Operation;
            //Tizen.Log.Info(Program.LOG_TAG, "appcontrol : " + appcontrol);
            //AppControl.SendLaunchRequest(appcontrol);

            LocationServiceEnabled = true;
            Tizen.Log.Info(Program.LOG_TAG, ">> StartLocation()");
        }

        /// <summary>
        /// </summary>
        public ICommand StopLocationService => new Command(StopLocation);

        /// <summary>
        /// </summary>
        void StopLocation()
        {
            AppControl appcontrol = new AppControl()
            {
                ApplicationId = Utility.ServiceAppID,
                Operation = Utility.LocationOff,
            };
            Mylabel = "StopLocation : appcontrol " + appcontrol.Operation;
            AppControl.SendLaunchRequest(appcontrol);
            LocationServiceEnabled = false;
        }

        /// <summary>
        /// </summary>
        public ICommand StartSensorService => new Command(StartSensor);

        /// <summary>
        /// </summary>
        void StartSensor()
        {
        }

        /// <summary>
        /// </summary>
        public ICommand StopSensorService => new Command(StopSensor);

        /// <summary>
        /// </summary>
        void StopSensor()
        {
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Called to notify that a change of property happened
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

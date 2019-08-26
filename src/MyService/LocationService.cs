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
using Tizen.Applications;
using Tizen.Applications.Messages;
using Tizen.Location;

namespace MyService
{
    class LocationService
    {
        private static readonly Lazy<LocationService> instance = new Lazy<LocationService>(() => new LocationService());
        MessagePort _msgPort;
        private Locator locator = null;

        public static LocationService Instance
        {
            get => instance.Value;
        }

        private LocationService()
        {
            _msgPort = new MessagePort("my_port", false);
            _msgPort.Listen();
        }

        /// <summary>
        /// Start location service
        /// </summary>
        public void StartLocationService()
        {           
            try
            {
                locator = new Locator(LocationType.Hybrid);
                locator.ServiceStateChanged += Locator_ServiceStateChanged;
                locator.LocationChanged += Locator_LocationChanged;
                locator.SettingChanged += Locator_SettingChanged;
                locator.DistanceBasedLocationChanged += Locator_DistanceBasedLocationChanged;
                locator.Start();
            }
            catch (Exception e)
            {
                Tizen.Log.Info(App.LogTag, "[StartLocationService] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
                SendMessage("[StartLocationService] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
            }
        }

        /// <summary>
        /// Stop location service
        /// </summary>
        public void StopLocationService()
        {
            try
            {
                locator.Stop();
                locator.ServiceStateChanged -= Locator_ServiceStateChanged;
                locator.LocationChanged -= Locator_LocationChanged;
                locator.SettingChanged -= Locator_SettingChanged;
                locator.DistanceBasedLocationChanged -= Locator_DistanceBasedLocationChanged;
                locator.Dispose();
            }
            catch (Exception e)
            {
                Tizen.Log.Info(App.LogTag, "[StopLocationService] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
                SendMessage("[StopLocationService] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
            }
        }

        /// <summary>
        /// Send message to UI application using MessagePort API
        /// </summary>
        /// <param name="txt">text to send</param>
        private void SendMessage(string txt)
        {
            try
            {
                var msg = new Bundle();
                msg.AddItem(Utility.LocationKey, txt);
                _msgPort.Send(msg, Utility.RemoteAppId, Utility.RemotePort);
            }
            catch (Exception e)
            {
                Tizen.Log.Info(App.LogTag, "[MessagePortSend] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
                SendMessage("[MessagePortSend] Error occurred : " + e.GetType() + ", " + e.Message + ", " + e.StackTrace);
            }
        }

        /// <summary>
        /// Invoked when the setting of location has been changed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">SettingChangedEventArgs</param>
        private void Locator_SettingChanged(object sender, SettingChangedEventArgs e)
        {
            //Tizen.Log.Info(App.LogTag, "[SettingChanged] LocationType: " + e.LocationType.ToString() + ", " + e.IsEnabled);
            SendMessage("[SettingChanged] LocationType: " + e.LocationType.ToString() + ", " + e.IsEnabled);
        }

        /// <summary>
        /// Invoked at intervals with the updated location data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">LocationChangedEventArgs</param>
        private void Locator_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            //Tizen.Log.Info(App.LogTag, "[LocationChanged] " + e.Location.Timestamp + "," + e.Location.Latitude + ", " + e.Location.Longitude + ", " + e.Location.Altitude);
            SendMessage("[LocationChanged]" + e.Location.Timestamp + "," + e.Location.Latitude + ", " + e.Location.Longitude + ", " + e.Location.Altitude);
        }

        /// <summary>
        /// Invoked when location service's state has been changed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ServiceStateChangedEventArgs</param>
        private void Locator_ServiceStateChanged(object sender, ServiceStateChangedEventArgs e)
        {
            //Tizen.Log.Info(App.LogTag, "[LocatorServiceStateChanged]  " + e.ServiceState.ToString());
            SendMessage("[LocatorServiceStateChanged]  " + e.ServiceState.ToString());
        }

        /// <summary>
        /// Invoked at a minimum interval or distance with the updated location data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">LocationChangedEventArgs</param>
        private void Locator_DistanceBasedLocationChanged(object sender, LocationChangedEventArgs e)
        {
            //Tizen.Log.Info(App.LogTag, "[DistanceBasedLocationChanged]  " + e.Location.Timestamp + ", " + e.Location.ToString());
            SendMessage("[DistanceBasedLocationChanged] " + e.Location.Timestamp + "," + e.Location.Latitude + ", " + e.Location.Longitude + ", " + e.Location.Altitude);
        }
    }
}

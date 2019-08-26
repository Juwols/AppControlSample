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

using Tizen.Applications;

namespace MyService
{
    class App : ServiceApplication
    {
        public const string LogTag = "MyService";
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        /// <summary>
        /// Invoked when application receives the appcontrol request.
        /// </summary>
        /// <param name="e">AppControlReceivedEventArgs</param>
        protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            base.OnAppControlReceived(e);
            Tizen.Log.Info(LogTag, "####   OnAppControlReceived - ReceivedAppControlOperation :" + e.ReceivedAppControl.Operation);

            #region usingExtradata
            // in case of using extradata,
            //{
            //Tizen.Log.Info(LogTag, "####   ExtraData : " + e.ReceivedAppControl.ExtraData.Count());
            //if (e.ReceivedAppControl.ExtraData != null)
            //{
            //    foreach (string s in e.ReceivedAppControl.ExtraData.GetKeys())
            //    {
            //        Tizen.Log.Info(LogTag, "####   key = : " + s);
            //    }
            //}
            //string type = e.ReceivedAppControl.ExtraData.Get<string>("location");
            //if (string.Compare(type, "start") == 0)
            //{
            //    LocationService.Instance.StartLocationService();
            //}
            //else if (string.Compare(type, "stop") == 0)
            //{
            //    LocationService.Instance.StopLocationService();
            //}
            #endregion

            // Based on the operation, execute proper method
            if (string.Compare(e.ReceivedAppControl.Operation, Utility.LocationOn) == 0)
            {
                // start location service
                LocationService.Instance.StartLocationService();
            }
            else if (string.Compare(e.ReceivedAppControl.Operation, Utility.LocationOff) == 0)
            {
                // stop location service
                LocationService.Instance.StopLocationService();
            }
            else if (string.Compare(e.ReceivedAppControl.Operation, Utility.LocationOn) == 0)
            {
                // start sensor service
            }
            else if (string.Compare(e.ReceivedAppControl.Operation, Utility.LocationOff) == 0)
            {
                // stop sensor service
            }
        }

        /// <summary>
        /// Invoked when device's orientation has been changed
        /// </summary>
        /// <param name="e">DeviceOrientationEventArgs</param>
        protected override void OnDeviceOrientationChanged(DeviceOrientationEventArgs e)
        {
            base.OnDeviceOrientationChanged(e);
        }

        /// <summary>
        /// Invoked when the device's locale has been changed
        /// </summary>
        /// <param name="e">LocaleChangedEventArgs</param>
        protected override void OnLocaleChanged(LocaleChangedEventArgs e)
        {
            base.OnLocaleChanged(e);
        }

        /// <summary>
        /// Invoked when the battery is low
        /// </summary>
        /// <param name="e">LowBatteryEventArgs</param>
        protected override void OnLowBattery(LowBatteryEventArgs e)
        {
            base.OnLowBattery(e);
        }

        /// <summary>
        /// Invoked when memory is low
        /// </summary>
        /// <param name="e">LowMemoryEventArgs</param>
        protected override void OnLowMemory(LowMemoryEventArgs e)
        {
            base.OnLowMemory(e);
        }

        /// <summary>
        /// Invoked when the region format has been changed
        /// </summary>
        /// <param name="e">RegionFormatChangedEventArgs</param>
        protected override void OnRegionFormatChanged(RegionFormatChangedEventArgs e)
        {
            base.OnRegionFormatChanged(e);
        }

        protected override void OnTerminate()
        {
            base.OnTerminate();
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}

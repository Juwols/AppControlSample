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

namespace MyService
{
    public static class Utility
    {
        public static string SensorOn = "http://tizen.org/appcontrol/operation/start_sensor_service";
        public static string SensorOff = "http://tizen.org/appcontrol/operation/stop_sensor_service";
        public static string LocationOn = "http://tizen.org/appcontrol/operation/start_location_service";
        public static string LocationOff = "http://tizen.org/appcontrol/operation/stop_location_service";
        public static string LocationKey = "location";

        public static string RemoteAppId = "org.tizen.example.AppControlSample";
        public static string RemotePort = "my_port";
    }
}
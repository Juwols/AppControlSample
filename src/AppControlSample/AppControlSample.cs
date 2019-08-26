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
using System.Threading.Tasks;
using Tizen.Applications;
using Tizen.Security;

namespace AppControlSample
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        App app;
        public const string LOG_TAG = "AppControlSample";

        // Location privilege
        public const string LocationPrivilege = "http://tizen.org/privilege/location";
        // HealthInfo privilege
        public const string HealthInfoPrivilege = "http://tizen.org/privilege/healthinfo";
        protected override void OnCreate()
        {
            Tizen.Log.Info(LOG_TAG, " <<< Program :OnCreate ");
            base.OnCreate();
            app = new App();
            LoadApplication(app);
            Tizen.Log.Info(LOG_TAG, " >>> Program :OnCreate ");
        }

        protected async override void OnResume()
        {
            Tizen.Log.Info(LOG_TAG, " <<< Program :OnResume ");
            base.OnResume();

            bool locationGranted = true/*, healthInfoGranted = true*/;
            Tizen.Log.Info(LOG_TAG, " <<< LocationPrivilege : " + PrivacyPrivilegeManager.CheckPermission(LocationPrivilege).ToString());
            //Tizen.Log.Info(LOG_TAG, " <<< HealthInfoPrivilege : " + PrivacyPrivilegeManager.CheckPermission(HealthInfoPrivilege).ToString());

            if (PrivacyPrivilegeManager.CheckPermission(LocationPrivilege) != CheckResult.Allow)
            {
                locationGranted = await RequestPermission(LocationPrivilege);
                Tizen.Log.Info(LOG_TAG, " >>> locationGranted : " + locationGranted);
            }

            //if (PrivacyPrivilegeManager.CheckPermission(HealthInfoPrivilege) != CheckResult.Allow)
            //{
            //    healthInfoGranted = await RequestPermission(HealthInfoPrivilege);
            //    Tizen.Log.Info(LOG_TAG, " >>> healthInfoGranted : " + healthInfoGranted);
            //}

            if (!locationGranted)
            {
                Tizen.Log.Error(LOG_TAG, "Failed to obtain user consent.");
                // Terminate this application.
                Application.Current.Exit();
                return;
            }

            Tizen.Log.Info(LOG_TAG, " >>> Program :OnResume ");
            app.PageModel.Mylabel = ">>> Program :OnResume " + DateTime.Now.ToString("h:mm:ss tt") + "\n" + app.PageModel.Mylabel;
        }

        protected override void OnPause()
        {
            base.OnPause();
            app.PageModel.Mylabel = ">>> Program :OnPause " + DateTime.Now.ToString("h:mm:ss tt") + "\n" + app.PageModel.Mylabel;
        }

        static Task<bool> RequestPermission(string privilege)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var response = PrivacyPrivilegeManager.GetResponseContext(privilege);
            PrivacyPrivilegeManager.ResponseContext target;
            response.TryGetTarget(out target);
            target.ResponseFetched += (s, e) =>
            {
                if (e.cause == CallCause.Error)
                {
                    /// Handle errors
                    Tizen.Log.Error(LOG_TAG, "An error occurred while requesting an user permission");
                    tcs.SetResult(false);
                    return;
                }

                // Now, we can check if the permission is granted or not
                switch (e.result)
                {
                    case RequestResult.AllowForever:
                        // Permission is granted.
                        // We can do this permission-related task we want to do.
                        Tizen.Log.Debug(LOG_TAG, "Response: RequestResult.AllowForever");
                        tcs.SetResult(true);
                        break;
                    case RequestResult.DenyForever:
                    case RequestResult.DenyOnce:
                        // Functionality that depends on this permission will not be available
                        Tizen.Log.Debug(LOG_TAG, "Response: RequestResult." + e.result.ToString());
                        tcs.SetResult(false);
                        break;
                }

            };
            PrivacyPrivilegeManager.RequestPermission(privilege);

            return tcs.Task;
        }


        static void Main(string[] args)
        {
            var app = new Program();
            global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);
            global::Tizen.Wearable.CircularUI.Forms.Renderer.FormsCircularUI.Init();
            app.Run(args);
        }
    }
}

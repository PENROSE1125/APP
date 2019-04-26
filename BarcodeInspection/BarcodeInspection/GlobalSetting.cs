using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeInspection
{
    public class GlobalSetting
    {
        //GITHUB테스트
        //git서버에 반영하려면 Command & Push
        //      private string DefaultEndpoint = "http://wms.dsbestco.co.kr";
        private string DefaultEndpoint = "http://172.28.200.48:8080";

        private string _baseEndpoint = string.Empty;
        private static readonly GlobalSetting _instance = new GlobalSetting();

        public GlobalSetting()
        {

            DefaultEndpoint = "http://172.28.200.48:8080";

            AuthToken = "INSERT AUTHENTICATION TOKEN";
            BaseEndpoint = DefaultEndpoint;
        }

        public static GlobalSetting Instance
        {
            get { return _instance; }
        }

        public string BaseEndpoint
        {
            get { return _baseEndpoint; }
            set
            {
                _baseEndpoint = value;
                UpdateEndpoint(_baseEndpoint);
            }
        }
        public string AuthToken { get; set; }

        public string ClientId { get { return "BarcodeInspection"; } }

        public string BCWMSEndpoint { get; set; }

        public string MOBILEEndpoint { get; set; }

        public string MobileGetEndpoint { get; set; }
        public string MobileSetEndpoint { get; set; }

        public string MobileAuthEndpoint { get; set; }

        private void UpdateEndpoint(string baseEndpoint)
        {
            MOBILEEndpoint = $"{baseEndpoint}/xamarin";
            MobileGetEndpoint = $"{baseEndpoint}/xamarin/GetServlet";
            MobileSetEndpoint = $"{baseEndpoint}/xamarin/SetServletForJSON";
            MobileAuthEndpoint = $"{baseEndpoint}/xamarin/AuthorizationServer";
        }
    }
}

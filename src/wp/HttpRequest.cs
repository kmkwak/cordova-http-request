using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.Generic;

#if WP8
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;

//Use alias in case Cordova File Plugin is enabled. Then the File class will be declared in both and error will occur.
using IOFile = System.IO.File;


#else
using Microsoft.Phone.Tasks;
#endif

namespace WPCordovaClassLib.Cordova.Commands
{
    class HttpRequest : BaseCommand
    {
        public void echo(string options)
        {
            string argsString = JSON.JsonHelper.Deserialize<string[]>(options)[0];
            string[] args = JSON.JsonHelper.Deserialize<string[]>(argsString);
            string url = args[0];
            string cookie = args[1];
            KeyValuePair<string, string>[] headers = JSON.JsonHelper.Deserialize<KeyValuePair<string, string>[]>(args[2]);

            string response = "{\"result\" : " + options + "}";
            DispatchCommandResult(new PluginResult(PluginResult.Status.OK, response));
        }

        public void request(string options)
        {
            string argsString = JSON.JsonHelper.Deserialize<string[]>(options)[0];
            string[] args = JSON.JsonHelper.Deserialize<string[]>(argsString);
            string url = args[0];
            string cookie = args[1];
            KeyValuePair<string, string>[] headers = JSON.JsonHelper.Deserialize<KeyValuePair<string, string>[]>(args[2]);

            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.SetCookies(uri, cookie); // 넘겨줄 쿠키가 있을때 CookiContainer 에 저장
            WebHeaderCollection webHeaderCollection = request.Headers;
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webHeaderCollection[pair.Key] = pair.Value;
            }

            request.BeginGetResponse(responseCallback, request);


        }

        void responseCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                try
                {
                    WebResponse response = request.EndGetResponse(result);

                    using (var reader = new StreamReader(response.GetResponseStream())) 
                    {
                        string jsonString = reader.ReadToEnd();
                        DispatchCommandResult(new PluginResult(PluginResult.Status.OK, jsonString));
                    }
                    
                }
                catch (WebException e)
                {
                    DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR));
                }
            }
        }
    }
}

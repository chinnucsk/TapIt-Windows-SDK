﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TapIt_WP8
{
    public class TapItHttpRequest
    {
        ///<summary>
        ///get the json data in string format
        ///</summary>
        public async Task<string> HttpRequest(String url)
        {
            string received = String.Empty;
            try
            {
                //Initializes a new WebRequest instance for the specified URI. 
                WebRequest httpWebRequest = HttpWebRequest.Create(url);

                //Begins and end an asynchronous request to an Internet resource
                var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null));

                //Gets the stream that is used to read the body of the response from the server.
                var responseStream = response.GetResponseStream();

                var sr = new StreamReader(responseStream);

                //Reads all characters from the current position to the end of the stream asynchronously
                //and returns them as one string.
                received = await sr.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in TapItHttpRequest() :" + ex.Message);
                throw ex;
            }
            return received;
        }
    }
}

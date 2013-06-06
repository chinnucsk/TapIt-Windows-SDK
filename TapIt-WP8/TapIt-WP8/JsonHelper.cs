﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TapIt_WP8
{
    [DataContract]
    public class JsonDataContract
    {
        [DataMember(Name = "type")]
        public string typeData;

        [DataMember(Name = "html")]
        public string Html;

        [DataMember(Name = "banner")]
        public string banner;

        [DataMember(Name = "clickurl")]
        public string clickUrl;

        [DataMember(Name = "adHeight")]
        public string adHeight;

        [DataMember(Name = "adWidth")]
        public string adWidth;
    }

    public class JsonParser
    {
        ///<summary>
        ///parson the json data
        ///</summary>
        public JsonDataContract ParseJson(string jsonResponse)
        {
            JsonDataContract jsonHlpr = null;
            try
            {
                //Serializes objects to the JavaScript 
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataContract));
                MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse));
                jsonHlpr = (JsonDataContract)jsonSerializer.ReadObject(ms);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in ParseJson() :" + ex.Message);
                throw ex;
            }

            return jsonHlpr;
        }

        ///<summary>
        ///convert string response to html format
        ///</summary>
        public String WrapToHTML(String data, String bridgeScriptPath, String scriptPath, JsonDataContract helper)
        {
            String alignment;
            if (Convert.ToInt32(helper.adWidth) > 0)
            {
                alignment = "style=\"width:" + helper.adWidth + "px; margin:0 auto; text-align:center;\"";
            }
            else
            {
                alignment = "align=\"left\"";
            }

            string strHTML = "<html><head>"
                + "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />"
                + "<title>Advertisement</title> "
                //            + "<script src=\"file:/" + bridgeScriptPath + "\" type=\"text/javascript\"></script>"
                //            + "<script src=\"file:/" + scriptPath + "\" type=\"text/javascript\"></script>"
                + "</head>"
                + "<body style=\"margin:0; padding:0; overflow:hidden; background-color:transparent;\">"
                + "<div " + alignment + ">"
                + data
                + "</div> "
                + "</body> "
                + "</html> ";

            return strHTML;
        }
    }
}

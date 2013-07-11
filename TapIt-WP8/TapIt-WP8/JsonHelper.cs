using System;
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

        [DataMember(Name = "adtitle")]
        public string adtitle;

        [DataMember(Name = "calltoaction")]
        public string calltoaction;

        [DataMember(Name = "declinestring")]
        public string declinestring;

    }

    public class JsonParser
    {
        ///<summary>
        ///parson the json data
        ///</summary>
        public JsonDataContract ParseJson(string jsonResponse)
        {
            JsonDataContract jsonHlpr = null;
            //Create a stream to serialize the object to.

            try
            {
                //Serializes objects to the JavaScript 
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(JsonDataContract));
                //MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
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
        public String WrapToHTML(String data, String bridgeScriptPath, String scriptPath,
            JsonDataContract helper, int width, int height)
        {
            int TableWidth = width;
            int TableHeight = height-1;
            string strHTML = "<html><head>"
                + "<meta name='viewport' content='"
                + "width="
                + TableWidth.ToString()
                + ", height="
                + TableHeight.ToString()
                + ", initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />"
                + "<title>Advertisement</title> "
                + "<script type=\"text/css\">a img {border: none;}</script>"
                + "</head>"
                + "<body style=\"margin:0; padding:0; overflow:hidden; background-color:transparent;\">"
                + "<table style=\"width: "
                + width.ToString()
                + "px; height: "
                + height.ToString()
                + "px; vertical-align:central; background-color: black;\">" // todo: color hardcoding - expose as property or use theme color
                + "<tr>"
                + "<td style=\"text-align:center;\">"
                + "<style type=\"text/css\">a img {border:none;}</style>"
                + data
                + "</td>"
                + "</tr>"
                + "</table>"
                + "</body> "
                + "</html> ";

            return strHTML;
        }
    }
}

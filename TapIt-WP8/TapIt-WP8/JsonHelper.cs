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
            /////////// dummy code starts 
            // do not remove

            //data = "<table border=1 width=400>";
            //data += "<tr height=100><td>this is first row</td></tr>";
            //data += "<tr height=50><td>this is second row</td></tr>";
            //data += "<tr height=100><td>this is third row</td></tr>";
            //data += "<tr height=50><td>this is forth row</td></tr>";
            //data += "<tr height=100><td>this is fifth row</td></tr>";

            //data += "<tr height=100><td>this is first row</td></tr>";
            //data += "<tr height=50><td>this is second row</td></tr>";
            //data += "<tr height=100><td>this is third row</td></tr>";
            //data += "<tr height=50><td>this is forth row</td></tr>";
            //data += "<tr height=100><td>this is fifth row</td></tr>";

            //data += "</table>";

            //data += "<img alt=\"Test_Image\" src=\"../Images/Test_Image.png\" />";
            /////////// dummy code ends

            String alignment;
            if (Convert.ToInt32(helper.adWidth) > 0)
            {
                alignment = "style=\"width:320"
                    // + helper.adWidth
                    // + "px; height:768" 
                    // + helper.adHeight
                    + "px; background-color: blue; margin:0 auto; text-align:center; display: table-cell; vertical-align: middle;\""; // todo: color hardcoded; either expose as property or use theme color
            }
            else
            {
                alignment = "align=\"left\"";
            }

            string strHTML = "<html><head>"
                + "<meta name='viewport' content='"
            + "width="
            + width.ToString()
            + ", height="
            + height.ToString()
            + ", initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />"
                + "<title>Advertisement</title> "
                //            + "<script src=\"file:/" + bridgeScriptPath + "\" type=\"text/javascript\"></script>"
                //            + "<script src=\"file:/" + scriptPath + "\" type=\"text/javascript\"></script>"
                + "</head>"
                + "<body style=\"margin:0; padding:0; overflow:hidden; background-color:transparent;\">"
                + "<table style=\"width: 100%; height: 100%; vertical-align:central; background-color: black;\">" // todo: color hardcoding - expose as property or use theme color
                + "<tr>"
                + "<td style=\"text-align:center;\">"
                //+ "<div style=\"display:table; width: 100%; height: 100%; background-color:green;\">" 
                //+ "<div " + alignment + ">"
                + data
                //+ "</div> "
                //+ "</div> "
                + "</td>"
                + "</tr>"
                + "</table>"
                + "</body> "
                + "</html> ";

            return strHTML;
        }
    }
}

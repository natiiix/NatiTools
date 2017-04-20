using System;
using System.Net;
using System.Text;
using System.IO;

namespace NatiTools.xWeb
{
    public static class HTTP
    {
        #region static string GetPostResponse()
        public static string GetPostResponse(string url, params string[] postdata)
        {
            string result = string.Empty;
            string data = string.Empty;

            ASCIIEncoding ascii = new ASCIIEncoding();

            if (postdata.Length % 2 != 0)
                return string.Empty;

            for (int i = 0; i < postdata.Length; i += 2)
                data += string.Format("&{0}={1}", postdata[i], postdata[i + 1]);

            data = data.Remove(0, 1);
            byte[] bytesarr = ascii.GetBytes(data);

            try
            {
                WebRequest request = WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytesarr.Length;

                Stream streamwriter = request.GetRequestStream();
                streamwriter.Write(bytesarr, 0, bytesarr.Length);
                streamwriter.Close();

                WebResponse response = request.GetResponse();
                streamwriter = response.GetResponseStream();

                StreamReader streamread = new StreamReader(streamwriter);
                result = streamread.ReadToEnd();
                streamread.Close();
            }
            catch (Exception) { }

            return result;
        }

        public static string GetPostResponse(string url, params PostData[] postdata)
        {
            string result = string.Empty;
            string data = string.Empty;

            ASCIIEncoding ascii = new ASCIIEncoding();

            for (int i = 0; i < postdata.Length; i++)
                data += string.Format("&{0}={1}", postdata[i].Name, postdata[i].Value);

            data = data.Remove(0, 1);
            byte[] bytesarr = ascii.GetBytes(data);

            try
            {
                WebRequest request = WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytesarr.Length;

                Stream streamwriter = request.GetRequestStream();
                streamwriter.Write(bytesarr, 0, bytesarr.Length);
                streamwriter.Close();

                WebResponse response = request.GetResponse();
                streamwriter = response.GetResponseStream();

                StreamReader streamread = new StreamReader(streamwriter);
                result = streamread.ReadToEnd();
                streamread.Close();
            }
            catch (Exception) { }

            return result;
        }
        #endregion
        #region class PostData
        public class PostData
        {
            public string Name;
            public string Value;

            public PostData(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
        #endregion
    }
}

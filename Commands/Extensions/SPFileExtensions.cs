using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace SharePointPnP.PowerShell.Commands.Extensions
{
    public static class SPFileExtensions
    {
        public static IList<RpcProperty> GetPropertyBag(this File file)
        {
            var ctx = (ClientContext)file.Context;
            var web = ctx.Web;
            var webRelativeFileUrl = file.ServerRelativeUrl.Replace(web.ServerRelativeUrl.TrimEnd('/') + '/', "");

            using (var wc = new WebClientEx())
            {
                if (file.Context.Credentials != null)
                {
                    wc.Credentials = file.Context.Credentials;
                }
                else
                {
                    wc.UseDefaultCredentials = true;
                }

                var requestUrl = web.Url.TrimEnd('/') + "/_vti_bin/_vti_aut/author.dll";

                wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                wc.Headers.Add("X-Vermeer-Content-Type", "application/x-www-form-urlencoded");

                var query = HttpUtility.ParseQueryString(string.Empty);
                query.Add("method", "getDocsMetaInfo");
                query.Add("url_list", $"[{webRelativeFileUrl}]");

                var rpcResult = Encoding.UTF8.GetString(
                    wc.UploadData(requestUrl, "POST", Encoding.UTF8.GetBytes(query.ToString()))
                    );

                return ParseRpcResult(rpcResult);
            }
        }

        public struct RpcProperty
        {
            private readonly string _key;
            private readonly object _value;
            private readonly bool _writable;

            public RpcProperty(string key, object value, bool writable)
            {
                this._key = key;
                this._value = value;
                this._writable = writable;
            }

            public string Key => _key;

            public object Value => _value;

            public bool Writable => _writable;

            public override string ToString()
            {
                return new { Key, Value, Writable }.ToString();
            }
        }

        private static IList<RpcProperty> ParseRpcResult(string rpcResult)
        {
            var result = new List<RpcProperty>();

            using (var sr = new System.IO.StringReader(rpcResult))
            {
                string currentLine;
                var hasReachedMetadata = false;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (!hasReachedMetadata)
                    {
                        if (currentLine == "<li>meta_info=")
                        {
                            // SKip the next ul line
                            sr.ReadLine();
                            hasReachedMetadata = true;
                        }
                        // else nothing to do
                    }
                    else
                    {
                        if (currentLine == "</ul>")
                        {
                            break; // end of data has been reached
                        }
                        else
                        {
                            var key = currentLine.Substring(4);
                            var rawValue = sr.ReadLine().Substring(4);
                            var typeInfo = rawValue.Substring(0, 1);
                            var writable = rawValue.Substring(1, 1) == "W";
                            var strValue = HttpUtility.HtmlDecode(rawValue.Substring(3));
                            object value = null;
                            switch (typeInfo)
                            {
                                case "B":
                                    value = Convert.ToBoolean(strValue);
                                    break;

                                case "I":
                                    value = Convert.ToInt32(strValue);
                                    break;

                                case "F":
                                case "T":
                                    value = DateTime.Parse(strValue);
                                    break;

                                case "S":
                                case "V":
                                    value = strValue;
                                    break;

                                default:
                                    throw new InvalidOperationException("Unknown RPC type");
                            }
                            var prop = new RpcProperty(key, value, writable);
                            result.Add(prop);
                        }
                    }
                }
                return result;
            }
        }

        // See https://stackoverflow.com/a/43172235/588868
        protected class WebClientEx : WebClient
        {
            protected override System.Net.WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;
                if (request != null)
                {
                    request.ServicePoint.Expect100Continue = false;
                }
                return request;
            }
        }
    }
}
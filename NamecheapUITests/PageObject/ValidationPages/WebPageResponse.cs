using System;
using System.Net;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class WebPageResponse
    {
        public string Url = string.Empty;
        public HttpWebResponse WebResponseStatus;
        public void VerifyPageWebResponseStatusCode()
        {
            try
            {
                Url = BrowserInit.Driver.Url;
                var webRequest = (HttpWebRequest)WebRequest.Create(Url);
                WebResponseStatus = (HttpWebResponse)webRequest.GetResponse();
                if (WebResponseStatus.StatusCode == HttpStatusCode.OK)
                {
                    WebResponseStatus.Close();
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var resp = (HttpWebResponse)e.Response;
                    switch (resp.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new WebException("WebException raised! - 404 Server - Error" + Environment.NewLine + "ErrorType:-" + e.Status + " in " + Environment.NewLine + "PageName:-" + Environment.NewLine + "Error Status Code:-" + 404 + Environment.NewLine + "Error Message:-" + "- Page Not found - This bird has flown.");
                        case HttpStatusCode.InternalServerError:
                            throw new WebException("WebException raised! - 500 Server - Error" + Environment.NewLine + "ErrorType:-" + e.Status + " in " + Environment.NewLine + "PageName:-" + Environment.NewLine + "Error Status Code:-" + 500 + Environment.NewLine + "Error Message:-" + "Server Error - Egg Is Broken :- Looks like our site is temporarily down. We'll be back soon.");
                        case HttpStatusCode.NonAuthoritativeInformation:
                            throw new WebException("WebException raised! - 203 Non-Authoritative Information" + 203 + Environment.NewLine + "Error Message:-" + "- Cached Copy Insted of orgin Server");
                        case HttpStatusCode.NoContent:
                            throw new WebException("WebException raised! - 204 No Content" + 204 + Environment.NewLine + "Error Message:-" + "- The response is Intentionally Blank");
                        case HttpStatusCode.TemporaryRedirect:
                            throw new WebException("WebException raised! - 307 Temporary Redirect" + 307 + Environment.NewLine + "Error Message:-" + "-The requested resource resides temporarily under a different URI");
                        case HttpStatusCode.BadRequest:
                            throw new WebException("WebException raised! - 400 Bad Request" + 400 + Environment.NewLine + "Error Message:-" + "-the server due to malformed syntax");
                        case HttpStatusCode.Unauthorized:
                            throw new WebException("WebException raised! - 401 Unauthorized" + 401 + Environment.NewLine + "Error Message:-" + "-authentication required by user");
                        case HttpStatusCode.MethodNotAllowed:
                            throw new WebException("WebException raised! -405 Method Not Allowed" + 405 + Environment.NewLine + "Error Message:-" + "-The method specified in the Request-Line is not allowed for the resource identified by the Request-URI");
                        case HttpStatusCode.HttpVersionNotSupported:
                            throw new WebException("WebException raised! -505 HTTP Version Not Supported" + 505 + Environment.NewLine + "Error Message:-" + "-The server does not support, or refuses to support");
                        default:
                            throw new WebException(e.Message + e.Source + e.Status);
                    }
                }
            }
        }
    }
}
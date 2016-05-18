using System;
using System.Net;

namespace VideoDownload
{
    public class PluralSight
    {
        private readonly string _url;
        private readonly string _localPath;

        public PluralSight(string url, string localPath)
        {
            _url = url;
            _localPath = localPath;
        }

        public void DownloadFile()
        {
            var webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri(_url), _localPath);
        }
    }
}
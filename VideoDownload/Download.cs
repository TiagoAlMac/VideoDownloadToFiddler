using Fiddler;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace VideoDownload
{
    public class Download : IAutoTamper, IFiddlerExtension
    {
        private readonly string _rootPath;


        public Download()
        {
            _rootPath = @"C:\PluralSight";
        }
        
        public void AutoTamperRequestBefore(Session oSession)
        {
            if (oSession.uriContains("pluralsight.com"))
            {
                if (oSession.uriContains(".mp4"))
                {
                    var temp = oSession.url.Split('/');

                    string chapter;
                    var title = _rootPath + "\\" + GetTitle(temp[5], out chapter);
                    chapter = title + "\\" + chapter;
                    createFolder(title);
                    createFolder(chapter);
                    var file = chapter + "\\" + temp[6] + ".mp4";
                    if (File.Exists(file)) return;
                    var pluralSight = new PluralSight("http://" + oSession.url, file);
                    var thread = new Thread(pluralSight.DownloadFile);
                    thread.Start();
                }
            }
        }

        private void createFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static string GetTitle(string candidatTitle, out string chapter)
        {
            var temp = candidatTitle.Split('-');
            var stop = false;
            int i;

            for (i = 0; i < temp.Length && !stop; i++)
            {
                if (Regex.IsMatch(temp[i], "m[0-9][0-9]*"))
                {
                    stop = true;
                }
            }

            var title = string.Join(" ", temp.Take(i - 1).ToArray()).TrimEnd();
            var numberTemp = temp[i - 1].Substring(1);
            int number;

            if (int.TryParse(numberTemp, out number))
                chapter = $"{number:00}" + " " + string.Join(" ", temp.Skip(i).ToArray()).TrimEnd();
            else
                chapter = string.Join(" ", temp.Skip(i - 1).ToArray()).TrimEnd().Substring(1);

            return title;
        }

        public void AutoTamperRequestAfter(Session oSession)
        {
            //MessageBox.Show("AutoTamperRequestAfter: "+ oSession.url);
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            //MessageBox.Show("AutoTamperResponseBefore: "+ oSession.url);
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            //MessageBox.Show("AutoTamperResponseAfter: "+ oSession.url);
        }

        public void OnBeforeReturningError(Session oSession)
        {
            //MessageBox.Show("OnBeforeReturningError: "+ oSession.url);
        }

        public void OnLoad()
        {
            //MessageBox.Show("OnLoad");
        }

        public void OnBeforeUnload()
        {
            //MessageBox.Show("OnBeforeUnload");
        }
    }
}

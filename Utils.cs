using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BandsinTown
{
    class Utils
    {
        public static Task<String> downloadFeed(Uri feed)
        {
            var tcs = new TaskCompletionSource<string>();
            var wc = new WebClient();
            wc.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    tcs.SetResult(e.Result);
                }
                else
                {
                    tcs.SetException(e.Error);
                }

            };

            wc.DownloadStringAsync(feed);
            return tcs.Task;
        }
    }
}

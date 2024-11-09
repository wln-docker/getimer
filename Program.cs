using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Wlniao;
var urls = Wlniao.Config.GetSetting("URLS").Trim().SplitBy();
if (int.TryParse(Wlniao.Config.GetSetting("WaitSeconds"), out int wait) && wait >= 0)
{
    if (wait > 0)
    {
        var http = new Http { ServerName = "Getimer", ListenPort = 80 };
        http.Handler = new System.Action<Http.Context>((ctx) =>
        {
            if (ctx.Path.IndexOf('.') < 0)
            {
                var finish = 0;
                var endTime = DateTime.Now.AddSeconds(wait);
                foreach (var url in urls)
                {
                    new System.Threading.Thread(() =>
                    {
                        var time = DateTools.Format();
                        var result = Wlniao.NetTools.Request(url, "");
                        Console.WriteLine($"{time} {url} => {result}");
                        finish++;
                    }).Start();
                }
                while (finish < urls.Length && DateTime.Now < endTime)
                {
                    System.Threading.Thread.Sleep(100);
                }
                ctx.Response = $"Finish: {finish}";
            }
        });
        http.Start();
    }
    else
    {
        foreach (var url in urls)
        {
            var time = DateTools.Format();
            var result = Wlniao.NetTools.Request(url, "");
            Console.WriteLine($"{time} {url} => {result}");
        }
    }
}
else
{
    Console.WriteLine("The ENV 'WaitSeconds' is invalid");
}
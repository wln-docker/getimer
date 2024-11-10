using System;
using Wlniao;
var urls = Wlniao.Config.GetSetting("URLS", "").Trim().SplitBy();
if (int.TryParse(Wlniao.Config.GetSetting("WaitSeconds", "0"), out int wait) && wait >= 0)
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
                        var result = Wlniao.NetTools.Request(url, "");
                        Wlniao.Log.Loger.Topic("history", $"{url} => {result}");
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
            var result = Wlniao.NetTools.Request(url, "");
            Wlniao.Log.Loger.Topic("history", $"{url} => {result}");
        }
    }
}
else
{
    Console.WriteLine("The ENV 'WaitSeconds' is invalid");
}
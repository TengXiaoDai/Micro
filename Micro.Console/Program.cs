using System.Net;
using System.Text;

Parallel.For(20, 100, async a => {
    using WebClient web = new WebClient();
    web.Encoding = Encoding.UTF8;
    byte[] txt = web.DownloadData("https://localhost:7138/api/Home/InStack");
    string rr = Encoding.GetEncoding("utf-8").GetString(txt);
    Console.WriteLine(rr);
});

Console.ReadLine();
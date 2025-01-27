namespace CVB.NET.TorNetwork
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;
    using com.LandonKey.SocksWebProxy;
    using com.LandonKey.SocksWebProxy.Proxy;
    using HtmlAgilityPack;
    using HtmlDocument = HtmlAgilityPack.HtmlDocument;

    public class TorConnector : IDisposable
    {
        public int TorSocksProxyProcessId { get; set; }

        public TorConnector()
        {
            StartTor();
        }

        public void Dispose()
        {
            StopTor(true);
        }

        public WebClient GetTorClient()
        {
            WebClient torClient = new WebClient();

            try
            {
                torClient.Proxy = new SocksWebProxy(
                    new ProxyConfig(
                        //This is an internal http->socks proxy that runs in process
                        IPAddress.Parse("127.0.0.1"),
                        //This is the port your in process http->socks proxy will run on
                        12345,
                        //This could be an address to a local socks proxy (ex: Tor / Tor Browser, If Tor is running it will be on 127.0.0.1)
                        IPAddress.Parse("127.0.0.1"),
                        //This is the port that the socks proxy lives on (ex: Tor / Tor Browser, Tor is 9150)
                        9251,
                        //This Can be Socks4 or Socks5
                        ProxyConfig.SocksVersion.Five));
            }
            catch
            {
                StopTor(true);
                throw;
            }

            return torClient;
        }

        private void StartTor()
        {
            ProcessStartInfo startTorInfo = new ProcessStartInfo(Application.StartupPath + "\\Dependencies\\Tor\\tor.exe", "-f .\\torrc-defaults");

            startTorInfo.WorkingDirectory = Application.StartupPath + "\\Dependencies\\Tor\\";

            TorSocksProxyProcessId = Process.Start(startTorInfo).Id;

            bool connectedToTor = false;

            WebClient testClient = GetTorClient();

            for (int retryAttemps = 0; retryAttemps < 4; retryAttemps++)
            {
                Thread.Sleep(5000);

                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(testClient.DownloadString("https://check.torproject.org/"));

                HtmlNode titleNode = doc.DocumentNode.SelectNodes("//head//title").SingleOrDefault();

                if (titleNode == null
                    || !titleNode.InnerText.Contains("Congratulations. This browser is configured to use Tor."))
                {
                    continue;
                }

                connectedToTor = true;

                break;
            }

            if (!connectedToTor)
            {
                throw new NotConnectedToTorException();
            }
        }

        private void StopTor(bool suppressThrow)
        {
            if (!suppressThrow)
            {
                Process.GetProcessById(TorSocksProxyProcessId).Kill();
                return;
            }

            try
            {
                Process.GetProcessById(TorSocksProxyProcessId).Kill();
            }
            catch
            {
            }
        }
    }
}
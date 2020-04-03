using System;
using System.IO;
using HitBTC_Triangulation.Model;

namespace HitBTC_Triangulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "HitBTC Triangulation";
            Console.WindowWidth = 200;

            Console.Write("START COIN: ");
            string coin = Console.ReadLine();

            Console.Write("RANGE: ");
            int range = int.Parse(Console.ReadLine());

            coin = coin.ToUpper().Trim();
            HitBTCClient hitbtc = new HitBTCClient(coin);
            Console.Write("CURRENT BALANCE: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(hitbtc.CurrentAmount + " " + coin);
            Console.ResetColor();
            Console.Write("CALCULATING PATHS... ");

            hitbtc.FillGraph();
            hitbtc.FindAllPathByCoin(range);

            Console.WriteLine("OK");

            Console.WriteLine("CONNECTING TO ORDERBOOK... ");
            hitbtc.InitWebSocket();

            while (!hitbtc.CanStart) { }
            Console.WriteLine("OK");
            while (true)
            {
                try
                {

                    Model.Path path = hitbtc.GetBestPath();

                    if (path.Percentage >= 100)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.WriteLine(path.ToString());
                    Console.ResetColor();

                    //if (path.Percentage > 100 && path.LstStep.Count > 3 && hitbtc.Tradable(path) && path.Profit > 0.00001m) //  && path.MaxStartVolume >= hitbtc.MinStartAmount
                    //{
                    //    hitbtc.WalkPath(path);
                    //    hitbtc.Reset();
                    //}

                }
                catch (Exception e)
                {
                    using (StreamWriter writetext = File.AppendText("C:\\log\\log.txt"))
                    {
                        writetext.Write("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + e.ToString() + "\r\n");
                        writetext.Close();
                    }

                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}

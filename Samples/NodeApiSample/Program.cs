using System;

namespace NodeApiSample
{
    using Eos;
    using Eos.Api;

    class Program
    {
        static void Main(string[] args)
        {
            NodeApiClient node = new NodeApiClient("https://jungle2.cryptolions.io");
            string accountName = "dmitrychegod";

            DoTasks(node, accountName);

            Console.ReadLine();
        }

        static async void DoTasks(NodeApiClient node, string accountName)
        {
            var info = await node.GetInfo();
            Console.WriteLine("Blockchain Info:");
            Console.WriteLine(info.ToJson());

            var accountInfo = await node.GetAccount(accountName);
            Console.WriteLine("\nAccount Info:");
            Console.WriteLine(accountInfo.ToJson());

            var balance = await node.GetBalance(accountName);
            Console.WriteLine("\nAccount Balance:");
            Console.WriteLine(balance.ToJson());
        }
    }
}

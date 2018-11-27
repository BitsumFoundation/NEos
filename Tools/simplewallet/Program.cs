using Eos;
using Eos.Api;
using Eos.Cryptography;
using Eos.Models;
using System;
using System.Collections.Generic;

namespace simplewallet
{
    class Program
    {
        static string help = $"help: Shows this help\n" +
            $"exit: Terminates the application\n" +
            $"info: Shows info about your account\n" +
            $"balance: Shows your balance\n" +
            $"transfer [account] [amount]: Transfers coins";

        static KeyPair keyPair;
        static NodeApiClient node;
        static string account;

        static void Main(string[] args)
        {
            Console.Clear();

            try
            {
                if (args.Length != 2)
                {
                    Console.WriteLine($"Invalid parameters. Please start with followed params:\n" +
                        $" simplewallet [node uri] [private key]\n" +
                        $" simplewallet https://jungle2.cryptolions.io 5KUht5PmugQNP716VVVusEghr1TQHwkzSvtbBwQtXqv8K9Lz1nP");

                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Welcome to EOS Simple Wallet\n");

                string uri = args[0];
                string key = args[1];

                keyPair = new KeyPair(key);
                node = new NodeApiClient(uri);
                account = node.GetAccounts(keyPair.PublicKey.ToString()).Result[0];
                
                Console.WriteLine($"Using node: {uri}");
                Console.WriteLine($"Account: {account}");
                Console.WriteLine($"Public key: {keyPair.PublicKey}");
                Console.WriteLine();
                Console.WriteLine("Available commands:");
                Console.WriteLine(help + "\n");

                StartShell();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        static void StartShell()
        {
            while (true)
            {
                Console.Write(">");
                string cmd = Console.ReadLine();

                try
                {
                    switch (cmd)
                    {
                        case "help": Console.WriteLine(help); break;
                        case "exit": Environment.Exit(0); break;
                        case "balance":
                            var b = node.GetBalance(account).Result;
                            foreach (var item in b)
                            {
                                Console.WriteLine(item);
                            }                        
                            break;
                        case "info":
                            var info = node.GetAccount(account).Result;
                            Console.WriteLine(info.ToJson());
                            break;
                        default: break;
                    }

                    if (cmd.Contains("transfer"))
                    {
                        cmd = cmd.Replace("transfer ", "");
                        string recipient = cmd.Substring(0, 12);
                        cmd = cmd.Replace(recipient + " ", "");
                        Currency amount = new Currency(cmd);

                        TransferAction action = new TransferAction()
                        {
                            Account = "eosio.token",
                            Data = new TransferData()
                            {
                                From = account,
                                To = recipient,
                                Quantity = amount
                            },
                            Authorization = new List<Authorization>()
                            {
                                new Authorization()
                                {
                                    Actor = account,
                                    Permission = "active"
                                }
                            }                            
                        };

                        var actions = new List<IAction>()
                        {
                            action
                        };

                        var tx = node.CreateTransaction(actions).Result;
                        var stx = node.SignTransaction(tx, keyPair.PrivateKey).Result;
                        var hash = node.PushTransaction(stx).Result;

                        Console.WriteLine($"TX Hash: {hash}");
                    }
                }
                catch (Exception ex)
                {
                    ProcessException(ex);
                }
            }

        }

        static void ProcessException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException is ApiException)
                {
                    var apiex = ex.InnerException as ApiException;

                    Console.WriteLine($"Exception ({apiex.Code}): {apiex.Message}");
                    Console.WriteLine($"{apiex.Error.ToJson()}");
                }
                else
                {
                    Console.WriteLine($"Exception ({ex.InnerException.GetType()}): {ex.InnerException.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Exception ({ex.GetType()}): {ex.Message}");
            }
        }
    }
}

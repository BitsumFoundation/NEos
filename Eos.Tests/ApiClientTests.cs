using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Eos.Tests
{
    using Models;
    using Api;
    using static Consts;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Eos.Cryptography;

    [TestClass]
    public class ApiClientTests
    {
        private NodeApiClient client;

        public object Json { get; private set; }

        [TestInitialize]
        public void Intitialize()
        {
            client = new NodeApiClient("https://api.jungle.alohaeos.com");
        }

        [TestMethod]
        public void GetInfoTest()
        {
            var result = client.GetInfo().Result;
            Trace.WriteLine(result.ToJson());
        }

        [TestMethod]
        public void GetAccountTest()
        {
            var result = client.GetAccount(AccountName).Result;
            Trace.WriteLine(result.ToJson());
        }

        [TestMethod]
        public void GetBalanceTest()
        {
            var result = client.GetBalance(AccountName, "eosio.token").Result;
            //var result = client.GetBalances("dmitrychegod", "dmitrychegod").Result;
            Trace.WriteLine(result.ToJson());
        }

        [TestMethod]
        public void GetCurrencyStatsTest()
        {
            var result = client.GetCurrencyStats("eosio.token", "EOS").Result;
            //var result = client.GetCurrencyStats("dmitrychegod", "BSM").Result;
            Trace.WriteLine(result.ToJson());
        }

        [TestMethod]
        public void GetAccountsTest()
        {
            var result = client.GetAccounts(JunglePublicKey).Result;
            Trace.WriteLine(result.ToJson());
        }

        [TestMethod]
        public void GetBlockTest()
        {
            var block = client.GetBlock(97108).Result;

            Trace.WriteLine(block.ToJson());
        }

        [TestMethod]
        public void AbiJsonToBinTest()
        {
            TransferAction action = new TransferAction()
            {
                Account = "eosio.token",
                Data = new TransferData()
                {
                    From = "dmitrychegod",
                    To = "bitsumbridge",
                    Quantity = new Currency("1.0000 EOS"),
                    Memo = "1"
                }
            };

            client.AbiJsonToBin(action).Wait();
            
            Trace.WriteLine(action.ToJson());
        }

        [TestMethod]
        public void PushTransactionTest()
        {
            TransferAction a = new TransferAction()
            {
                Account = "eosio.token",
                Data = new TransferData()
                {
                    From = "dmitrychegod",
                    To = "bitsumbridge",
                    Quantity = new Currency("1.0000 EOS"),
                    Memo = "test"
                }
            };

            a.Authorization = new List<Authorization>()
            {
                new Authorization()
                {
                    Actor = a.Data.From,
                    Permission = "active"
                }
            };

            var actions = new List<IAction>
            {
                a
            };

            PrivateKey pvt = new PrivateKey(JunglePrivateKey);

            var tx = client.CreateTransaction(actions).Result;
            Trace.WriteLine(tx.ToJson());

            var stx = client.SignTransaction(tx, pvt).Result;
            Trace.WriteLine(stx.ToJson());

            var txid = client.PushTransaction(stx).Result;
            Trace.WriteLine($"\nTX Hash: {txid}");
        }
    }
}

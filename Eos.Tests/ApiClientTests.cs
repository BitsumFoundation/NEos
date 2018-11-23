using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Eos.Tests
{
    using Api;
    using static Consts;

    [TestClass]
    public class ApiClientTests
    {
        private ApiClient client;

        [TestInitialize]
        public void Intitialize()
        {
            client = new ApiClient("https://api.jungle.alohaeos.com");
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
            var result = client.GetBalances(AccountName, "eosio.token").Result;
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
    }
}

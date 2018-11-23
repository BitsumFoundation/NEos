using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Api
{
    using Chain;
    using History;
    using System.Collections.Generic;

    public class ApiClient
    {
        public readonly int ApiVersion = 1;

        public const string ChainGroup = "chain";

        public const string HistoryGroup = "history";

        private HttpClient httpClient;

        public ApiClient() : this("http://localhost:8888") { }

        public ApiClient(string uri) : this(new Uri(uri)) { }

        public ApiClient(Uri uri)
        {
            httpClient = new HttpClient() { BaseAddress = uri };
        }

        public async Task<Info> GetInfo()
        {
            return await GetAsync<Info>($"/v{ApiVersion}/{ChainGroup}/get_info");
        }

        public async Task<Account> GetAccount(string name)
        {
            AccountRequest request = new AccountRequest()
            {
                AccountName = name
            };

            return await PostAsync<Account>(request, $"/v{ApiVersion}/{ChainGroup}/get_account");
        }

        public async Task<List<Currency>> GetBalances(string account)
        {
            return await GetBalances("eosio.token", account, null);
        }

        public async Task<List<Currency>> GetBalances(string account, string code)
        {
            return await GetBalances(account, code, null);
        }

        public async Task<List<Currency>> GetBalances(string account, string code, string symbol)
        {
            BalanceRequest request = new BalanceRequest()
            {
                Code = code,
                Account = account,
                Symbol = symbol
            };

            return await PostAsync<List<Currency>>(request, $"/v{ApiVersion}/{ChainGroup}/get_currency_balance");
        }

        public async Task<CurrencyStats> GetCurrencyStats(string code, string symbol)
        {
            CurrencyStatsRequest request = new CurrencyStatsRequest()
            {
                Code = code,
                Symbol = symbol
            };

            Dictionary<string, CurrencyStats> pairs = await PostAsync<Dictionary<string, CurrencyStats>>(request, $"/v{ApiVersion}/{ChainGroup}/get_currency_stats");
            if (!pairs.ContainsKey(symbol))
            {
                return null;
            }

            pairs[symbol].Ticker = symbol;
            return pairs[symbol];
        }

        public async Task<List<string>> GetAccounts(string publicKey)
        {
            KeyAccountRequest request = new KeyAccountRequest()
            {
                PublicKey = publicKey
            };

            return (await PostAsync<KeyAccountResponse>(request, $"/v{ApiVersion}/{HistoryGroup}/get_key_accounts")).AccountNames;
        }



        protected async Task<T> GetAsync<T>(string uri)
        {
            string response;
            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                response = await (await httpClient.SendAsync(req)).Content.ReadAsStringAsync();
            }

            var error = JsonConvert.DeserializeObject<ApiError>(response);
            if (error.Error != null)
            {
                throw new ApiException(error);
            }

            T res = JsonConvert.DeserializeObject<T>(response);

            return res;
        }

        protected async Task<T> PostAsync<T>(object request, string uri)
        {
#if DEBUG
            string requestCmd = JsonConvert.SerializeObject(request, Formatting.Indented);
#else
            string requestCmd = JsonConvert.SerializeObject(request);
#endif
            string response;

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentTypes.ApplicationJson));

            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, uri))
            {
                req.Content = new StringContent(requestCmd, Encoding.UTF8, ContentTypes.ApplicationJson);
                response = await (await httpClient.SendAsync(req)).Content.ReadAsStringAsync();
            }

            //var error = JsonConvert.DeserializeObject<ApiError>(response);
            //if (error.Code != 0)
            //{
            //    throw new ApiException(error);
            //}

            T res = JsonConvert.DeserializeObject<T>(response);
            if (res != null) return res;

            return default(T);
        }
    }
}

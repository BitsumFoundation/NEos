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
    using Models;
    using Eos.Cryptography;
    using System.IO;

    public class NodeApiClient
    {
        public readonly int ApiVersion = 1;

        public const string ChainGroup = "chain";

        public const string HistoryGroup = "history";

        private HttpClient httpClient;

        public NodeApiClient() : this("http://localhost:8888") { }

        public NodeApiClient(string uri) : this(new Uri(uri)) { }

        public NodeApiClient(Uri uri)
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

        public async Task<List<Currency>> GetBalance(string account)
        {
            return await GetBalance(account, "eosio.token", null);
        }

        public async Task<List<Currency>> GetBalance(string account, string code)
        {
            return await GetBalance(account, code, null);
        }

        public async Task<List<Currency>> GetBalance(string account, string code, string symbol)
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

        public async Task<Block> GetBlock(ulong blockNumber)
        {
            return await GetBlock(blockNumber.ToString());
        }

        public async Task<Block> GetBlock(string blockId)
        {
            Dictionary<string, string> request = new Dictionary<string, string>()
            {
                { "block_num_or_id", blockId }
            };

            return (await PostAsync<Block>(request, $"/v{ApiVersion}/{ChainGroup}/get_block"));
        }

        public async Task AbiJsonToBin(IAction action)
        {
            AbiJsonToBinRequest request = new AbiJsonToBinRequest()
            {
                Code = action.Account,
                Action = action.Name,
                Args = action.RawData
            };

            var response = await PostAsync<Dictionary<string, string>>(request, $"/v{ApiVersion}/{ChainGroup}/abi_json_to_bin");

            action.HexData = response["binargs"];
        }
        
        public async Task<Transaction> CreateTransaction(List<IAction> actions)
        {
            var chainInfo = await GetInfo();
            var block = await GetBlock(chainInfo.LastIrreversibleBlockId);

            foreach (var item in actions)
            {
                await AbiJsonToBin(item);
            }

            var transaction = new Transaction
            {
                RefBlockNum = chainInfo.LastIrreversibleBlockNum,
                RefBlockPrefix = block.RefBlockPrefix,
                Expiration = chainInfo.HeadBlockTime.AddSeconds(60),
                Actions = actions,
                ChainId = chainInfo.ChainId
            };

            return transaction;
        }

        public async Task<SignedTransaction> SignTransaction(Transaction transaction, PrivateKey privateKey)
        {
            return await Task.Run(() =>
            {
                SignedTransaction stx = new SignedTransaction()
                {
                    Transaction = transaction
                };

                byte[] packed_trx;
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    transaction.WriteBytes(writer);
                    packed_trx = ms.ToArray();
                }

                stx.PackedTransaction = packed_trx.ToHex();

                byte[] data = transaction.ChainId.FromHex().Concat(packed_trx, new byte[32]);

                string signature = privateKey.SignData(data);

                stx.Signatures = new List<string>()
                {
                    signature
                };

                // TODO: compression type
                stx.Compression = "none";

                return stx;
            });
        }

        public async Task<string> PushTransaction(SignedTransaction transaction)
        {
            PushTransactionRequest request = new PushTransactionRequest()
            {
                PackedTransaction = transaction.PackedTransaction,
                Signatures = transaction.Signatures
            };

            var response = await PostAsync<PushTransactionResponse>(request, $"/v{ApiVersion}/{ChainGroup}/push_transaction");

            string result = response.TransactionId;

            return result;
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

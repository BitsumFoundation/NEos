using Newtonsoft.Json;

namespace Eos.Models
{
    [JsonConverter(typeof(StringCurrencyConverter))]
    public class Currency
    {
        public Currency() { }

        public Currency(string value)
        {
            var parts = value.Split(' ');
            Amount = decimal.Parse(parts[0]);
            Ticker = parts[1];
        }

        public decimal Amount { get; set; }

        public string Ticker { get; set; }

        public override string ToString()
        {
            return $"{Amount} {Ticker}";
        }
    }
}

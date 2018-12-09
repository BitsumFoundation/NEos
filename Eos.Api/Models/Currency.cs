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
            if (parts.Length == 2)
            {
                Ticker = parts[1];
            }
            else
            {
                Ticker = "EOS";
            }
        }

        public decimal Amount { get; set; }

        public string Ticker { get; set; }

        public static implicit operator Currency(double value)
        {
            return new Currency(value.ToString());
        }

        public static implicit operator Currency(int value)
        {
            return new Currency(value.ToString());
        }

        public static implicit operator Currency(string value)
        {
            return new Currency(value);
        }

        public override string ToString()
        {
            return $"{Amount:0.0000} {Ticker}";
        }
    }
}

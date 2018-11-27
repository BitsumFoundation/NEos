using Newtonsoft.Json;
using System;

namespace Eos.Api
{
    public class ApiException : Exception
    {
        [JsonProperty("message")]
        private string message;

        public ApiException(ApiError error) : base()
        {
            Error = error;
        }

        [JsonProperty("code")]
        public int Code { get; private set; }

        public override string Message => message;

        [JsonProperty("error")]
        public ApiError Error { get; private set; }
    }
}

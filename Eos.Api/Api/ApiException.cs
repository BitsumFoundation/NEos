using System;

namespace Eos.Api
{
    public class ApiException : Exception
    {
        public ApiException(ApiError error) : base($"[{error.Code}]: {error.Message}") { }
    }
}

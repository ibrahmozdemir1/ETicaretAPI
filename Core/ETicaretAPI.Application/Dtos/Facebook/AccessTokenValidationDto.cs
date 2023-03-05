using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Dtos.Facebook
{
    public class AccessTokenValidationDto
    {
        [JsonPropertyName("data")]
        public AccessTokenValidationData Data { get; set; }
    }

    public class AccessTokenValidationData
    {
        [JsonPropertyName("is_Valid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}

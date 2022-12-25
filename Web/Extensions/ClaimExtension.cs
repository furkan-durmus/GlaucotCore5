using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Web.Extensions
{
    public static class ClaimExtension
    {
        public static Claim GetClaim(this IEnumerable<Claim> list, string type)
        {
            return list.FirstOrDefault(v => v.Type == type);
        }
    }
}

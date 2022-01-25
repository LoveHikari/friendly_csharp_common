using System.Collections.Generic;

namespace Hikari.Common.CreditCard.BINValidators
{
    /// <summary>
    /// 已知卡类型
    /// </summary>
    public class KnownCardTypes
    {
        public static Dictionary<string, string> Default => new Dictionary<string, string>
        {
            { "Amex Card", "^3[47][0-9]{13}$" },
            { "Discover", "^65[4-9][0-9]{13}|64[4-9][0-9]{13}|6011[0-9]{12}|(622(?:12[6-9]|1[3-9][0-9]|[2-8][0-9][0-9]|9[01][0-9]|92[0-5])[0-9]{10})$" },
            { "JCB", @"^(?:2131|1800|35\d{3})\d{11}$" },
            { "China Union Pay", "^((62|81)[0-9]{14,17})$" },
            { "MasterCard", "^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$" },
            { "Visa", "^4[0-9]{12}(?:[0-9]{3})?$" }
        };
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hikari.Common.CreditCard.BINValidators;

/******************************************************************************************************************
 * 
 * 
 * 标  题：信用卡验证器(版本：Version1.0.0)
 * 作　者：YuXiaoWei
 * 日　期：2020/02/13
 * 修　改：
 * 参　考：https://github.com/EdiWang/Edi.CreditCardUtils
 * 说　明：暂无...
 * 备　注：暂无...
 * 
 * 
 * ***************************************************************************************************************/
namespace Hikari.Common.CreditCard
{
    /// <summary>
    /// 信用卡验证器
    /// </summary>
    public class CreditCardValidator
    {
        /// <summary>
        /// 根据以下条件验证信用卡号:
        /// 1. 数字格式（14-16位）
        /// 2. Luhn检验
        /// 3. 特定的BIN格式（可选）
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <param name="formatValidators">BIN格式验证器</param>
        /// <returns></returns>
        public CreditCardValidationResult ValidCardNumber(string cardNumber, ICardTypeValidator[] formatValidators = null)
        {
            static CreditCardValidationResult CreateResult(CardNumberFormat format, string[] cardTypes = null)
            {
                return new CreditCardValidationResult
                {
                    CardNumberFormat = format,
                    CardTypes = cardTypes
                };
            }

            // 检查卡号长度
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 14 || cardNumber.Length > 19)
            {
                return CreateResult(CardNumberFormat.Invalid_BadStringFormat);
            }

            // 检查字符串是全数字
            var isMatch = Regex.IsMatch(cardNumber, @"^\d*$");
            if (!isMatch)
            {
                return CreateResult(CardNumberFormat.Invalid_BadStringFormat);
            }

            // 尝试Luhn检验
            var digits = GetDigitsArrayFromCardNumber(cardNumber);
            if (!IsLuhnValid(digits))
            {
                return CreateResult(CardNumberFormat.Invalid_LuhnFailure);
            }

            // 针对已知类型进行检验
            var matchedCardTypes = new List<string>();
            foreach (var (key, value) in KnownCardTypes.Default)
            {
                if (Regex.IsMatch(cardNumber, value))
                {
                    matchedCardTypes.Add(key);
                }
            }

            // 针对类型验证器进行检验
            if (null != formatValidators)
            {
                var more = from validator in formatValidators
                           let brandMatch = Regex.IsMatch(cardNumber, validator.RegEx)
                           where brandMatch
                           select validator.Name;

                matchedCardTypes.AddRange(more);
            }

            return matchedCardTypes.Any() ? 
                CreateResult(CardNumberFormat.Valid_BINTest, matchedCardTypes.ToArray()) : 
                CreateResult(CardNumberFormat.Valid_LuhnOnly);
        }

        /// <summary>
        /// Check credit card numbers agaist Luhn Algorithm
        /// https://en.wikipedia.org/wiki/Luhn_algorithm
        /// </summary>
        /// <param name="digits">Credit card numbers</param>
        /// <returns>Is valid Luhn</returns>
        private bool IsLuhnValid(int[] digits)
        {
            var sum = 0;
            var alt = false;
            for (var i = digits.Length - 1; i >= 0; i--)
            {
                if (alt)
                {
                    digits[i] *= 2;
                    if (digits[i] > 9)
                    {
                        digits[i] -= 9;
                    }
                }
                sum += digits[i];
                alt = !alt;
            }

            return sum % 10 == 0;
        }

        private int[] GetDigitsArrayFromCardNumber(string cardNumber)
        {
            var digits = cardNumber.Select(p => int.Parse(p.ToString())).ToArray();
            return digits;
        }
    }
}

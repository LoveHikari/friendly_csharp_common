namespace Hikari.Common.CreditCard.BINValidators
{
    public interface ICardTypeValidator
    {
        string Name { get; }
        string RegEx { get; }
    }
}

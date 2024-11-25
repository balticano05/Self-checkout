namespace SelfCheckoutServiceMachine.Models;

public class DebitCard
{
    public int Id { get; private set; }
    public decimal Balance { get; private set; }
    public string Number { get; private set; }
    public string Password { get; private set; }
    public bool IsBlock { get; private set; }

    private DebitCard() { }

    public class Builder
    {
        private readonly DebitCard _debitCard = new DebitCard();

        public Builder SetId(int id)
        {
            _debitCard.Id = id;
            return this;
        }

        public Builder SetBalance(decimal balance)
        {
            _debitCard.Balance = balance;
            return this;
        }

        public Builder SetNumber(string number)
        {
            _debitCard.Number = number;
            return this;
        }

        public Builder SetPassword(string password)
        {
            _debitCard.Password = password;
            return this;
        }

        public Builder SetIsBlock(bool isBlock)
        {
            _debitCard.IsBlock = isBlock;
            return this;
        }

        public DebitCard Build()
        {
            return _debitCard;
        }
    }
}
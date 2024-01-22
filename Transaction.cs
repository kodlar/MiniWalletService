namespace ConsoleAppWallet
{
    public class Transaction
    {
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public CurrencyEnum Currency {  get; private set; }
        public Transaction(decimal amount, string description, DateTime transactionDate, CurrencyEnum currency)
        {
            Amount = amount;
            Description = description;
            TransactionDate=transactionDate;
            Currency = currency;
        }
    }
}

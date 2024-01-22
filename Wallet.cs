using System.Transactions;

namespace ConsoleAppWallet
{

    public class Wallet
    {
        private decimal balance;

        private readonly List<Transaction> transactions;

        private readonly StreamWriter logFile;

        private const string LogDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
        public Wallet(CurrencyEnum currency, string owner)
        {
            ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
            
            string address = Guid.NewGuid().ToString();
            balance = decimal.Round(Decimal.Zero, 2);
            transactions = new List<Transaction>();
            logFile = new StreamWriter($"{address}-wallet.log");
            Address = address;
            Currency = currency;
            Owner = owner;
            Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde {currency} cüzdanı oluşturuldu. Bakiye: {balance} {currency}");            
        }

        public string Owner { get;  set; }
        public CurrencyEnum Currency { get; set; }
        public string Address { get; set; }      
        public decimal Balance
        {
            get { return decimal.Round(balance, 2); }
            set { balance = value; }
        }
        public List<Transaction> Transactions
        {
            get { return transactions; }
        }

      
        public Task Log(string message)
        {
            logFile.WriteLine(message);

            logFile.Flush();

            return Task.CompletedTask;
        }
    }



}

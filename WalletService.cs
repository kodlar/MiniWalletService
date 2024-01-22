using Microsoft.Extensions.Logging;

namespace ConsoleAppWallet
{
    public class WalletService : IWalletService
    {
        private const int InitialCount = 1;
        private const int MaxCount = 1;
        private readonly ILogger<WalletService> _logger;
        private readonly Wallet _wallet;
        private const string LogDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
        public WalletService(ILogger<WalletService> logger, Wallet wallet)
        {
            _logger = logger;
            _wallet = wallet;
        }

        private readonly SemaphoreSlim _semaphore = new(InitialCount, MaxCount);

        //TODO:all transactions should be rolled back if you encounter a problem during the deposit operation
        //TODO:users can deposit/withdraw the requested amount to the wallet that you have created
        public async Task Deposit(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {

                _wallet.Balance += amount;
                _wallet.Transactions.Add(new Transaction(amount, "Para Yatırma", DateTime.Now, currency));
                await _wallet.Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde Para Yatırma: {amount} {currency}");

            }
            catch (Exception exception)
            {
                // Hata durumunda rollback yap
                _wallet.Balance -= amount;
                _wallet.Transactions.Remove(_wallet.Transactions.Last());
                await _wallet.Log(exception.Message);
                _logger?.LogError(exception, "Deposit esnasında hata oluştu");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        //TODO:all transactions should be rolled back if you encounter a problem during the deposit operation
        public async Task DepositWithError(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {

                _wallet.Balance += amount;
                _wallet.Transactions.Add(new Transaction(amount, "Para Yatırma", DateTime.Now, currency));

                throw new Exception($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde {amount} yatırılırken bir hata oluştu");

            }
            catch (Exception exception)
            {
                // Hata durumunda rollback yap
                _wallet.Balance -= amount;
                _wallet.Transactions.Remove(_wallet.Transactions.Last());
                _wallet.Transactions.Add(new Transaction(amount, "Para Yatırma esnasında hata oluştu işlem geri alındı", DateTime.Now, currency));
                await _wallet.Log(exception.Message);
                await _wallet.Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde Para Yatırma: {amount} {currency} işlemi gerçekleştirilmedi bakiye eski haline getirildi");
                _logger?.LogError(exception, "Deposit esnasında hata oluştu");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        //TODO:all transactions should be rolled back if you encounter a problem during thewithdraw operation
        public async Task<Result> Withdraw(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            Result result = new();

            try
            {

                if (_wallet.Balance < amount)
                {
                    _wallet.Transactions.Add(new Transaction(amount, "Para çekme işlemi esnasında yetersiz bakiye", DateTime.Now, currency));

                    await _wallet.Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde {amount} {currency} para çekimi talebinde bulunuldu. Hesapta Yetersiz bakiye işlem iptal edildi");

                    result.Description = "Yetersiz bakiye";

                    return result;
                }

                _wallet.Balance -= amount;

                _wallet.Transactions.Add(new Transaction(amount, "Para Çekimi", DateTime.Now, currency));

                await _wallet.Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde Para Çekimi: {amount} {currency}");

                result.Status = true;

                result.Description = "Para çekimi başarıyla gerçekleşti.";

            }
            catch (Exception exception)
            {
                // Hata durumunda rollback yap
                _wallet.Balance += amount;

                _wallet.Transactions.Remove(_wallet.Transactions.Last());

                await _wallet.Log(exception.Message);

                _logger?.LogError(exception, "Witdraw esnasında hata oluştu");
            }
            finally
            {
                _semaphore.Release();
            }

            return result;
        }

        //TODO:users can query the current balance
        public async Task<decimal> GetCurrentBalance()
        {
            await _wallet.Log($"{DateTime.Now.ToString(LogDateTimeFormat)} tarihinde güncel bakiye sorgulaması: {_wallet.Balance} {_wallet.Currency}");

            return _wallet.Balance;
        }


    }
}

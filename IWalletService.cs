
namespace ConsoleAppWallet
{
    public interface IWalletService
    {
        Task Deposit(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken);
        Task DepositWithError(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken);
        Task<decimal> GetCurrentBalance();
        Task<Result> Withdraw(decimal amount, CurrencyEnum currency, CancellationToken cancellationToken);
    }
}
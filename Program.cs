using ConsoleAppWallet;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();

//TODO:users can add new wallets to this service. The wallet supports different currencies

Wallet tlWallet = new(CurrencyEnum.TL, "M-Cüzdan Sahibi");

logger.LogInformation("M-Cüzdan Sahibi için TL cüzdan oluşturuldu");

IWalletService walletService = new WalletService(loggerFactory.CreateLogger<WalletService>(), tlWallet);

logger.LogInformation("TL Cüzdan servisi başladı");

Wallet usdWallet = new(CurrencyEnum.USD, "M-Cüzdan Sahibi");

IWalletService walletServiceUsd = new WalletService(loggerFactory.CreateLogger<WalletService>(), usdWallet);

logger.LogInformation("M-Cüzdan Sahibi için USD cüzdan oluşturuldu");

await walletService.Deposit(1000.59M, tlWallet.Currency, CancellationToken.None);

logger.LogInformation($"M-Cüzdan Sahibi için {tlWallet.Balance} {tlWallet.Currency} para yatırma işlemi gerçekleşti.");

decimal balance = await walletService.GetCurrentBalance();

logger.LogInformation($"M-Cüzdan Sahibi için yatırma işleminden sonraki güncel {balance} {tlWallet.Currency} bakiye");

//Deposit esnasında alınan hata ile işlem geri alınır
await walletService.DepositWithError(1425.98M, tlWallet.Currency, CancellationToken.None);

logger.LogInformation($"M-Cüzdan Sahibi için 1425,98 {tlWallet.Currency} para yatırma işlemi gerçekleşmedi.");

//hatalı deposit işleminden sonra bakiye ilk yatırılan ile aynı olmalıdır
decimal newbalance = await walletService.GetCurrentBalance();

logger.LogInformation($"M-Cüzdan Sahibi için hatalı yatırma işleminden sonraki güncel {balance} {tlWallet.Currency} bakiye");


//Withdraw işlemi
await walletService.Withdraw(999.99M, tlWallet.Currency ,CancellationToken.None);

logger.LogInformation($"M-Cüzdan Sahibi için 999,99 {tlWallet.Currency} çekim isteği yapıldı.");

balance = await walletService.GetCurrentBalance();

logger.LogInformation($"M-Cüzdan Sahibi için çekimden sonraki güncel {balance} {tlWallet.Currency} bakiye");

//0 bakiyeye rağmen yeni para çekim talebinde bulunma
var result = await walletService.Withdraw(50.25M, tlWallet.Currency, CancellationToken.None);

logger.LogInformation($"M-Cüzdan Sahibi için 50,25 {tlWallet.Currency} çekim isteği yapıldı.");

if(!result.Status)
{
    logger.LogInformation($"M-Cüzdan Sahibi  {result.Description} ");
}


//TODO:all transactions could be able to report
logger.LogInformation("Yapılan transactionların işlem raporu");

tlWallet.Transactions.ForEach(x => logger.LogInformation($"{x.TransactionDate} tarihinde {x.Amount}{x.Currency} miktarında {x.Description}"));

//oluşan cüzdan ve ilgili logları bin klasörü altında yer alır.

Console.ReadKey();





**Açıklama**

Uygulamamız memory tabanlı bir m-cüzdan konsol uygulamasıdır.

**Kod Yapısı**

Uygulamada bir adet Wallet ve WalletService sınıfından oluşmaktadır.
Program.cs üzerinde kodun uygulanması gösterilmektedir.

Wallet.cs sınıfı cüzdan oluşturmak için gerekli özellikleri barındırır. Cüzdan oluşturma wallet.cs sınıfının yeni bir örneğinin constructor bazında alanların
doldurulmasıyla başlatılır. Burada ilgili hesap sahibi için farklı currencylerde cüzdan oluşturabilir.


WalletService.cs cüzdan için gereken para yatırma, para çekme ve bakiye sorgulama metodlarını barındırır.


Oluşan cüzdan bin folderı altında {GUID}-wallet.log olarak görülebilir ve içinde hesap hareketleri incelenebilir.

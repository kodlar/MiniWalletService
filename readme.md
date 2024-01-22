## Açıklama

- Uygulamamız memory tabanlı bir m-cüzdan c# konsol uygulamasıdır.
 
## Kod Yapısı

- Uygulama bir adet Wallet ve WalletService sınıfından oluşmaktadır.
Program.cs üzerinde kodun uygulanması gösterilmektedir.

- Wallet.cs sınıfı cüzdan oluşturmak için gerekli özellikleri barındırır. Cüzdan oluşturma wallet.cs sınıfının yeni bir örneğinin constructor bazında alanların
doldurulmasıyla başlatılır. Burada ilgili hesap sahibi için farklı currencylerde cüzdan oluşturabilir.

- WalletService.cs cüzdan için gereken para yatırma, para çekme ve bakiye sorgulama metodlarını barındırır. Para yatırma ve çekme esnasında thread kontrolü eklenmiştir

- Oluşan cüzdan bin folderı altında {GUID}-wallet.log olarak görülebilir ve içinde hesap hareketleri incelenebilir. Ancak txt uzantılı olarak {GUID}-wallet.txt tarafımdan root'a eklendi

using i4pTest;
namespace testing
{
    public class UnitTest1
    {
        [Fact]
        public void EncryptionPass()
        {
            Encryption encryption = new Encryption("helloworld", "abcdefgijkl"); //Message/key
            Decryption decryption = new Decryption(encryption.Encrypting(), "abcdefgijkl");
            Assert.Equal(decryption.Decrypting(), encryption.Message);
        }
        [Fact]
        public void EncryptionFail()
        {
            Encryption encryption = new Encryption("helloworld", "abc"); //Message/key
            Assert.Fail(encryption.Encrypting());
        }
        [Fact]
        public void DecryptionFail()
        {
            Encryption encryption = new Encryption("helloworld", "abcdefgijkl"); //Message/key
            Decryption decryption = new Decryption(encryption.Encrypting(), "aaaaaaaaaaa");
            Assert.Equal(encryption.Message,decryption.Decrypting());
        }
    }
}
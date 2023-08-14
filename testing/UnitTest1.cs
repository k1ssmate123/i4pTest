using i4pTest;
namespace testing
{
    public class UnitTest1
    {
        [Fact]
        public void EncryptionTest()
        {
            Encryption encryption = new Encryption("helloworld", "abcdefgijkl"); //Message/key
            Decryption decryption = new Decryption(encryption.Encrypting(), "abcdefgijkl");
            Assert.Equal(decryption.Decrypting(), encryption.Message);
        }
    }
}
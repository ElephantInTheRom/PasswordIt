using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordItBackend.Systems;

//Arrange
//Act
//Assert

namespace BackendTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanEncodeData()
        {
            
        }

        [TestMethod]
        public void CanDecodeData()
        {
            //Arrange
            string key = "apple";
            string input = "turtle123";
            //Act
            string scrambled = DataScrambler.ScrambleOnKey(input, key);
            string unscrambled = DataScrambler.UnscrambleOnKey(scrambled, key);
            //Assert
            Assert.AreEqual(input, unscrambled, $"Scrambled data looked like: {scrambled}\nUnscrambled data looked like: {unscrambled}");
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasswordItBackend.Systems;

//Arrange
//Act
//Assert
namespace BackendTest
{
    [TestClass]
    public class ScramblerTests
    {
        [TestMethod]
        public void CanRejectUnallowedChars()
        {
            //Arrange
            char[] chars = { '\"', '\\', '{', '}', '[', ']' };
            //Act
            bool charNotRejected = false;
            foreach(char c in chars)
            {
                if(DataScrambler.CharAllowed(c))
                {
                    charNotRejected = true;
                    break;
                }
            }
            //Assert
            Assert.IsFalse(charNotRejected, "An unallowed character was not rejected by the scrambler as expected.");
        }

        [TestMethod]
        public void CanRejectUnallowedStrings()
        {
            //Arrange
            string inputString = "h@ydenjjkka\"\"{}\\\\[]09991";
            //Act
            bool stringAccepted = DataScrambler.StringAllowed(inputString);
            //Assert
            Assert.IsFalse(stringAccepted, "The string was accepted with unallowed characters.");
        }

        [TestMethod]
        public void CanEncodeData()
        {
            //Arrange
            string key = "apple";
            string input = "turtle123";
            //Act
            string? scrambled = DataScrambler.ScrambleOnKey(input, key);
            Console.WriteLine(scrambled);
            //Assert
            Assert.AreEqual(scrambled, ":42:374:323:273:78:12:918:935:728:", "The scrambled data did not match the expected scramble. Check scramble method.");
        }

        [TestMethod]
        public void CanDecodeData()
        {
            //Arrange
            string key = "aplleplplp1273182731**&747a";
            string input = "_5N$@cyRd0dO84VM1K+nvyx=O&qZp8tXoOVCjiyC%i_KMYVA&^te3fAryuA-aA?PYJXjRrziF1rHln=sNP$Y9NLeC=6ciAxKiR!t%U-RgUKewwut1U=oINGA+*Y@O!6";
            //Act
            string? scrambled = DataScrambler.ScrambleOnKey(input, key);
            string? unscrambled = DataScrambler.UnscrambleOnKey(scrambled, key);
            //Assert
            Assert.AreEqual(input, unscrambled, $"Scrambled data looked like: {scrambled}\nUnscrambled data looked like: {unscrambled}");
        }
    }
}
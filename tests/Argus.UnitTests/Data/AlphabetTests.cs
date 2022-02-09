/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-06
 * @last updated      : 2021-02-06
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Data;
using Xunit;

namespace Argus.UnitTests
{
    public class AlphabetTests
    {
        [Fact]
        public void AlphabetTest()
        {
            var letter = new AlphabetLetter(1);
            Assert.Equal('a', letter.Value);
            Assert.Equal(1, letter.NumericValue);
            Assert.Equal(97, letter.CharacterCodeValue);


            AlphabetLetter newLetter = 'z';
            Assert.Equal('z', newLetter.Value);
            Assert.Equal(26, newLetter.NumericValue);
            Assert.Equal(122, newLetter.CharacterCodeValue);
        }

        [Fact]
        public void LetterEquality()
        {
            AlphabetLetter newLetter = 'z';
            Assert.Equal('z', newLetter.Value);
            Assert.Equal(26, newLetter.NumericValue);
            Assert.Equal(122, newLetter.CharacterCodeValue);

            Assert.True('z' == newLetter.Value);
            Assert.True(newLetter.Value == new AlphabetLetter('z').Value);
        }

        [Fact]
        public void LetterCaseSensitiveEquals()
        {
            AlphabetLetter newLetter = 'h';
            Assert.False(newLetter.Equals('H', true));
            Assert.True(newLetter.Equals('h', true));
        }

        [Fact]
        public void Operators()
        {
            AlphabetLetter newLetter = 'h';
            Assert.True(newLetter == 'h');
            Assert.True('h' == newLetter);
            Assert.True(newLetter == 'h');
            Assert.False(newLetter == 'i');
            Assert.True(newLetter != 'i');
            Assert.False(newLetter != 'h');
        }

        [Fact]
        public void EndOfAlphabet()
        {
            AlphabetLetter letter = 'z';
            Assert.True(letter.Eoa());
        }

        [Fact]
        public void IterationTestCount()
        {
            var letter = new AlphabetLetter('a');
            int i = 1;

            while (!letter.Eoa())
            {
                letter = letter.GetNextLetter();
                i++;
            }

            Assert.True(i == 26);
        }

        [Fact]
        public void IterationLetterCheck()
        {
            var letter = new AlphabetLetter('a');
            int i = 1;

            while (!letter.Eoa())
            {
                Assert.True(letter.NumericValue == i);

                if (i == 16)
                {
                    Assert.True(letter == 'p');
                }

                letter = letter.GetNextLetter();
                i++;
            }

            Assert.True(i == 26);
        }

    }
}

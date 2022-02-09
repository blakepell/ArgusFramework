/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-02-07
 * @last updated      : 2021-02-07
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Argus.Extensions;
using Xunit;

namespace Argus.UnitTests
{
    public class ListTests
    {
        [Fact]
        public void PopLast()
        {
            var list = this.GetList();
            int? item = list.PopLast();
            Assert.Equal(10, item);
            Assert.Equal(9, list.Count);
        }

        [Fact]
        public void PopFirst()
        {
            var list = this.GetList();
            int? item = list.PopFirst();
            Assert.Equal(1, item);
            Assert.Equal(9, list.Count);
        }


        [Fact]
        public void Coalesce()
        {
            var list = this.GetList();
            list.Insert(0, null);

            int? item = list.Coalesce();

            Assert.Equal(1, item);
        }

        [Fact]
        public void FromDelimitedString()
        {
            var list = new List<string>();
            list.FromDelimitedString("One,Two,Three,Four,Five,,Six", ",", true, false);

            Assert.Equal(6, list.Count);
            Assert.Equal("One", list[0]);
            Assert.Equal("Five", list[4]);
            Assert.Equal("Six", list[5]);

            list.FromDelimitedString("One,Two,Three,Four,Five,,Six", ",", false, true);
            Assert.Equal(7, list.Count);
            Assert.Equal("One", list[0]);
            Assert.Equal("Five", list[4]);
            Assert.Equal("Six", list[6]);
        }

        [Fact]
        public void ToDelimitedString()
        {
            var list = this.GetStringList();
            string buf = list.ToDelimitedString(",");
            Assert.Equal("1,2,3,4,5,6,7,8,9,10", buf);

            list = this.GetStringList();
            buf = list.ToDelimitedString(',');
            Assert.Equal("1,2,3,4,5,6,7,8,9,10", buf);

            list = this.GetStringList();
            buf = list.ToDelimitedString(",", "\"");
            Assert.Equal("\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"", buf);
        }

        [Fact]
        public void StringSort()
        {
            var list = this.GetStringList();
            list.StringSort(ListSortDirection.Descending);

            // "9" instead of "10" because it sorts like a string.
            Assert.Equal("9", list[0]);
        }

        [Fact]
        public void LengthLongestLine()
        {
            var list = new List<string>();
            list.Add("One");
            list.Add("Two");
            list.Add("Three");
            list.Add("Four");
            list.Add("Five");
            list.Add("Six");
            list.Add("Seven");
            list.Add("Eight");
            list.Add("Nine");
            list.Add("Ten");

            Assert.Equal(5, list.LengthLongestLine());
        }

        private List<string> GetStringList()
        {
            var list = new List<string>();

            for (int i = 1; i <= 10; i++)
            {
                list.Add(i.ToString());
            }

            return list;
        }

        private List<int?> GetList()
        {
            var list = new List<int?>();

            for (int i = 1; i <= 10; i++)
            {
                list.Add(i);
            }

            return list;
        }
    }
}

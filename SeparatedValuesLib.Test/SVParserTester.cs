using Com.PlanktonSoup.SeparatedValuesLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeparatedValuesLib.Test {
    public class SVParserTester {

        static public string ValidDoc1 = @"John,40
Jane,45
""Ge""""orge"",47
""Sue"""""",41
""Har,ry"",39
Meg,han,38
,,

Last";

        [Fact]
        public void Expect_valid_parsing_outcomes() {
            
            StringReader reader = new StringReader(ValidDoc1);

            SVParser parser = new SVParser(',', '\"', reader);
            
            var lines = parser.ParseAllLines().ToArray();

            Assert.Equal(9, lines.Count());

            Assert.Equal("John", lines.GetCellValue(0, 0));
            Assert.Equal("40", lines.GetCellValue(0, 1));

            Assert.Equal("Jane", lines.GetCellValue(1, 0));
            Assert.Equal("45", lines.GetCellValue(1, 1));

            Assert.Equal("Ge\"orge", lines.GetCellValue(2, 0));
            Assert.Equal("47", lines.GetCellValue(2, 1));

            Assert.Equal("Sue\"", lines.GetCellValue(3, 0));
            Assert.Equal("41", lines.GetCellValue(3, 1));

            Assert.Equal(2, lines.ElementAt(4).Count());
            Assert.Equal("Har,ry", lines.GetCellValue(4, 0));
            Assert.Equal("39", lines.GetCellValue(4, 1));

            Assert.Equal(3, lines.ElementAt(5).Count());
            Assert.Equal("Meg", lines.GetCellValue(5, 0));
            Assert.Equal("han", lines.GetCellValue(5, 1));
            Assert.Equal("38", lines.GetCellValue(5, 2));

            Assert.Equal(3, lines.ElementAt(6).Count());
            Assert.Equal("", lines.GetCellValue(6, 0));
            Assert.Equal("", lines.GetCellValue(6, 1));
            Assert.Equal("", lines.GetCellValue(6, 2));

            Assert.Equal(1, lines.ElementAt(7).Count());

            Assert.Equal(1, lines.ElementAt(8).Count());
        }
    }
}

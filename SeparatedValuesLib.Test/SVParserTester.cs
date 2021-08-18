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

        static public string Doc1 = @"John,40
Jane,45";

        [Fact]
        public void Expect_parsing_outcomes() {
            StringReader reader = new StringReader(Doc1);
            SVParser parser = new SVParser(',', '\\', reader);
            var lines = parser.ParseAll().ToArray();

            Assert.Equal(2, lines.Count());
            Assert.Equal("John", lines.GetCellValue(0, 0));
            Assert.Equal("40", lines.GetCellValue(0, 1));
            Assert.Equal("Jane", lines.GetCellValue(1, 0));
            Assert.Equal("45", lines.GetCellValue(1, 1));
        }
    }
}

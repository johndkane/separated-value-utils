using Com.PlanktonSoup.SeparatedValuesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeparatedValuesLib.Test {

    public class TextQualificationTesting {

        [Fact]
        public void Expect_pinned_col_qualification() {
            StringBuilder sb = new StringBuilder();
            using (var writer = TestableWriterCreator.CreateCsvPersonWriterPinningAgeQualification(sb)) {
                writer.WriteLine("Bob", 30);
                writer.WriteLine("Annie", 20);
            }

            
        }

    }

}

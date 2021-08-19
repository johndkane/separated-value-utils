using Com.PlanktonSoup.SeparatedValuesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeparatedValuesLib.Test {

    public class SVReaderStateTester {
        [Fact]
        public void Expect_In_outcomes() {
            //Assert.True(((SVReaderState)0).In(SVReaderState.S0_StartLine));
            Assert.False(((SVReaderState)0).In(SVReaderState.S1_QualificationOpener));
        }
    }
}

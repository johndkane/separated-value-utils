using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.PlanktonSoup.SeparatedValuesLib {
    static public class SVReaderStateExtensions {

        static public bool In(this SVReaderState test, params SVReaderState[] states)
            => In(test, (IEnumerable<SVReaderState>)states);

        static public bool In(this SVReaderState test, IEnumerable<SVReaderState> states)
                => states?.Any(test => states.Contains(test)) ?? false;
    }
}

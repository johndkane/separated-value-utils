using System.Collections.Generic;
using System.Linq;

namespace Com.PlanktonSoup.SeparatedValuesLib {
    static public class ColumnSpecExtensions {
        static public IEnumerable<ColumnSpec> Spec(this IEnumerable<string> columnNames) {
            return columnNames?.Select(name => new ColumnSpec(name, false));
        }
    }
}

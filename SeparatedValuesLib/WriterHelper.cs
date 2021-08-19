using System;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    static public class WriterHelper {

        /// <summary>
        /// The default method to stringify any value simply makes an internal
        /// call to <see cref="Convert.ToString(object)"/>.
        /// </summary>
        /// <param name="value">The value to convert to a string.</param>
        /// <param name="columnName">The column name of the writer this value belongs to.</param>
        /// <returns></returns>
        static public string StringifyValue(object value, WriterStats stats, ColumnInfo col)
            => Convert.ToString(value);

    }
}

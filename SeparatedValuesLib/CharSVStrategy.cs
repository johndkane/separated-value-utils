namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Contains the strategy to manage values that are to be separated
    /// by a given character <see cref="SeparatorChar"/>.
    /// <remarks>
    /// This is based on creating a standard format for separated value
    /// files as described by <see cref="Separate(string, string)"/>
    /// and <see cref="Escape(string, string)"/>.
    /// </remarks>
    /// </summary>
    public class CharSVStrategy {

        /// <summary>
        /// The separator character to use.
        /// </summary>
        public char SeparatorChar { get; internal protected set; }

        /// <summary>
        /// Constructs a strategy instance around the given separator character <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator"></param>
        public CharSVStrategy(char separator) {
            this.SeparatorChar = separator;
        }

        /// <summary>
        /// <para>
        /// If the <paramref name="value"/> contains the separator character then 
        /// it is surrounded by double quotes. For example, if comma is the separate
        /// and a value contains a comma like (abc,def) then that value is quoted as
        /// "(abc,def)".
        /// </para>
        /// <para>
        /// Furthermore, if the value contains any double quote characters then those are escaped 
        /// using two double quotes. For example value (abc"def) becomes (abc""def)
        /// </para>
        /// </summary>
        /// <param name="value">The value to escape for use by the writing process</param>
        /// <param name="header">The column header of the value if available</param>
        /// <returns></returns>
        public string Escape(string value, string header) {
            string escapedValue = value?.Replace("\"", "\"\"");
            if (escapedValue?.Contains(SeparatorChar) ?? false) {
                string qualifiedValue = string.Concat("\"", escapedValue, "\"");
                return qualifiedValue;
            }
            else
                return escapedValue;
        }

        /// <summary>
        /// Separates the given value from the previous values by prepending
        /// the separator character to it. 
        /// </summary>
        /// <param name="nextValue">The value to separate from the previous values</param>
        /// <param name="column">The column name of the value if available</param>
        /// <returns></returns>
        public string Separate(string nextValue, string column) {
            return string.Concat(SeparatorChar, nextValue);
        }

        /// <summary>
        /// Implicitly casts a .NET <see cref="System.Char"/> into
        /// a <see cref="CharSVStrategy"/>;
        /// </summary>
        /// <param name="separator"></param>
        static public implicit operator CharSVStrategy(char separator)
            => new(separator);
    }
}

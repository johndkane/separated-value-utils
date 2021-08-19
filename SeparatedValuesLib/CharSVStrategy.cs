namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Contains the strategy to manage texts separated
    /// by a given character <see cref="Separator"/>.
    /// </summary>
    public class CharSVStrategy {

        /// <summary>
        /// The separator character to use.
        /// </summary>
        public char Separator { get; internal protected set; }

        /// <summary>
        /// Constructs a strategy instance around the given separator character <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator"></param>
        public CharSVStrategy(char separator) {
<<<<<<< HEAD:SeparatedValuesLib/CharSVStrategy.cs
            this.Separator = separator;
=======
            this.SeparatorChar = separator;
>>>>>>> 44fa28409c4555545145d07eb5a703650c87353d:SeparatedValuesLib/CharSeparatedValuesStrategy.cs
        }

        /// <summary>
        /// <para>
        /// If the <paramref name="text"/> contains the separator character then 
        /// it is surrounded by double quotes. For example, if comma is the separate
        /// and a value contains a comma like (abc,def) then that value is quoted as
        /// "(abc,def)".
        /// </para>
        /// <para>
        /// Furthermore, if the value contains any double quote characters then those are escaped 
        /// using two double quotes. For example value (abc"def) becomes (abc""def)
        /// </para>
        /// </summary>
        /// <param name="text">The value to escape for use by the writing process</param>
        /// <param name="header">The column header of the value if available</param>
        /// <returns></returns>
        public string EscapeText(string text, WriterStats stats, ColumnInfo col) {
            string escapedValue = text?.Replace("\"", "\"\"");
            return escapedValue;
        }

        public bool CellNeedsQualification(CellInfo cell)
            => cell.Text?.Contains(Separator) ?? false;

        public string QualifyText(string text, WriterStats stats, ColumnInfo col) {
            string qualifiedValue = string.Concat("\"", text, "\"");
            return qualifiedValue;
        }

        /// <summary>
        /// Separates the given value from the previous values by prepending
        /// the separator character to it. 
        /// </summary>
        /// <param name="nextValue">The value to separate from the previous values</param>
        /// <param name="column">The column name of the value if available</param>
        /// <returns></returns>
        public string PrependSeparator(string nextValue, WriterStats stat, ColumnInfo col) {
            return string.Concat(Separator, nextValue);
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

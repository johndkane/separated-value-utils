namespace Com.PlanktonSoup.SeparatedValuesLib {
    /// <summary>
    /// Stringifies a value for use by the <see cref="SeparatedValuesWriterBase{TLineObject}"/>.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="columnName">The column name the value belongs to.</param>
    /// <returns>A string version of the value.</returns>
    /// <remarks>
    /// This delegate is used in <see cref="WriterOptions{TLineObject}.FuncStringifyValue"/>
    /// </remarks>
    public delegate string StringifyValueDelegate(object value, string columnName = null);
}

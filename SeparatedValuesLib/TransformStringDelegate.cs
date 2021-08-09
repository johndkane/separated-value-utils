namespace Com.PlanktonSoup.SeparatedValuesLib {
    /// <summary>
    /// Transforms a string vlaue for use by the <see cref="SeparatedValuesWriterBase{TLineObject}"/>.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <param name="columnName">The column name this value belongs to.</param>
    /// <returns>A transformed string.</returns>
    /// <remarks>
    /// This delegate is used in <see cref="WriterOptions{TLineObject}.FuncEscapeValue"/>
    /// and <see cref="WriterOptions{TLineObject}.FuncSeparateValue"/>.
    /// </remarks>
    public delegate string TransformStringDelegate(string value, string columnName = null);
}

namespace Com.PlanktonSoup.SeparatedValuesLib {
    /// <summary>
    /// Transforms a string vlaue for use by the <see cref="WriterBase{TLineObject}"/>.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <param name="columnName">The column name this value belongs to.</param>
    /// <returns>A transformed string.</returns>
    /// <remarks>
    /// This delegate is used in <see cref="WriterOptions{TLineObject}.FuncEscapeText"/>
    /// and <see cref="WriterOptions{TLineObject}.PrependTextSeparator"/>.
    /// </remarks>
    public delegate string TransformStringDelegate(string value, WriterStats stats, ColumnInfo info);
}

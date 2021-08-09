namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Stats maintained about the <see cref="SeparatedValuesWriterBase{TLineObject}"/> process.
    /// </summary>
    public class WriterStats {
        /// <summary>
        /// The total number of lines written including 
        /// the <see cref="HeaderLineCount"/> count.
        /// </summary>
        public long TotalLinesWritten { get; internal set; } = 0;
        /// <summary>
        /// The maximum number of columns seen in a line.
        /// </summary>
        public int MaxColumnsSeen { get; internal set; } = 0;
        /// <summary>
        /// The minimum number of columns seen in a line.
        /// </summary>
        public int MinColumnsSeen { get; internal set; } = 0;
        /// <summary>
        /// If jagged lines have been encountered yet.
        /// </summary>
        public bool AnyJagged => MaxColumnsSeen == MinColumnsSeen;
        /// <summary>
        /// If any lines are written and there are zero columns
        /// </summary>
        public bool ZeroWidthData => TotalLinesWritten != 0 && MaxColumnsSeen == 0;
        /// <summary>
        /// If any header lines have been written.
        /// </summary>
        public bool AnyHeaderLines => HeaderLineCount != 0;
        /// <summary>
        /// The total number of lines in the header.
        /// </summary>
        public long HeaderLineCount { get; internal set; } = 0;

        /// <summary>
        /// Increments the header line count, also causes the 
        /// <see cref="TotalLinesWritten"/> value to update.
        /// </summary>
        /// <param name="howMany">The amount to increment by.</param>
        internal void IncrementHeader(long howMany = 1) {
            HeaderLineCount += howMany;
            IncrementLinesWritten(howMany);
        }

        /// <summary>
        /// Increments the <see cref="TotalLinesWritten"/> count.
        /// </summary>
        /// <param name="howMany"></param>
        internal void IncrementLinesWritten(long howMany = 1) {
            TotalLinesWritten += howMany;
        }
    }
}

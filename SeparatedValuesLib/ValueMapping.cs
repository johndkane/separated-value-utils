namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Ties a value and a <see cref="ColumnSpec"/>.
    /// </summary>
    public struct ValueMapping {

        public ValueMapping(object value, ColumnSpec columnSpec) {
            this.value = value;
            this.colSpec = columnSpec;
        }

        private readonly object value;
        private readonly ColumnSpec colSpec;

        public readonly object Value => value;
        public readonly ColumnSpec ColumnSpec => colSpec;
    }
}

namespace Com.PlanktonSoup.SeparatedValuesLib {
    /// <summary>
    /// Info about a separated-values column in a line being constructed.
    /// </summary>
    public struct ColumnInfo {

        public ColumnInfo(int index, ColumnSpec? col = null) {
            this.Index = index;
            this.Spec = col;
        }

        public int Index { get; internal set; }

        public ColumnSpec? Spec { get; internal set; }

        public bool HasSpec => !(Spec is null);

    }
}

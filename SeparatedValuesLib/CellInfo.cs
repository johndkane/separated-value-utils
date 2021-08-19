namespace Com.PlanktonSoup.SeparatedValuesLib {

    public struct CellInfo {

        public CellInfo(object value, string text, ColumnInfo colInfo) {
            this.value = value;
            this.text = text;
            this.col = colInfo;
        }

        readonly object value;
        readonly string text;
        readonly ColumnInfo col;

        public readonly object Value => value;
        public readonly string Text => text;
        public readonly ColumnInfo Column => col;
    }
}

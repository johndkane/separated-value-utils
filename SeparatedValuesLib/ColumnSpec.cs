namespace Com.PlanktonSoup.SeparatedValuesLib {

    public struct ColumnSpec {

        public string Name { get; internal set; }

        public bool AlwaysQualify { get; internal set; }

        public ColumnSpec(string name, bool alwaysQualify) {
            this.Name = name;
            this.AlwaysQualify = alwaysQualify;
        }

        static public implicit operator ColumnSpec(string columnName)
            => new ColumnSpec(columnName, false);

        static public explicit operator string(ColumnSpec spec)
            => spec.Name;
    }

}

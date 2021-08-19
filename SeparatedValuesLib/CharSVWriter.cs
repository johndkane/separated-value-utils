using System;
using System.Collections.Generic;
using System.IO;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Creates a writer that can write objects and values 
    /// to a Character-Separated-Values format.
    /// </summary>
    /// <typeparam name="TLineObject"></typeparam>
    public class CharSVWriter<TLineObject> : WriterBase<TLineObject>, IDisposable {

        public CharSVStrategy Strategy { get; internal set; }

        public char Separator => Strategy.Separator;

        static public CharSVWriter<TLineObject> Create(char separator, TextWriter writer, bool autoCloseWriter,
            IEnumerable<ColumnSpec> columnsSpec,
            DefineObjectDelegate<TLineObject> defineLine,
            StringifyValueDelegate stringifyValue = null) {

            CharSVStrategy strategy = new CharSVStrategy(separator);

            var options = new WriterOptions<TLineObject>(writer, autoCloseWriter,
                columnsSpec,
                strategy.EscapeText,
                strategy.CellNeedsQualification,
                strategy.QualifyText,
                strategy.PrependSeparator,
                defineLine,
                stringifyValue);

            var svWriter = new CharSVWriter<TLineObject>(options);

            svWriter.Strategy = strategy;

            return svWriter;
        }

        internal CharSVWriter(WriterOptions<TLineObject> options) : base(options) {
            ;
        }

    }
}

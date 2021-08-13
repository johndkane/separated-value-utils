using System.Collections.Generic;
using System.IO;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Extends <see cref="WriterOptions{TLineObject}"/> by adding a
    /// separator character <see cref="SeparatorChar"/> to the options.
    /// </summary>
    /// <typeparam name="TLineObject"></typeparam>
    public class CharSVOptions<TLineObject> : WriterOptions<TLineObject> {

        static public CharSVOptions<TLineObject> Csv(
            TextWriter writer,
            IEnumerable<string> columnSpecOrNull = null,
            DefineObjectDelegate<TLineObject> defineObjectOrDefault = null
        ) {
            var csvOptions = new CharSVOptions<TLineObject>(
                separator: ',',
                writer: writer,
                autoCloseWriter: false,
                columnSpecOrNull: columnSpecOrNull,
                defineLineOrDefault: defineObjectOrDefault,
                stringifyValueOrDefault: null
            );
            
            return csvOptions;
        }

        static public CharSVOptions<TLineObject> Tsv(
            TextWriter writer,
            IEnumerable<string> columnSpec = null,
            DefineObjectDelegate<TLineObject> defineObjectOrDefault = null
        ) {
            var csvOptions = new CharSVOptions<TLineObject>(
                separator: '\t',
                writer: writer,
                autoCloseWriter: false,
                columnSpecOrNull: columnSpec,
                defineLineOrDefault: defineObjectOrDefault,
                stringifyValueOrDefault: null
            );
            
            return csvOptions;
        }

        public CharSVOptions(char separator, TextWriter writer, bool autoCloseWriter,
            IEnumerable<string> columnSpecOrNull,
            DefineObjectDelegate<TLineObject> defineLineOrDefault = null,
            StringifyValueDelegate stringifyValueOrDefault = null)

            : base(writer: writer, autoCloseWriter: autoCloseWriter,
                columnSpecOrNull: columnSpecOrNull) {

            var strategy = new CharSVStrategy(separator);

            AssignFuncEscapeValue(strategy.Escape);
            AssignFuncSeparateValue(strategy.Separate);
            AssignFuncDefineLineOrDefault(defineLineOrDefault);
            AssignFuncStringifyValueOrDefault(stringifyValueOrDefault);

            this.SeparatorChar = separator;
        }

        /// <summary>
        /// The separator character to use.
        /// </summary>
        public char SeparatorChar { get; private set; }
    }
}

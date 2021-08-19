using System;
using System.Collections.Generic;
using System.IO;

namespace Com.PlanktonSoup.SeparatedValuesLib {

    /// <summary>
    /// Options for the <see cref="WriterBase{TLineObject}"/>
    /// </summary>
    public class WriterOptions<TLineObject> : IWriterSettings<TLineObject> {

        /// <summary>
        /// The text writer to use to write lines with.
        /// </summary>
        public TextWriter Writer { get; internal protected set; }

        /// <summary>
        /// Whether to automatically close the <see cref="Writer"/> when done writing.
        /// </summary>
        public bool AutoClose { get; internal protected set; }

        /// <summary>
        /// Function to define a line to write using its column names.
        /// </summary>
        public DefineObjectDelegate<TLineObject> FuncDefineLineObject { get; internal protected set; }

        /// <summary>
        /// Function to convert a <typeparamref name="TLineObject"/> instance into a string.
        /// </summary>
        public StringifyValueDelegate FuncStringifyValue { get; internal protected set; }

        /// <summary>
        /// Function to escape a text for writing.
        /// </summary>
        public TransformStringDelegate FuncEscapeText { get; internal protected set; }

        /// <summary>
        /// Function to perform a custom check to determin if a value needs to be qualified.
        /// </summary>
        public AnalyzeCellDelegate FuncQualificationCheck { get; internal protected set; }

        /// <summary>
        /// Function to qualify text for writing.
        /// </summary>
        public TransformStringDelegate FuncQualifyText { get; internal protected set; }

        /// <summary>
        /// Function to separate the next text from the previous for writing.
        /// </summary>
        public TransformStringDelegate PrependTextSeparator { get; internal protected set; }

        /// <summary>
        /// The columns to use for writing. The order of these columns
        /// is retained by the <see cref="WriterBase{TLineObject}"/>.
        /// </summary>
        public IEnumerable<ColumnSpec> ColumnsSpec { get; internal protected set; }

        /// <summary>
        /// Override to cause all columns to have their lines qualified for writing.
        /// </summary>
        public bool AlwaysQualifyAllColumns { get; internal protected set; }

        /// <summary>
        /// Initialize this options instance.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="autoCloseWriter">Whether to close the writer automatically when done.</param>
        /// <param name="columnsSpecOrNull">The optional column spec. Value can still be written without this.</param>
        /// <param name="escapeText">The function to escape a given text value.</param>
        /// <param name="qualifyText">The function to qualify a text value.</param>
        /// <param name="prependTextSeparator">The function to separate a given text value from the previous.</param>
        /// <param name="defineLineOrDefault">The function to define an object.</param>
        /// <param name="stringifyValueOrDefault">The function to stringify a value.</param>
        public WriterOptions(TextWriter writer, bool autoCloseWriter,
            IEnumerable<ColumnSpec> columnsSpecOrNull,
            TransformStringDelegate escapeText,
            AnalyzeCellDelegate qualificationCheckOrNull,
            TransformStringDelegate qualifyText,
            TransformStringDelegate prependTextSeparator,
            DefineObjectDelegate<TLineObject> defineLineOrDefault = null,
            StringifyValueDelegate stringifyValueOrDefault = null) {

            this.Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            this.AutoClose = autoCloseWriter;

            this.ColumnsSpec = columnsSpecOrNull;

            this.FuncEscapeText = escapeText ?? throw new ArgumentNullException(nameof(escapeText));
            this.PrependTextSeparator = prependTextSeparator ?? throw new ArgumentNullException(nameof(prependTextSeparator));
            this.FuncQualificationCheck = qualificationCheckOrNull;
            this.FuncQualifyText = qualifyText ?? throw new ArgumentNullException(nameof(qualifyText));

            this.FuncDefineLineObject = defineLineOrDefault ?? Helper.DefinePublicProps;
            this.FuncStringifyValue = stringifyValueOrDefault ?? WriterHelper.StringifyValue;
        }

    }
}

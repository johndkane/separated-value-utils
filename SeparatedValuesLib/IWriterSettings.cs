using System.Collections.Generic;
using System.IO;

namespace Com.PlanktonSoup.SeparatedValuesLib {
    public interface IWriterSettings<TLineObject> {

        bool AlwaysQualifyAllColumns { get; }
        bool AutoClose { get; }
        IEnumerable<ColumnSpec> ColumnsSpec { get; }
        DefineObjectDelegate<TLineObject> FuncDefineLineObject { get; }
        TransformStringDelegate FuncEscapeText { get; }
        TransformStringDelegate PrependTextSeparator { get; }
        StringifyValueDelegate FuncStringifyValue { get; }
        AnalyzeCellDelegate FuncQualificationCheck { get; }
        TransformStringDelegate FuncQualifyText { get; }
        TextWriter Writer { get; }
    }
}
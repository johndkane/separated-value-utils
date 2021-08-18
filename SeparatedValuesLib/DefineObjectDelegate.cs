using System.Collections.Generic;

namespace Com.PlanktonSoup.SeparatedValuesLib {
    /// <summary>
    /// Defines an object in a dictionary format.
    /// </summary>
    /// <typeparam name="TObject">The type to define.</typeparam>
    /// <param name="obj">The object to define.</param>
    /// <returns>A dictionary defining the object.</returns>
    /// <remarks>
    /// This delegate is used by <see cref="WriterOptions{TLineObject}.FuncDefineLineObject"/>
    /// </remarks>
    public delegate Dictionary<string, object> DefineObjectDelegate<TObject>(TObject obj, WriterStats stats);
}

# Separated Values Utilities (SVU)

Provides a framework for writing lines of text in separated value formats. 

Ships with support for Comma-Separated Values (CSV) and Tab-Separated Values (TSV) formats, or for any  character you want to use as a separator.

# Installation and Use

**Package Management**

dotnet cli
```powershell
dotnet nuget add source  
dotnet add package Com.PlanktonSoup.SeparatedValuesLib
```

C# usings
```C#
using Com.PlanktonSoup.SeparatedValuesLib
```

**CSV Writer with C#**

This example is based on turning instances of the following Person class into line items in a CSV file.

```C#
class Person {
    public int Age;
    public string FirstName;
    public string LastName;
    public DateTime Birth;
    public string Note;
}
```

Any type of `System.IO.TextWriter` can be used. 
This example writes text lines into a `System.Text.StringBuilder` instance created by this code.

```C#
using System.Text;
using System.IO;

StringBuilder sb = new StringBuilder();
TextWriter sbWriter = new StringWriter(sb);
// The written lines can be accessed with sb.ToString()
```

The following column specification will be given to the writer.

```C#
var columns = new string[] {
    "Fullname",
    "Birthday",
    "Note",
};
```

The CSV Writer code based on the above scaffolding is:

```C#
// define csv options
var csvOptions =  CharSeparatedValuesOptions<Person>.Csv(
    // where to write lines
    writer: sbWriter,
    // provide the column spec
    columnSpecOrNull: columns, // ^ from above
    // provide the function to define a Person in a Dictionary
    defineObjectOrDefault: person => new Dictionary<string, object> {
        // Make the keys the column names to target values into
        { "Fullname", string.Concat(person.FirstName, " ", person.LastName).Trim() },
        { "Age", person.Age },
        { "Birthday", person.Birth },
        { "Note", person.Note },
    });

// create a csv writer by passing it the options
using (var csvWriter = new CharSeparatedValuesWriter<Person>(csvOptions)) {

    // and write lines ...

    // write the header line (this is the column names)
    csvWriter.WriteHeaderLine();

    csvWriter.WriteObjects(new Person {
        FirstName = "John,s",
        LastName = "Doe",
        Birth = new DateTime(1970, 1, 1),
        Note = "test \"record\" 1",
    }, new Person {
        FirstName = "Sarah",
        LastName = "Smith",
        Birth = new DateTime(1973, 7, 3),
        Note = "test record 2",
    });

    /* Or write plain values as a line item without mapping them to the column names.
    * It is the responsiblity of the caller to order the values properly
    */
    csvWriter.WriteLine("John Jacob Jingleheimer Schmidt", new DateTime(1950, 1, 1), "infamous person");

};
```

These CSV lines are written:

```text
FullName,Birthday,Note
"John,s Doe",1/1/1970 12:00:00 AM,test ""record"" 1
Sarah Smith,7/3/1973 12:00:00 AM,test record 2
John Jacob Jingleheimer Schmidt, 1/1/1950,infamous actor
```

Hints pending more documentation:

* For Tab Separated Values format use `CharSeparatedValuesOptions<Person>.Tsv(..)` when creating the writer options.
* Use `File.Open` and an instance of the .NET `StreamWriter` class to capture the lines into a disk file that can be opened in Excel or another program that supports CSV format.

namespace Brimborium.CodeAsCode;

public interface ICascDefinitionSourceCode {
    string Name { get; set; }
    string SourceCodeFilePath { get; set; }
    int SourceCodeLineNumber { get; set; }
}

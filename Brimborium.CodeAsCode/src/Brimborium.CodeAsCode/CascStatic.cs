using System.Runtime.CompilerServices;

namespace Brimborium.CodeAsCode;

// Brimborium.CodeAsCode.CascStatic
public static class CascStatic {
    public static CascElement CascElement(
        [CallerMemberName] string name = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
        ) 
        => new CascElement(name, callerFilePath, callerLineNumber);
}

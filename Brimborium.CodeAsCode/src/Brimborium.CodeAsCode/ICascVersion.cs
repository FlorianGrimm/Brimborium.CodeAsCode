namespace Brimborium.CodeAsCode;

public interface ICascVersion {
    public long CascVersion { get; set; }
}

// TODO: Is this usefull? Review it!
public static class CascVersionUtility {
    private static long _NextVersion = 1;
    public static long GetNextVersion() => System.Threading.Interlocked.Increment(ref _NextVersion);
}
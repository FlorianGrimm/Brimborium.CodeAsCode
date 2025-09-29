namespace Brimborium.CodeAsCode;

public interface ICascDefinition : ICascVersion {
}

public class CascDefinition : ICascDefinition {
    public long CascVersion { get; set; }
}
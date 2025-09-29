using System.Collections;

namespace Brimborium.CodeAsCode;

public class CascListOwned<T> : IEnumerable<T>
    where T : ICascVersion {
    private readonly ICascVersion _Owner;
    private readonly List<T> _Items = new();

    public long CascVersion { get; set; }
    public long CascItemsVersion { get; set; }

    public CascListOwned(ICascVersion owner) {
        this._Owner = owner;
    }

    public void Add(T item) {
        this._Items.Add(item);
        if (this.CascItemsVersion < item.CascVersion) {
            this.CascItemsVersion = item.CascVersion;
        }
        this._Owner.CascVersion = CascVersionUtility.GetNextVersion();
    }

    public void AddRange(IEnumerable<T> listItem) {
        var itemsVersion = this.CascItemsVersion;
        foreach (var item in listItem) {
            if (this.CascVersion < item.CascVersion) {
                this.CascItemsVersion = item.CascVersion;
            }
        }
        this._Owner.CascVersion = CascVersionUtility.GetNextVersion();
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this._Items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this._Items).GetEnumerator();
}
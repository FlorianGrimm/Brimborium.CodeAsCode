using System.Collections;

namespace Brimborium.CodeAsCode;
public class CascList<T> : IEnumerable<T>
    where T : ICascVersion {
    public long CascVersion { get; set; } = 0;
    public long CascItemsVersion { get; set; } = 0;

    public CascList() { }

    public CascList(IEnumerable<T> values) {
        this.Items.AddRange(values);
        var currentVersion = this.CascItemsVersion;
        foreach (var v in values) {
            if (this.CascVersion < v.CascVersion) {
                this.CascItemsVersion = v.CascVersion;
            }
        }
        if (currentVersion != this.CascItemsVersion) {
            this.CascVersion = this.CascItemsVersion;
        } else {
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }

    public List<T> Items { get; } = new List<T>();
    public void Add(T item) {
        this.Items.Add(item);
        if (this.CascVersion < item.CascVersion) {
            this.CascItemsVersion = item.CascVersion;
            this.CascVersion = this.CascItemsVersion;
        } else {
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }

    public void AddRange(IEnumerable<T> listItem) {
        var itemsVersion = this.CascItemsVersion;
        foreach (var item in listItem) {
            if (this.CascVersion < item.CascVersion) {
                this.CascItemsVersion = item.CascVersion;
            }
        }
        if (itemsVersion < this.CascItemsVersion) {
            this.CascVersion = CascVersionUtility.GetNextVersion();
        }
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.Items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Items).GetEnumerator();

    public int Count => this.Items.Count;
    public T this[int index] => this.Items[index];

}

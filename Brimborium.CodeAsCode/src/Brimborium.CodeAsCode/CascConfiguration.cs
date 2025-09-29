
using System.Diagnostics.CodeAnalysis;

namespace Brimborium.CodeAsCode;

public class CascConfiguration {
    private readonly HashSet<object> _ConfiguredObjects = new();
    private readonly Queue<ConfigureAction> _ConfigureActions = new();
    private bool _Configuring = false;
    public CascConfiguration() {
    }

    public void Configure<TToBeConfigured>(
        TToBeConfigured toBeConfigured,
        Func<TToBeConfigured, Action<CascConfiguration>> getAction
        )
        where TToBeConfigured : class {
        if (this._ConfiguredObjects.Contains(toBeConfigured)) {
            return;
        }
        var action = getAction(toBeConfigured);
        this._ConfigureActions.Enqueue(new ConfigureAction<TToBeConfigured>(toBeConfigured, action));
        this.Configuring();
    }

    public void Configure<TToBeConfigured, TArg>(
        TToBeConfigured toBeConfigured,
        TArg arg,
        Func<TToBeConfigured, Action<CascConfiguration, TArg>> getAction
        )
        where TToBeConfigured : class
        where TArg : class {
        if (this._ConfiguredObjects.Contains(toBeConfigured)) {
            return;
        }
        var action = getAction(toBeConfigured);
        this._ConfigureActions.Enqueue(new ConfigureAction<TToBeConfigured, TArg>(toBeConfigured, arg, action));
        this.Configuring();
    }

    public bool IsConfigured<TThat>(TThat that)
        where TThat : class {
        return this._ConfiguredObjects.Contains(that);
    }

    //public bool TryGet<T>([MaybeNullWhen(false)] out T value) {
    //    foreach (var o in this._ConfiguredObjects) { 
    //        if (o is T t) {
    //            value = t;
    //            return true;
    //        }
    //    }
    //    value = default;
    //    return false;
    //}

    private void Configuring() {
        if (this._Configuring) {
            return;
        }
        this._Configuring = true;
        try {
            while (this._ConfigureActions.TryDequeue(out var configureAction)) {
                var toBeConfigured = configureAction.GetToBeConfigured();
                if (this._ConfiguredObjects.Contains(toBeConfigured)) {
                    continue;
                }
                configureAction.Execute(this);
                this._ConfiguredObjects.Add(toBeConfigured);
            }
        } finally {
            this._Configuring = false;
        }
    }

    internal abstract class ConfigureAction {
        public abstract object GetToBeConfigured();
        public abstract void Execute(CascConfiguration cascConfiguration);
    }

    internal class ConfigureAction<TToBeConfigured>
        : ConfigureAction
        where TToBeConfigured : class {
        private readonly TToBeConfigured _ToBeConfigured;
        private readonly Action<CascConfiguration> _Action;
        private bool _IsExecuted = false;
        public ConfigureAction(
            TToBeConfigured toBeConfigured,
            Action<CascConfiguration> action) {
            this._ToBeConfigured = toBeConfigured;
            this._Action = action;
        }

        public override object GetToBeConfigured() => this._ToBeConfigured;

        public override void Execute(CascConfiguration cascConfiguration) {
            if (this._IsExecuted) {
                return;
            }
            this._IsExecuted = true;
            this._Action(cascConfiguration);
        }
    }
    internal class ConfigureAction<TToBeConfigured, TArg>
        : ConfigureAction
        where TToBeConfigured : class
        where TArg : class {
        private readonly Action<CascConfiguration, TArg> _Action;
        private readonly TToBeConfigured _ToBeConfigured;
        private readonly TArg _Arg;
        private bool _IsExecuted = false;
        public ConfigureAction(
            TToBeConfigured toBeConfigured,
            TArg arg,
            Action<CascConfiguration, TArg> action) {
            this._ToBeConfigured = toBeConfigured;
            this._Arg = arg;
            this._Action = action;
        }

        public override object GetToBeConfigured() => this._ToBeConfigured;

        public override void Execute(CascConfiguration cascConfiguration) {
            if (this._IsExecuted) {
                return;
            }
            this._IsExecuted = true;
            this._Action(cascConfiguration, this._Arg);
        }
    }
}
namespace Brimborium.CodeAsCode;

public interface ICascConfigurable {
    void Configure(CascConfiguration cascConfiguration);
}

public interface ICascConfigurable<TArg> {
    void Configure(CascConfiguration cascConfiguration, TArg arg);
}

public sealed class CascConfiguration {
    private readonly HashSet<object> _ConfiguredObjects = new();
    private readonly Queue<IConfigureAction> _ConfigureActions = new();
    private bool _Configuring = false;

    public CascConfiguration() {
    }

    public void ConfigureProperties<T>(T value) where T : class {
        if (value is null) { return; }
        foreach (var property in CascUtility.EnumPropertyOf<ICascDefinition>(value)) {
            if (this._ConfiguredObjects.Contains(property)) {
                continue;
            }

            if (property is ICascConfigurable propertyConfigurable) {
                this.Configure(propertyConfigurable);
            }
        }
    }

    public void RootConfigure<T>(T root)
        where T : class, ICascConfigurable {
        this._ConfigureActions.Enqueue(new ConfigureAction(root, root.Configure));
        this.Configuring<T>(root);
    }

    private void Configuring<T>(T root)
        where T : class {
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
                this._ConfiguredObjects.Add(toBeConfigured);
                configureAction.Execute(this);

                foreach (var property in CascUtility.EnumPropertyOf<ICascConfigurable, ICascConfigurable<T>>(toBeConfigured)) {
                    if (this._ConfiguredObjects.Contains(property)) {
                        continue;
                    }

                    if (property is ICascConfigurable<T> propertyConfigurableT) {
                        this._ConfigureActions.Enqueue(new ConfigureAction<T>(propertyConfigurableT, root, propertyConfigurableT.Configure));
                    } else if (property is ICascConfigurable propertyConfigurable) {
                        this._ConfigureActions.Enqueue(new ConfigureAction(propertyConfigurable, propertyConfigurable.Configure));
                    }
                }
            }
        } finally {
            this._Configuring = false;
        }
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
        this._ConfigureActions.Enqueue(new ConfigureAction(toBeConfigured, action));
        this.Configuring();
    }

    public void Configure<TToBeConfigured>(
        TToBeConfigured toBeConfigured
        )
        where TToBeConfigured : class, ICascConfigurable {
        if (this._ConfiguredObjects.Contains(toBeConfigured)) {
            return;
        }
        this._ConfigureActions.Enqueue(new ConfigureAction(toBeConfigured, toBeConfigured.Configure));
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
        this._ConfigureActions.Enqueue(new ConfigureAction<TArg>(toBeConfigured, arg, action));
        this.Configuring();
    }

    public void Configure<TToBeConfigured, TArg>(
        TToBeConfigured toBeConfigured,
        TArg arg
        )
        where TToBeConfigured : class, ICascConfigurable<TArg>
        where TArg : class {
        if (this._ConfiguredObjects.Contains(toBeConfigured)) {
            return;
        }
        this._ConfigureActions.Enqueue(new ConfigureAction<TArg>(toBeConfigured, arg, toBeConfigured.Configure));
        this.Configuring();
    }

    public bool IsConfigured<TThat>(TThat that)
        where TThat : class {
        return this._ConfiguredObjects.Contains(that);
    }

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
                this._ConfiguredObjects.Add(toBeConfigured);
                configureAction.Execute(this);
                this.ConfigureProperties(toBeConfigured);
            }
        } finally {
            this._Configuring = false;
        }
    }

    internal interface IConfigureAction {
        object GetToBeConfigured();
        void Execute(CascConfiguration cascConfiguration);
    }

    internal sealed class ConfigureAction : IConfigureAction {
        private readonly object _ToBeConfigured;
        private readonly Action<CascConfiguration> _Action;
        private bool _IsExecuted = false;
        public ConfigureAction(
            object toBeConfigured,
            Action<CascConfiguration> action) {
            this._ToBeConfigured = toBeConfigured;
            this._Action = action;
        }

        public object GetToBeConfigured() => this._ToBeConfigured;

        public void Execute(CascConfiguration cascConfiguration) {
            if (this._IsExecuted) {
                return;
            }
            this._IsExecuted = true;
            this._Action(cascConfiguration);
        }
    }
    internal sealed class ConfigureAction<TArg> : IConfigureAction where TArg : class {
        private readonly Action<CascConfiguration, TArg> _Action;
        private readonly object _ToBeConfigured;
        private readonly TArg _Arg;
        private bool _IsExecuted = false;
        public ConfigureAction(
            object toBeConfigured,
            TArg arg,
            Action<CascConfiguration, TArg> action) {
            this._ToBeConfigured = toBeConfigured;
            this._Arg = arg;
            this._Action = action;
        }

        public object GetToBeConfigured() => this._ToBeConfigured;

        public void Execute(CascConfiguration cascConfiguration) {
            if (this._IsExecuted) {
                return;
            }
            this._IsExecuted = true;
            this._Action(cascConfiguration, this._Arg);
        }
    }
}
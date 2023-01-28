// BaseViewModel.cs
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class BaseViewModel : INotifyPropertyChanged {
    readonly Dictionary<string, object> _properties = new Dictionary<string, object>();


    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged {
        add {
            throw new System.NotImplementedException();
        }

        remove {
            throw new System.NotImplementedException();
        }
    }

    protected T Get<T>([CallerMemberName] string propertyName = null) {
        // Return the indicated property
    }

    protected bool Set(object value, [CallerMemberName] string propertyName = null) {
        // Set the indicated property. Return true and raise the PropertyChanged
        // event if the property has changed
    }
}
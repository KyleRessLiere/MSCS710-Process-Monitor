// ButtonViewModel
using System.Windows.Input;

public class ButtonViewModel : BaseViewModel {
    public ICommand Command {
        get => Get<ICommand>();
        set => Set(value);
    }

    public string Label {
        get => Get<string>();
        set => Set(value);
    }
}
/*using System.Windows.Input;

namespace DnDVoiceStudio.Commands;

public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        if (parameter is T t)
            _execute(t);
    }

    public event EventHandler? CanExecuteChanged;
}*/
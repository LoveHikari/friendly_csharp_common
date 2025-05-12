using System;
using System.Windows.Input;

namespace Hikari.Mvvm.Command;
[Obsolete("请使用CommunityToolkit.Mvvm", true)]
public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> _executeMethod;
    private readonly Func<T, bool> _canExecuteMethod;

    public DelegateCommand(Action<T> executeMethod)
        : this(executeMethod, null)
    { }

    public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    {
        _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
        _canExecuteMethod = canExecuteMethod;
    }

    #region ICommand 成员
    /// <summary>
    ///  Method to determine if the command can be executed
    /// </summary>
    public bool CanExecute(T parameter)
    {
        if (_canExecuteMethod != null)
        {
            return _canExecuteMethod(parameter);
        }
        return true;

    }

    /// <summary>
    ///  Execution of the command
    /// </summary>
    public void Execute(T parameter)
    {
        _executeMethod?.Invoke(parameter);
    }

    #endregion


    event EventHandler ICommand.CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    #region ICommand 成员

    public bool CanExecute(object parameter)
    {
        if (parameter == null && typeof(T).IsValueType)
        {
            return (_canExecuteMethod == null);

        }

        return CanExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        Execute((T)parameter);
    }

    #endregion
}

[Obsolete("请使用CommunityToolkit.Mvvm")]
public class DelegateCommand : DelegateCommand<object>
{
    public DelegateCommand(Action<object> executeMethod) : base(executeMethod)
    {
    }

    public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
    {
    }
}
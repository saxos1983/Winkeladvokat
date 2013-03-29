namespace Winkeladvokat
{
    using System;
    using System.Windows.Input;

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> handler;

        private bool isEnabled;

        public RelayCommand(Action<T> handler)
        {
            this.handler = handler;
        }

        public event EventHandler CanExecuteChanged;

        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                if (value != this.isEnabled)
                {
                    this.isEnabled = value;
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.IsEnabled;
        }

        public void Execute(object parameter)
        {
            this.handler((T)parameter);
        }
    }
}
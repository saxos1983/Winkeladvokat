namespace Winkeladvokat
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyEachPropertyChanged()
        {
            var properties = GetType().GetProperties();
            foreach (var property in properties.Where(p => p.CanRead))
            {
                this.NotifyPropertyChanged(property.Name);
            }
        }

        protected virtual void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> item)
        {
            MemberExpression memberExpression;
            var lambdaExpression = (LambdaExpression)item;

            UnaryExpression body = lambdaExpression.Body as UnaryExpression;
            if (body != null)
            {
                var unaryExpression = body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambdaExpression.Body;
            }

            this.NotifyPropertyChanged(memberExpression.Member.Name);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

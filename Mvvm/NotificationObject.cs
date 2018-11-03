using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Mvvm
{
    /// <summary>
    /// INotifyPropertyChanged 인터페이스의 구현클래스입니다.
    /// </summary>
    public class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 해당 Property가 변경되었음에 대한 알림을 설정합니다.
        /// </summary>
        /// <typeparam name="T">직접적으로 타입을 명시하지 않고 action 매개변수를 통해 지정합니다.</typeparam>
        /// <param name="action">해당 Property를 반환하는 표현식입니다.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> action)
        {
            var propertyName = GetPropertyName(action);
            RaisePropertyChanged(propertyName);
        }

        private static string GetPropertyName<T>(Expression<Func<T>> action)
        {
            var expression = (MemberExpression)action.Body;
            var propertyName = expression.Member.Name;
            return propertyName;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 해당 Property가 변경되었을때 발생하는 이벤트입니다.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

    }
}

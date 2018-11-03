using System;
using System.Windows.Input;

namespace Mvvm
{
    /// <summary>
    /// ICommand 인터페이스를 구현한 Deletage입니다.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// MVVM.DelegateCommand.CanExecute 값이 변할때 호출되는 event입니다.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// MVVM.DelegateCommand의 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="command">command</param>
        /// <param name="canExecute">command의 실행 가능 여부를 판단할 떄 호출할 함수입니다.</param>
        public DelegateCommand(Action command, Func<bool> canExecute)
        {
            if (command == null)
                throw new ArgumentNullException("command", "command parameter should not passe null.");
            _canExecute = canExecute;
            _command = command;
        }

        /// <summary>
        /// MVVM.DelegateCommand._command 필드에 연결된 Action을 실행합니다.
        /// </summary>
        /// <param name="parameter">사용하지 않는 매개변수입니다.</param>
        public void Execute(object parameter)
        {
            _command();
        }

        /// <summary>
        /// 현재 command의 실행 가능 여부를 반환합니다.
        /// </summary>
        /// <param name="parameter">사용하지 않는 매개변수입니다.</param>
        /// <returns>실행 가능 여부</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
    }
}

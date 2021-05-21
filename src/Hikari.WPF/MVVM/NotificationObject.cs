using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hikari.WPF.MVVM
{
    /// <summary>
    /// INotifyPropertyChanged 用于通知属性改变（实现ViewModel向View喊话，所有绑定该属性的都会得到通知）
    /// </summary>
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变时调用该方法发出通知
        /// </summary>
        /// <param name="propertyName">[CallerMemberName] 是.net 4.5的新特性，可获取调用者的名称</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
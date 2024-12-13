using System.Windows;
using System.Windows.Input;

namespace Hikari.Mvvm.Attached
{
    /// <summary>
    /// https://blog.csdn.net/sd7o95o/article/details/136640023
    /// 
    /// WPF在触摸屏下，如果有滚动条（ScrollViewer）的情况下，默认包含触底反馈的功能，就是触摸屏滑动到底或从底滑到顶，界面都会出现抖动的情况。
    /// 
    /// </summary>
    public class ManipulationBoundaryFeedbackAttachedProperties
    {
        public static bool GetIsFeedback(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFeedbackProperty);
        }
        public static void SetIsFeedback(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFeedbackProperty, value);
        }
        public static readonly DependencyProperty IsFeedbackProperty =
            DependencyProperty.RegisterAttached("IsFeedback", typeof(bool), typeof(UIElement), new PropertyMetadata(true,
                (s, e) =>
                {
                    var target = s as UIElement;
                    if (target != null)
                        target.ManipulationBoundaryFeedback += Target_ManipulationBoundaryFeedback;
                }));

        private static void Target_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            var target = sender as UIElement;
            if (target != null)
            {
                if (!GetIsFeedback(target))
                {
                    e.Handled = true;
                }
            }
        }
    }
}

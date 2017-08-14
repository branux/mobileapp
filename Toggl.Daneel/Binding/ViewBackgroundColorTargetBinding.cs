using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color.iOS;
using UIKit;

namespace Toggl.Daneel.Binding
{
    public class ViewBackgroundColorTargetBinding : MvxTargetBinding<UIView, MvxColor>
    {
        public const string BindingName = "BackgroundColor";

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public ViewBackgroundColorTargetBinding(UIView target)
            : base(target)
        {
        }
        
        protected override void SetValue(MvxColor value)
        {
            Target.BackgroundColor = value.ToNativeColor();
        }
    }
}

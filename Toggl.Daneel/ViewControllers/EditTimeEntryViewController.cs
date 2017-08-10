using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Foundation.MvvmCross.Converters;
using Toggl.Foundation.MvvmCross.ViewModels;

namespace Toggl.Daneel.ViewControllers
{
    [ModalCardPresentation]
    public partial class EditTimeEntryViewController : MvxViewController
    {
        private const int switchHeight = 24;

        public EditTimeEntryViewController() : base(nameof(EditTimeEntryViewController), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var size = CGSize.Empty;
            size.Height = View.Frame.Height;
            PreferredContentSize = size;

            resizeSwitch();

            var durationConverter = new TimeSpanToDurationWithUnitValueConverter();
            var dateTimeConverter = new DateToTitleStringValueConverter();
            var timeConverter = new DateTimeToTimeConverter();

            var bindingSet = this.CreateBindingSet<EditTimeEntryViewController, EditTimeEntryViewModel>();

            //Text
            bindingSet.Bind(DescriptionLabel)
                      .To(vm => vm.Description);
            bindingSet.Bind(ProjectLabel)
                      .To(vm => vm.Project);
            bindingSet.Bind(TaskLabel)
                      .To(vm => vm.Task);
            bindingSet.Bind(DurationLabel)
                      .To(vm => vm.Duration)
                      .WithConversion(durationConverter);
            bindingSet.Bind(StartDateLabel)
                      .To(vm => vm.StartTime)
                      .WithConversion(dateTimeConverter);
            bindingSet.Bind(StartTimeLabel)
                      .To(vm => vm.StartTime)
                      .WithConversion(timeConverter);
            bindingSet.Bind(BillableSwitch)
                      .To(vm => vm.Billable);

            //Commands
            bindingSet.Bind(CloseButton)
                      .To(vm => vm.CloseCommand);
            bindingSet.Bind(DeleteButton)
                      .To(vm => vm.DeleteCommand);

            bindingSet.Apply();
        }

        private void resizeSwitch()
        {
            var scale = switchHeight / BillableSwitch.Frame.Height;
            BillableSwitch.Transform = CGAffineTransform.MakeScale(scale, scale);
        }
    }
}

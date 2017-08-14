using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using PropertyChanged;
using Toggl.Foundation.DataSources;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using static Toggl.Multivac.Extensions.ObservableExtensions;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    public class EditTimeEntryViewModel : MvxViewModel<ITimeEntry>
    {
        private readonly ITogglDataSource dataSource;
        private readonly IMvxNavigationService navigationService;
        private readonly ITimeService timeService;

        private IDisposable deleteDisposable;
        private IDisposable tickingDisposable;

        public long Id { get; set; }

        public string Description { get; set; }

        public string Project { get; set; }

        public string Task { get; set; }

        [DependsOn(nameof(StartTime), nameof(EndTime))]
        public TimeSpan Duration
            => (EndTime ?? timeService.CurrentDateTime) - StartTime;

        public DateTimeOffset StartTime { get; set; }

        private DateTimeOffset? endTime;
        public DateTimeOffset? EndTime
        {
            get => endTime;
            set
            {
                endTime = value;
                if (endTime != null)
                    tickingDisposable.Dispose();
            }
        }

        public List<string> Tags { get; set; }

        public bool Billable { get; set; }

        public IMvxCommand DeleteCommand { get; }

        public IMvxCommand ConfirmCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }

        public EditTimeEntryViewModel(ITogglDataSource dataSource, IMvxNavigationService navigationService, ITimeService timeService)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(navigationService, nameof(navigationService));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.dataSource = dataSource;
            this.navigationService = navigationService;
            this.timeService = timeService;

            DeleteCommand = new MvxCommand(delete);
            ConfirmCommand = new MvxCommand(confirm);
            CloseCommand = new MvxAsyncCommand(close);
        }

        public override async Task Initialize(ITimeEntry parameter)
        {
            await base.Initialize();

            Id = parameter.Id;
            Description = parameter.Description;
            StartTime = parameter.Start;
            EndTime = parameter.Stop;
            Billable = parameter.Billable;
            Tags = parameter.Tags?.ToList() ?? new List<string>();
            
            if (parameter.ProjectId != null)
                Project = (await dataSource.Projects.GetById((int)parameter.ProjectId)).Name;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            
            if (EndTime == null)
                tickingDisposable = timeService
                    .CurrentDateTimeObservable
                    .Subscribe(_ => RaisePropertyChanged(nameof(Duration )));
                
        }

        private void delete()
        {
            deleteDisposable = dataSource.TimeEntries
                .Delete(Id)
                .Subscribe(onDeleteError, onDeleteCompleted);
        }

        private void onDeleteCompleted()
        {
            close();
        }

        private void onDeleteError(Exception exception){}

        private void confirm()
        {
            throw new NotImplementedException();
        }

        private Task close()
            => navigationService.Close(this);
    }
}

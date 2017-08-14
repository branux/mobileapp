using System;
using System.Collections.Generic;
using System.Reactive;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface ITimeEntriesSource
    {
        IObservable<IDatabaseTimeEntry> CurrentlyRunningTimeEntry { get; }

        IObservable<IEnumerable<IDatabaseTimeEntry>> GetAll();

        IObservable<IDatabaseTimeEntry> Start(DateTimeOffset startTime, string description, bool billable);
         IObservable<IDatabaseTimeEntry> Stop(DateTimeOffset stopTime);

        IObservable<Unit> Delete(long id);
    }
}

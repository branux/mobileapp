using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Ultrawave.ApiClients
{
    public interface IWorkspaceFeaturesApi
    {
        IObservable<List<IWorkspaceFeature>> GetAll();
    }
}

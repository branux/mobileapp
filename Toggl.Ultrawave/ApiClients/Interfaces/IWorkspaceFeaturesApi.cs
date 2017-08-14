using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Ultrawave.ApiClients
{
    public interface IWorkspaceFeaturesApi
    {
        IObservable<List<IWorkspaceFeature>> GetAll();
        IObservable<List<IWorkspaceFeature>> GetEnabledFeatures();
        IObservable<List<IWorkspaceFeature>> GetEnabledFeaturesForWorkspace(long workspaceId);
    }
}

using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Ultrawave.Models;

namespace Toggl.Ultrawave.ApiClients
{
    public interface IWorkspaceFeaturesApi
    {
        IObservable<List<WorkspaceFeature>> GetAll();
        IObservable<List<WorkspaceFeature>> GetEnabledFeatures();
        IObservable<List<WorkspaceFeature>> GetEnabledFeaturesForWorkspace(long workspaceId);
        IObservable<bool> IsFeatureEnabled(long workspaceId, WorkspaceFeatureId featureId);
    }
}

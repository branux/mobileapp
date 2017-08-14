using System;
using System.Linq;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Ultrawave.Models;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Serialization;
using static Toggl.Ultrawave.ApiClients.WorkspaceFeaturesApi;
using System.Reactive.Linq;

namespace Toggl.Ultrawave.ApiClients
{
    internal sealed class WorkspaceFeaturesApi : BaseApi, IWorkspaceFeaturesApi
    {
        private readonly WorkspaceFeaturesEndpoints endPoints;

        public WorkspaceFeaturesApi(WorkspaceFeaturesEndpoints endPoints, IApiClient apiClient, IJsonSerializer serializer,
            Credentials credentials)
            : base(apiClient, serializer, credentials)
        {
            this.endPoints = endPoints;
        }

        public IObservable<List<WorkspaceFeature>> GetAll()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list =>
                    list.ToWorkspaceFeatures().ToList());
        }

        public IObservable<List<WorkspaceFeature>> GetEnabledFeatures()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list =>
                    list.ToWorkspaceFeatures()
                        .Where(wf => wf.Enabled)
                        .ToList());
        }

        public IObservable<List<WorkspaceFeature>> GetEnabledFeaturesForWorkspace(long workspaceId)
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list
                    .Where(wf => wf.WorkspaceId == workspaceId)
                    .ToWorkspaceFeatures()
                    .Where(wf => wf.Enabled)
                    .ToList());
        }

        public IObservable<bool> IsFeatureEnabled(long workspaceId, WorkspaceFeatureId featureId)
        {
            // Is this an overkill to ask for all features only to check one!?

            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list
                    .Where(wf => wf.WorkspaceId == workspaceId)
                    .ToWorkspaceFeatures()
                    .Any(wf => wf.FeatureId == featureId && wf.Enabled));
        }

        public IObservable<List<(WorkspaceFeatureId FeatureId, string Name)>> GetAllRaw()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list.SelectMany(x => x.Features).Select(f => (f.FeatureId, f.Name)).Distinct().ToList());
        }

        internal class WorkspaceFeatureCollectionDTO
        {
            public int WorkspaceId { get; set; }
            public List<WorkspaceFeatureDTO> Features { get; set; }
        }

        internal class WorkspaceFeatureDTO
        {
            public WorkspaceFeatureId FeatureId { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
        }
    }

    internal static class WorkspaceFeatureCollectionExtensions
    {
        internal static IEnumerable<WorkspaceFeature> ToWorkspaceFeatures(this IEnumerable<WorkspaceFeatureCollectionDTO> collection)
            => collection
            .SelectMany(wf => wf.Features.Select(f => f.ToWorkspaceFeature(wf.WorkspaceId)))
            .ToList();

        internal static WorkspaceFeature ToWorkspaceFeature(this WorkspaceFeatureDTO feature, long workspaceId)
            => new WorkspaceFeature
            {
                WorkspaceId = workspaceId,
                Enabled = feature.Enabled,
                FeatureId = feature.FeatureId
            };
    }
}

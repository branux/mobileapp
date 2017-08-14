using System;
using System.Linq;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Multivac.Models;
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

        public IObservable<List<IWorkspaceFeature>> GetAll()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list
                    .ToWorkspaceFeatures()
                    .ToList());
        }

        public IObservable<List<IWorkspaceFeature>> GetEnabledFeatures()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list =>
                    list.ToWorkspaceFeatures()
                        .Where(wf => wf.Enabled)
                        .ToList());
        }

        public IObservable<List<IWorkspaceFeature>> GetEnabledFeaturesForWorkspace(long workspaceId)
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list
                    .Where(wf => wf.WorkspaceId == workspaceId)
                    .ToWorkspaceFeatures()
                    .Where(wf => wf.Enabled)
                    .ToList());
        }

        public IObservable<List<(WorkspaceFeatureId FeatureId, string Name)>> GetAllRaw()
        {
            return CreateObservable<List<WorkspaceFeatureCollectionDTO>>(endPoints.Get, AuthHeader)
                .Select(list => list
                    .SelectMany(x => x.Features)
                    .Select(f => (f.FeatureId, f.Name))
                    .Distinct()
                    .ToList());
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
        internal static IEnumerable<IWorkspaceFeature> ToWorkspaceFeatures(this IEnumerable<WorkspaceFeatureCollectionDTO> collection)
            => collection
            .SelectMany(wf => wf.Features.Select(f => f.ToWorkspaceFeature(wf.WorkspaceId)))
            .ToList();

        internal static IWorkspaceFeature ToWorkspaceFeature(this WorkspaceFeatureDTO feature, long workspaceId)
            => new WorkspaceFeature
            {
                WorkspaceId = workspaceId,
                Enabled = feature.Enabled,
                FeatureId = feature.FeatureId
            };
    }
}

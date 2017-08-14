using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Toggl.Ultrawave.Tests.Integration.BaseTests;
using Xunit;
using Toggl.Multivac;
using System.Diagnostics;
using WorkspaceFeature = Toggl.Ultrawave.Models.WorkspaceFeature;
using Toggl.Ultrawave.ApiClients;
using static Toggl.Ultrawave.ApiClients.WorkspaceFeaturesApi;

namespace Toggl.Ultrawave.Tests.Integration
{
    public class WorkspaceFeaturesApiTests
    {
        public class TheGetAllMethod : AuthenticatedEndpointBaseTests<List<WorkspaceFeature>>
        {
            protected override IObservable<List<WorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
                => togglApi.WorkspaceFeatures.GetAll();

            [Fact, LogTestInfo]
            public async Task ReturnsAllWorkspaceFeatures()
            {
                var (togglClient, user) = await SetupTestUser();

                var features = await CallEndpointWith(togglClient);

                var featuresInEnum = Enum.GetValues(typeof(WorkspaceFeatureId));

                var distinctWorkspacesCount = features.Select(f => f.WorkspaceId).Distinct().Count();

                features.Should().HaveCount(featuresInEnum.Length);
                distinctWorkspacesCount.Should().Be(1);
            }
        }

        public class TheGetEnabledFeaturesMethod : AuthenticatedEndpointBaseTests<List<WorkspaceFeature>>
        {
            protected override IObservable<List<WorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
                => togglApi.WorkspaceFeatures.GetEnabledFeatures();

            [Fact, LogTestInfo]
            public async Task ReturnsEnabledWorkspaceFeatures()
            {
                var (togglClient, user) = await SetupTestUser();

                var features = await CallEndpointWith(togglClient);

                var distinctWorkspacesCount = features.Select(f => f.WorkspaceId).Distinct().Count();

                features.First().FeatureId.Should().Be(WorkspaceFeatureId.Free);
                features.Should().HaveCount(1);
                distinctWorkspacesCount.Should().Be(1);
            }
        }

        public class TheGetEnabledFeaturesForWorkspaceMethod : AuthenticatedEndpointBaseTests<List<WorkspaceFeature>>
        {
            protected override IObservable<List<WorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
                => Observable.Defer(async () =>
                {
                    var user = await togglApi.User.Get();
                    return CallEndpointWith(togglApi, user.DefaultWorkspaceId);
                });

            protected IObservable<List<WorkspaceFeature>> CallEndpointWith(ITogglApi togglApi, long workspaceId)
                => togglApi.WorkspaceFeatures.GetEnabledFeaturesForWorkspace(workspaceId);

            [Fact, LogTestInfo]
            public async Task ReturnsEnabledWorkspaceFeaturesForWorkspace()
            {
                var (togglClient, user) = await SetupTestUser();

                var features = await CallEndpointWith(togglClient, user.DefaultWorkspaceId);

                var myWorkspaces = await togglClient.Workspaces.GetAll();
                var unusedWorkspaceId = myWorkspaces.Max(w => w.Id) + 1;
                var unusedWorkspaceFeatures = await CallEndpointWith(togglClient, unusedWorkspaceId);

                var distinctWorkspacesCount = features.Select(f => f.WorkspaceId).Distinct().Count();

                distinctWorkspacesCount.Should().Be(1);
                features.First().FeatureId.Should().Be(WorkspaceFeatureId.Free);
                features.Should().HaveCount(1);

                unusedWorkspaceFeatures.Should().HaveCount(0);
            }
        }

        public class TheIsFeatureEnabledMethod : AuthenticatedEndpointBaseTests<bool>
        {
            protected override IObservable<bool> CallEndpointWith(ITogglApi togglApi)
                => Observable.Defer(async () =>
                {
                    var user = await togglApi.User.Get();
                    return CallEndpointWith(togglApi, user.DefaultWorkspaceId, WorkspaceFeatureId.Free);
                });

            protected IObservable<bool> CallEndpointWith(ITogglApi togglApi, long workspaceId, WorkspaceFeatureId featureId)
                => togglApi.WorkspaceFeatures.IsFeatureEnabled(workspaceId, featureId);

            [Fact, LogTestInfo]
            public async Task ReturnsEnabledWorkspaceFeatures()
            {
                var (togglClient, user) = await SetupTestUser();

                var isFree = await CallEndpointWith(togglClient, user.DefaultWorkspaceId, WorkspaceFeatureId.Free);
                var isPro = await CallEndpointWith(togglClient, user.DefaultWorkspaceId, WorkspaceFeatureId.Pro);
                var supportsLabourCost = await CallEndpointWith(togglClient, user.DefaultWorkspaceId, WorkspaceFeatureId.LabourCost);
                var supportsTrackingReminders = await CallEndpointWith(togglClient, user.DefaultWorkspaceId, WorkspaceFeatureId.TrackingReminders);

                isFree.Should().BeTrue();
                isPro.Should().BeFalse();
                supportsLabourCost.Should().BeFalse();
                supportsTrackingReminders.Should().BeFalse();
            }
        }
    }
}

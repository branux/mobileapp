﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Toggl.Ultrawave.Tests.Integration.BaseTests;
using Xunit;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Ultrawave.Tests.Integration
{
    public class WorkspaceFeaturesApiTests
    {
        public class TheGetAllMethod : AuthenticatedEndpointBaseTests<List<IWorkspaceFeature>>
        {
            protected override IObservable<List<IWorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
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

        public class TheGetEnabledFeaturesMethod : AuthenticatedEndpointBaseTests<List<IWorkspaceFeature>>
        {
            protected override IObservable<List<IWorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
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

        public class TheGetEnabledFeaturesForWorkspaceMethod : AuthenticatedEndpointBaseTests<List<IWorkspaceFeature>>
        {
            protected override IObservable<List<IWorkspaceFeature>> CallEndpointWith(ITogglApi togglApi)
                => Observable.Defer(async () =>
                {
                    var user = await togglApi.User.Get();
                    return CallEndpointWith(togglApi, user.DefaultWorkspaceId);
                });

            protected IObservable<List<IWorkspaceFeature>> CallEndpointWith(ITogglApi togglApi, long workspaceId)
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
    }
}

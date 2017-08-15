using System;
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
    }
}

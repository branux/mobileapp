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
    public class WorkspaceFeaturesIdEnumTests
    {
        public class BackendEnumValuesCheckTest : AuthenticatedEndpointBaseTests<List<(WorkspaceFeatureId FeatureId, string Name)>>
        {
            protected override IObservable<List<(WorkspaceFeatureId FeatureId, string Name)>> CallEndpointWith(ITogglApi togglApi)
                => (togglApi.WorkspaceFeatures as WorkspaceFeaturesApi).GetAllRaw();

            [Fact, LogTestInfo]
            public async Task TestsWhetherEnumValuesMatchBackendResponse()
            {
                var (togglClient, user) = await SetupTestUser();

                var workspaceFeaturesCollections = await (togglClient.WorkspaceFeatures as WorkspaceFeaturesApi).GetAllRaw();

                var distinctResponseFeatures =
                    workspaceFeaturesCollections
                    .ToDictionary(wf => wf.FeatureId, wf => wf.Name.ToPascalCase());

                var enumFeatures = Enum
                    .GetValues(typeof(WorkspaceFeatureId))
                    .OfType<WorkspaceFeatureId>()
                    .ToDictionary(wf => wf, wf => wf.ToString());

                distinctResponseFeatures.Should().HaveCount(enumFeatures.Count);

                foreach (var featureId in distinctResponseFeatures.Keys)
                {
                    string enumName = enumFeatures[featureId];
                    string responseName = distinctResponseFeatures[featureId];

                    enumName.Should().Be(responseName);
                }
            }
        }
    }

    public static class WorkspaceFeaturesApiTestsExtensions
    {
        public static string ToPascalCase(this string snakeCasedString)
        {
            var segments = snakeCasedString
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1));

            return string.Join("", segments);
        }
    }
}

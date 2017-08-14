using System;
using Newtonsoft.Json;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Ultrawave.Models
{
    public sealed class WorkspaceFeature : IWorkspaceFeature
    {
        // TODO: This property doesn't have a representation in Toggl API
        // So WorkspaceFeature either can't implement IBaseModel or a surrogate implementation must be found
        public long Id { get; set; }

        public long WorkspaceId { get; set; }

        public WorkspaceFeatureId FeatureId { get; set; }

        public bool Enabled { get; set; }
    }
}
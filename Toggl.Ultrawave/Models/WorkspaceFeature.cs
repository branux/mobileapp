using System;
using Newtonsoft.Json;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Ultrawave.Models
{
    public sealed partial class WorkspaceFeature : IWorkspaceFeature
    {
        public long WorkspaceId { get; set; }

        public WorkspaceFeatureId FeatureId { get; set; }

        public bool Enabled { get; set; }
    }
}
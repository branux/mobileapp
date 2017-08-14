using System;

namespace Toggl.Multivac.Models
{
    public interface IWorkspaceFeature
    {
        long WorkspaceId { get; }

        WorkspaceFeatureId FeatureId { get; }

        bool Enabled { get; }
    }
}

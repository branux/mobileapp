using System;

namespace Toggl.Multivac.Models
{
    public interface IWorkspaceFeature : IBaseModel
    {
        long WorkspaceId { get; }

        WorkspaceFeatureId FeatureId { get; }

        bool Enabled { get; }
    }
}

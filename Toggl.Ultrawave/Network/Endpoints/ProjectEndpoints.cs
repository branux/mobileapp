﻿using System;

namespace Toggl.Ultrawave.Network
{
    internal struct ProjectEndpoints
    {
        private readonly Uri baseUrl;

        public ProjectEndpoints(Uri baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public Endpoint Get => Endpoint.Get(baseUrl, "me/projects");

        public Endpoint Post(long workspaceId)
            => Endpoint.Post(baseUrl, $"workspaces/{ workspaceId }/projects");
    }
}

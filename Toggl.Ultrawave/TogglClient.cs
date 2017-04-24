﻿using Toggl.Ultrawave.Clients;
using Toggl.Ultrawave.Network;

namespace Toggl.Ultrawave
{
    public sealed class TogglClient : ITogglClient
    {
        private readonly Endpoints endpoints;

        internal TogglClient(ApiEnvironment apiEnvironment)
        {
            endpoints = new Endpoints(apiEnvironment);

            Tags = new TagsClient();
            User = new UserClient();
            Tasks = new TasksClient();
            Clients = new ClientsClient();
            Projects = new ProjectsClient();
            Workspaces = new WorkspacesClient();
            TimeEntries = new TimeEntriesClient();
        }

        public ITagsClient Tags { get; }
        public IUserClient User { get; }
        public ITasksClient Tasks { get; }
        public IClientsClient Clients { get; }
        public IProjectsClient Projects { get; }
        public IWorkspacesClient Workspaces { get; }
        public ITimeEntriesClient TimeEntries { get; }

        public static ITogglClient WithBaseUrl(ApiEnvironment apiEnvironment) => new TogglClient(apiEnvironment);
    }
}
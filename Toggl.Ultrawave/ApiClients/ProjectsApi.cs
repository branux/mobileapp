﻿using System;
using System.Collections.Generic;
using Toggl.Multivac.Models;
using Toggl.Ultrawave.Models;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Serialization;

namespace Toggl.Ultrawave.ApiClients
{
    internal sealed class ProjectsApi : BaseApi, IProjectsApi
    {
        private readonly ProjectEndpoints endPoints;

        public ProjectsApi(ProjectEndpoints endPoints, IApiClient apiClient, IJsonSerializer serializer, Credentials credentials)
            : base(apiClient, serializer, credentials)
        {
            this.endPoints = endPoints;
        }

        public IObservable<List<IProject>> GetAll()
            => CreateListObservable<Project, IProject>(endPoints.Get, AuthHeader);

        public IObservable<IProject> Create(IProject project)
        {
            var endPoint = endPoints.Post(project.WorkspaceId);
            var projectCopy = project as Project ?? new Project(project);
            var observable = CreateObservable(endPoint, AuthHeader, projectCopy, SerializationReason.Post);
            return observable;
        }
    }
}

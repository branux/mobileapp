using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Toggl.Ultrawave.Tests.Integration.BaseTests;
using Xunit;
using Client = Toggl.Ultrawave.Models.Client;
using UserModel = Toggl.Ultrawave.Models.User;

namespace Toggl.Ultrawave.Tests.Integration
{
    public class ClientsApiTests
    {
        private static Expression<Func<Client, bool>> clientWithSameIdNameAndWorkspaceAs(Client client)
            => c => c.Id == client.Id && c.Name == client.Name && c.WorkspaceId == client.WorkspaceId;

        public class TheGetAllMethod : AuthenticatedEndpointBaseTests<List<Client>>
        {
            protected override IObservable<List<Client>> CallEndpointWith(ITogglApi togglApi)
                => togglApi.Clients.GetAll();

            [Fact, LogTestInfo]
            public async Task ReturnsAllClients()
            {
                var (togglClient, user) = await SetupTestUser();

                var firstClient = new Client { Name = "First", WorkspaceId = user.DefaultWorkspaceId };
                var firstClientPosted = await togglClient.Clients.Create(firstClient);
                var secondClient = new Client { Name = "Second", WorkspaceId = user.DefaultWorkspaceId };
                var secondClientPosted = await togglClient.Clients.Create(secondClient);

                var clients = await CallEndpointWith(togglClient);

                clients.Should().HaveCount(2);
                clients.Should().Contain(clientWithSameIdNameAndWorkspaceAs(firstClientPosted));
                clients.Should().Contain(clientWithSameIdNameAndWorkspaceAs(secondClientPosted));
            }
        }

        public class TheGetAllSinceMethod : AuthenticatedGetSinceEndpointBaseTests<Client>
        {
            protected override IObservable<List<Client>> CallEndpointWith(ITogglApi togglApi, DateTimeOffset threshold)
                => togglApi.Clients.GetAllSince(threshold);

            protected override DateTimeOffset AtDateOf(Client model) => model.At;

            protected override Client MakeUniqueModel(ITogglApi api, UserModel user)
                => new Client {Name = Guid.NewGuid().ToString(), WorkspaceId = user.DefaultWorkspaceId};

            protected override IObservable<Client> PostModelToApi(ITogglApi api, Client model)
                => api.Clients.Create(model);

            protected override Expression<Func<Client, bool>> ModelWithSameAttributesAs(Client model)
                => clientWithSameIdNameAndWorkspaceAs(model);
        }

        public class TheCreateMethod : AuthenticatedPostEndpointBaseTests<Client>
        {
            protected override IObservable<Client> CallEndpointWith(ITogglApi togglApi)
                => Observable.Defer(async () =>
                {
                    var user = await togglApi.User.Get();
                    var client = new Client { Name = Guid.NewGuid().ToString(), WorkspaceId = user.DefaultWorkspaceId };
                    return CallEndpointWith(togglApi, client);
                });

            private IObservable<Client> CallEndpointWith(ITogglApi togglApi, Client client)
                => togglApi.Clients.Create(client);

            [Fact, LogTestInfo]
            public async Task CreatesNewClient()
            {
                var (togglClient, user) = await SetupTestUser();
                var newClient = new Client { Name = Guid.NewGuid().ToString(), WorkspaceId = user.DefaultWorkspaceId };

                var persistedClient = await CallEndpointWith(togglClient, newClient);

                persistedClient.Name.Should().Be(newClient.Name);
                persistedClient.WorkspaceId.Should().Be(newClient.WorkspaceId);
            }
        }
    }
}

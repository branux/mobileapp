using System;
using System.Collections.Generic;
using Toggl.Ultrawave.Models;

namespace Toggl.Ultrawave.ApiClients
{
    public interface IClientsApi
    {
        IObservable<List<Client>> GetAll();
        IObservable<List<Client>> GetAllSince(DateTimeOffset threshold);
        IObservable<Client> Create(Client client);
    }
}

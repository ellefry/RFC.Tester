using SAP.Middleware.Connector;
using System;

namespace RFC.Common.Interfaces
{
    public interface IRfcRepositoryCreator : IDisposable
    {
        RfcRepository Create(string destinationName);
        RfcDestination Destination { get; }
    }
}

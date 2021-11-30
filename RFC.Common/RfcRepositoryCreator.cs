using RFC.Common.Interfaces;
using SAP.Middleware.Connector;
using System;

namespace RFC.Common
{
    public class RfcRepositoryCreator : IRfcRepositoryCreator
    {
        private readonly SapConnectionConfig _sapConnectionConfig;

        private ECCDestinationConfig cfg;

        public RfcRepoWrapper RfcRepoWrapper { get; set; }

        public RfcRepositoryCreator(SapConnectionConfig sapConnectionConfig)
        {
            this._sapConnectionConfig = sapConnectionConfig;
        }

        public RfcDestination Destination { get; private set; }

        public RfcRepoWrapper Create(string destinationName)
        {
            cfg = new ECCDestinationConfig(this._sapConnectionConfig);
            RfcDestinationManager.RegisterDestinationConfiguration(cfg);
            Destination = RfcDestinationManager.GetDestination(destinationName);

            var repo = Destination.Repository;
            return RfcRepoWrapper.Create(repo);
        }

        public void Dispose()
        {
            if (Destination != null)
                RfcSessionManager.EndContext(Destination);
            if (cfg !=null)
                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
        }
    }

    public class RfcRepoWrapper
    {
        public RfcRepository RfcRepository { get; set; }

        public static RfcRepoWrapper Create(RfcRepository rfcRepository)
        {
            return new RfcRepoWrapper {
                RfcRepository = rfcRepository
            };
        }
    }
}

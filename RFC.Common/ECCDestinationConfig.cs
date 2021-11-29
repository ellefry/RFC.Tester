using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;

namespace RFC.Common
{
    public class ECCDestinationConfig : IDestinationConfiguration
    {
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        private readonly SapConnectionConfig _sapConnectionConfig;

        public ECCDestinationConfig(SapConnectionConfig sapConnectionConfig)
        {
            _sapConnectionConfig = sapConnectionConfig;
        }

        public bool ChangeEventsSupported()
        {
            return true;
        }

        /// <summary>
        /// Rfc name
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

            //SAP Parameters
            if (destinationName.Equals("mySAPdestination"))
            {
                parms.Add(RfcConfigParameters.AppServerHost, _sapConnectionConfig.IPAddress);
                parms.Add(RfcConfigParameters.SystemNumber, _sapConnectionConfig.SystemNumber);
                parms.Add(RfcConfigParameters.SystemID, _sapConnectionConfig.SystemID);
                parms.Add(RfcConfigParameters.User, _sapConnectionConfig.User);
                parms.Add(RfcConfigParameters.Password, _sapConnectionConfig.Password);
                parms.Add(RfcConfigParameters.RepositoryPassword, _sapConnectionConfig.ReposotoryPassword);
                parms.Add(RfcConfigParameters.Client, _sapConnectionConfig.Client);
                parms.Add(RfcConfigParameters.Language, _sapConnectionConfig.Language);
                parms.Add(RfcConfigParameters.PoolSize, _sapConnectionConfig.PoolSize);
            }

            return parms;
        }
    }
}

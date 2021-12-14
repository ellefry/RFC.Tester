using RFC.Common;
//using SAP.Middleware.Connector;
using System;
using System.Windows.Forms;


namespace RFC.Connector.Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SapConnectionConfig LoadSapConfig()
        {
            return new SapConnectionConfig
            {
                IPAddress = txtIPAddress.Text,
                SystemNumber = txtSystemNumber.Text,
                SystemID = txtSystemID.Text,
                User = txtUsername.Text,
                Password = txtPassword.Text,
                ReposotoryPassword = txtRepoPassword.Text,
                Client = txtClient.Text,
                Language = txtLanguage.Text,
                PoolSize = txtPoolSize.Text,
            };
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //var pi = new RfcStructureData("test", 10, typeof(int));

            //var sapConnectionConfig = LoadSapConfig();
            //IDestinationConfiguration eCCDestinationConfig = null;
            //RfcDestination rfcDestination = null;

            //try
            //{
            //    eCCDestinationConfig = new ECCDestinationConfig(sapConnectionConfig);
            //    RfcDestinationManager.RegisterDestinationConfiguration(eCCDestinationConfig);
            //    rfcDestination = RfcDestinationManager.GetDestination("mySAPdestination");

            //    var repo = rfcDestination.Repository;

            //    MessageBox.Show("Repository returs!");
            //    RfcSessionManager.EndContext(rfcDestination);
            //    RfcDestinationManager.UnregisterDestinationConfiguration(eCCDestinationConfig);
            //}
            //catch (Exception ex)
            //{
            //    if (rfcDestination != null)
            //        RfcSessionManager.EndContext(rfcDestination);
            //    if (eCCDestinationConfig != null)
            //        RfcDestinationManager.UnregisterDestinationConfiguration(eCCDestinationConfig);
            //    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            //}
        }
    }
}

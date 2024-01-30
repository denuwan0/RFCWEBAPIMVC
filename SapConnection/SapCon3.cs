using SAP.Middleware.Connector;
using System.Data;

namespace RFCWEBAPIMVC.SapConnection
{
    public class SapCon3
    {
        public RfcDestination dest = null;
        public RfcRepository repo = null;
        public SapCon3()
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.AppServerHost, "10.10.50.12"); //prd
            //parms.Add(RfcConfigParameters.AppServerHost, "10.10.50.51"); //dev
            //parms.Add(RfcConfigParameters.AppServerHost, "10.10.50.52"); //qas
            parms.Add(RfcConfigParameters.SystemNumber, "00"); //prd
            parms.Add(RfcConfigParameters.SystemID, "PR1");
            //parms.Add(RfcConfigParameters.SystemID, "DEV");
            //parms.Add(RfcConfigParameters.SystemID, "QAS");
            parms.Add(RfcConfigParameters.User, "BIUSER"); //prd
            //parms.Add(RfcConfigParameters.User, "00117534"); //qas
            //parms.Add(RfcConfigParameters.User, "TESTRFC"); //dev
            parms.Add(RfcConfigParameters.Password, "userbi@1");//prd
            //parms.Add(RfcConfigParameters.Password, "123456A*");//qas
            //parms.Add(RfcConfigParameters.Password, "707602"); //dev
            parms.Add(RfcConfigParameters.Client, "300"); //prd
            //parms.Add(RfcConfigParameters.Client, "120"); // dev
            //parms.Add(RfcConfigParameters.Client, "300"); //qas
            parms.Add(RfcConfigParameters.Language, "EN"); //prd
            //parms.Add(RfcConfigParameters.Name, "PR1"); //prd
            //parms.Add(RfcConfigParameters.Name, "DEV"); //dev
            //parms.Add(RfcConfigParameters.Name, "QAS"); //qas
            parms.Add(RfcConfigParameters.Name, "RFC2023"); //destination programe name
            parms.Add(RfcConfigParameters.IdleTimeout, "60000");//            
            parms.Add(RfcConfigParameters.PoolSize, "10");
            parms.Add(RfcConfigParameters.MaxPoolSize, "10");
            parms.Add(RfcConfigParameters.RepositoryDestination, "RFC2023");
            //parms.Add(RfcConfigParameters.GatewayService, "sapgw00");

            dest = RfcDestinationManager.GetDestination(parms);
            repo = dest.Repository;


        }


        //create Sap IRfcFunction
        public IRfcFunction sapFunc(RfcDestination dest, RfcRepository repo, string sap_function, string param1 = null, string param2 = null)
        {
            IRfcFunction func;

            try
            {
                func = repo.CreateFunction(sap_function);
            }
            catch (RfcCommunicationException e)
            {
                throw;
            }

            if (param1 != null)
            {
                if (param1 == "T001P")
                {
                    func.SetValue("QUERY_TABLE", param1);
                    IRfcTable IrfTable1 = func.GetTable("OPTIONS");
                    //IRfcTable IrfTable2 = sapFunc.GetTable("FIELDS");

                    IrfTable1.Append();
                    IrfTable1.SetValue("TEXT", "WERKS like '" + param2[0] + "%'");
                }
                else
                {
                    func.SetValue("P_PERNR", param1);
                }

            }

            try
            {
                func.Invoke(dest);
            }
            catch (RfcCommunicationException e)
            {
                //throw e;
                // SapConnection();
            }


            return func;
        }

        //convert IRfcTable to Datatable
        public DataTable IrfcToDataTable(IRfcTable IrfTable)
        {
            var dataTable = new DataTable();

            for (int element = 0; element < IrfTable.ElementCount; element++)
            {
                RfcElementMetadata metadata = IrfTable.GetElementMetadata(element);
                dataTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in IrfTable)
            {
                DataRow newRow = dataTable.NewRow();
                for (int element = 0; element < IrfTable.ElementCount; element++)
                {
                    RfcElementMetadata metadata = IrfTable.GetElementMetadata(element);
                    newRow[metadata.Name] = row.GetString(metadata.Name);
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }
    }
}
using Newtonsoft.Json.Linq;
using RFCWEBAPIMVC.Models;
using RFCWEBAPIMVC.SapConnection;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RFCWEBAPIMVC.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeesController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult GetEmployees()
        {
            //create SapConn
            SapConn rfc = new SapConn();
            //create RfcDestination
            RfcDestination sapDest = rfc.SapConnection();
            //create RfcRepository
            /*RfcRepository sapRepo = rfc.sapRepo(sapDest);*/
            RfcRepository sapRepo;
            try
            {
                sapRepo = rfc.sapRepo(sapDest);
            }
            catch (RfcCommunicationException e)
            {
                return Json(new { message = e.Message });
            }

            //create IRfcFunction
            IRfcFunction sapFunc = rfc.sapFunc(sapDest, sapRepo, "YH_ACT_EMP_LIST_RFC");
            //YH_ACT_EMP_LIST
            //get result as a IRfcTable       
            IRfcTable IrfTable = sapFunc.GetTable("it_pa0001");

            IrfTable.Append();

            //convert rfctable to datatable            
            var dataTable = rfc.IrfcToDataTable(IrfTable);

            //check datatable is not empty
            if (dataTable.Rows.Count <= 0)
            {
                return Json(new { message = "NULL DATA" });
            }
            else
            {
                return Json(dataTable);
            }

        }

        // GET api/<controller>
        public IHttpActionResult GetPArea(string company, string pArea, string pSubArea)//
        {
            //create SapConn
            SapConn rfc = new SapConn();
            //create RfcDestination
            RfcDestination sapDest = rfc.SapConnection();
            //create RfcRepository
            /*RfcRepository sapRepo = rfc.sapRepo(sapDest);*/
            RfcRepository sapRepo;
            try
            {
                sapRepo = rfc.sapRepo(sapDest);
            }
            catch (RfcCommunicationException e)
            {
                return Json(new { message = e.Message });
            }
            //create IRfcFunction
            IRfcFunction sapFunc = rfc.sapFunc(sapDest, sapRepo, "RFC_READ_TABLE", "T001P", company);



            //IrfTable2.Append();
            //IrfTable2.SetValue("FIELDNAME", "");
            //IrfTable2.SetValue("OFFSET", "");
            //IrfTable2.SetValue("LENGTH", "");
            //IrfTable2.SetValue("TYPE", "");
            //IrfTable2.SetValue("FIELDTEXT", "");

            //sapFunc.Invoke(sapDest);

            //get result as a IRfcTable       
            IRfcTable IrfTable = sapFunc.GetTable("DATA");

            //convert rfctable to datatable            
            var dataTable = rfc.IrfcToDataTable(IrfTable);

            //get distinct values
            //DataTable distinctTable = dataTable.DefaultView.ToTable( /*distinct*/ true);

            //check datatable is not empty
            if (dataTable.Rows.Count <= 0)
            {
                return Json(new { message = "NULL DATA" });
            }
            else
            {
                return Json(dataTable);
            }

        }

        // GET api/<controller>/5

        public IHttpActionResult Get(string id)
        {
            //create Employee object
            Employee empinfo = new Employee();
            //create SapConn
            SapConn rfc = new SapConn();
            //create RfcDestination
            RfcDestination sapDest = rfc.SapConnection();
            //create RfcRepository
            /*RfcRepository sapRepo = rfc.sapRepo(sapDest);*/
            RfcRepository sapRepo;
            try
            {
                sapRepo = rfc.sapRepo(sapDest);
            }
            catch (RfcCommunicationException e)
            {
                return Json(new { message = e.Message });
            }
            //create IRfcFunction
            IRfcFunction sapFunc = rfc.sapFunc(sapDest, sapRepo, "YH_ACT_EMP_LIST_RFC", id);
            //YH_ACT_EMP_LIST
            //string iDate = "01/17/2020";
            //DateTime oDate = Convert.ToDateTime(iDate);
            //string EPF = "00117534";

            //get result as a IRfcTable              
            IRfcTable IrfTable = sapFunc.GetTable("it_pa0001");

            //convert rfctable to datatable            
            var dataTable = rfc.IrfcToDataTable(IrfTable);

            //check datatable is not empty
            if (dataTable.Rows.Count <= 0)
            {
                return Json(new { message = "Invalid EPF" });
            }
            else
            {
                empinfo.PERNR = Convert.ToInt32(IrfTable.GetString("PERNR"));
                empinfo.ENAME = IrfTable.GetString("ENAME");
                empinfo.WERKS = IrfTable.GetString("WERKS");
                empinfo.BTRTL = IrfTable.GetString("BTRTL");
                empinfo.KOSTL = IrfTable.GetString("KOSTL");
                empinfo.PERSK = IrfTable.GetString("PERSK");
                empinfo.PLANS = Convert.ToInt32(IrfTable.GetString("PLANS"));
                empinfo.MASSN = IrfTable.GetString("MASSN");
                empinfo.MASSG = IrfTable.GetString("MASSG");

                return Json(empinfo);
            }

        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        //[System.Web.Http.HttpGet]
        [Route("api/GetEmpByList/empInfo")]

        public IHttpActionResult GetEmpByList(JObject data)
        {
            //create SapConn
            SapConn rfc = new SapConn();
            //create RfcDestination
            RfcDestination sapDest = rfc.SapConnection();
            //create RfcRepository
            /*RfcRepository sapRepo = rfc.sapRepo(sapDest);*/
            RfcRepository sapRepo;
            try
            {
                sapRepo = rfc.sapRepo(sapDest);
            }
            catch (RfcCommunicationException e)
            {
                return Json(new { message = e.Message });
            }
            //create IRfcFunction
            IRfcFunction sapFunc = rfc.sapFunc(sapDest, sapRepo, "YH_EMP_ACTION_DET");


            //save incoming values
            List<string> comapnyList = data["company"].Select(t => (string)t).ToList();
            List<string> pAreaList = data["pArea"].Select(t => (string)t).ToList();
            List<string> pSubAreaList = data["pSubArea"].Select(t => (string)t).ToList();
            List<string> epfList = data["epfList"].Select(t => (string)t).ToList();
            string period = (string)data["period"];
            string fromDate = (string)data["fromDate"];
            string toDate = (string)data["toDate"];
            string company = comapnyList[0];

            //create R_WERKS table structure 
            IRfcTable IrfTable1 = sapFunc.GetTable("R_WERKS");
            //create R_BTRTL table structure 
            IRfcTable irftable2 = sapFunc.GetTable("R_BTRTL");
            //create R_PERNR table structure 
            IRfcTable IrfTable3 = sapFunc.GetTable("R_PERNR");


            if (pAreaList.Count > 0 && pAreaList[0] != "all")
            {
                //Populate R_WERKS table structure 

                //DataTable dataSet1 = new DataTable();
                //dataSet1.Columns.Add("SIGN");
                //dataSet1.Columns.Add("option");
                //dataSet1.Columns.Add("high");
                //dataSet1.Columns.Add("low");

                //for (var i = 0; i < pAreaList.Count; i++)
                //{
                //    DataRow dataRow = dataSet1.NewRow();
                //    dataRow["SIGN"] = "I";
                //    dataRow["OPTION"] = "EQ";
                //    dataRow["HIGH"] = pAreaList[i];
                //    dataRow["LOW"] = "00000000";

                //    dataSet1.Rows.Add(dataRow);
                //}

                for (var i = 0; i < pAreaList.Count; i++)
                {
                    IrfTable1.Append();
                    IrfTable1.SetValue("sign", "I");
                    IrfTable1.SetValue("option", "EQ");
                    IrfTable1.SetValue("low", pAreaList[i]);
                    IrfTable1.SetValue("high", "00000000");

                    sapFunc.SetValue("R_WERKS", IrfTable1);
                }
            }
            //if personnal sub area is not all
            if (pSubAreaList.Count > 0 && pSubAreaList[0] != "all")
            {
                //Populate R_BTRTL table structure 
                for (var i = 0; i < pSubAreaList.Count; i++)
                {
                    irftable2.Append();
                    irftable2.SetValue("sign", "I");
                    irftable2.SetValue("option", "EQ");
                    irftable2.SetValue("low", pSubAreaList[i]);
                    irftable2.SetValue("high", "00000000");

                    sapFunc.SetValue("R_BTRTL", irftable2);
                }
            }
            //if personal number is not all
            if (epfList.Count > 0 && epfList[0] != "all")
            {
                //Populate R_PERNR table structure 
                for (var i = 0; i < epfList.Count; i++)
                {
                    IrfTable3.Append();
                    IrfTable3.SetValue("sign", "I");
                    IrfTable3.SetValue("option", "EQ");
                    IrfTable3.SetValue("low", epfList[i]);
                    IrfTable3.SetValue("high", "00000000");

                    sapFunc.SetValue("R_PERNR", IrfTable3);
                }
            }

            sapFunc.SetValue("P_BUKRS", company);
            sapFunc.SetValue("P_PERIOD", period);

            //validate time period
            if (period == "O")
            {
                if (fromDate != null && toDate != null && fromDate != "" && toDate != "")
                {
                    DateTime fDate = Convert.ToDateTime(fromDate);
                    DateTime tDate = Convert.ToDateTime(toDate);
                    sapFunc.SetValue("P_SDATE", fDate);
                    sapFunc.SetValue("P_EDATE", tDate);
                }
                else
                {
                    return Json(new { message = "INVALID DATE RANGE" });
                }
            }



            sapFunc.Invoke(sapDest);
            IRfcTable IrfTable = sapFunc.GetTable("IT_OUTPUT");

            //convert rfctable to datatable            
            var dataTable = rfc.IrfcToDataTable(IrfTable);

            //check datatable is not empty
            if (dataTable.Rows.Count <= 0)
            {
                return Json(new { message = "NULL DATA" });
            }
            else
            {
                return Json(dataTable);
            }

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {

        }
    }
}
using RFCWEBAPIMVC.Models;
using RFCWEBAPIMVC.SapConnection;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace RFCWEBAPIMVC.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]


    public class AuthenticationsController : ApiController
    {
        private saprfcdbEntities db = new saprfcdbEntities();

        //// POST: api/Authentications/5
        //[Route("api/Authentications/Getfile")]
        //[HttpPost]
        //public IHttpActionResult Getfile()
        //{
        //    try
        //    {
        //        // Setup session options
        //        var sessionOptions = new SessionOptions
        //        {
        //            Protocol = Protocol.Sftp,
        //            HostName = "10.10.50.51", //sap server
        //            UserName = "root", //server username
        //            Password = "pDEVApr2o22", //server password
        //            SshHostKeyFingerprint = "ecdsa-sha2-nistp256 256 IBu6wmO+LqUm7WsLPBOe5UDROv5ykzWxT2IurK6KRSw", //winscp fingerprint
        //        };

        //        const string localPath = @"C:\DEV"; //local server path
        //        const string remotePath = "/appbackup/RFC/HR/"; //sap server path
        //        const int batches = 3;

        //        var started = DateTime.Now;
        //        int count = 0;
        //        long bytes = 0;

        //        using (var session = new Session())
        //        {
        //            //Console.WriteLine("Connecting...");
        //            session.Open(sessionOptions);

        //            //Console.WriteLine("Starting files enumeration...");
        //            var opts = WinSCP.EnumerationOptions.AllDirectories;
        //            IEnumerable<RemoteFileInfo> files = session.EnumerateRemoteFiles(remotePath, null, opts);
        //            IEnumerator<RemoteFileInfo> filesEnumerator = files.GetEnumerator();

        //            var tasks = new List<Task>();

        //            for (int i = 1; i <= batches; i++)
        //            {
        //                int no = i;

        //                var task = new Task(() =>
        //                {
        //                    using (var downloadSession = new Session())
        //                    {
        //                        //Console.WriteLine($"Starting download {no}...");
        //                        downloadSession.Open(sessionOptions);

        //                        while (true)
        //                        {
        //                            string remoteFilePath;
        //                            lock (filesEnumerator)
        //                            {
        //                                if (!filesEnumerator.MoveNext())
        //                                {
        //                                    break;
        //                                }

        //                                RemoteFileInfo file = filesEnumerator.Current;
        //                                bytes += file.Length;
        //                                count++;
        //                                remoteFilePath = file.FullName;
        //                            }

        //                            string localFilePath = RemotePath.TranslateRemotePathToLocal(remoteFilePath, remotePath, localPath);
        //                            //Console.WriteLine($"Downloading {remoteFilePath} to {localFilePath} in {no}...");
        //                            string localFileDir = Path.GetDirectoryName(localFilePath);
        //                            Directory.CreateDirectory(localFileDir);
        //                            downloadSession.GetFileToDirectory(remoteFilePath, localFileDir, true);// delete remote location files after successful sync
        //                        }

        //                        //Console.WriteLine($"Download {no} done");
        //                    }
        //                });

        //                tasks.Add(task);
        //                task.Start();
        //            }

        //            //Console.WriteLine("Waiting for downloads to complete...");
        //            Task.WaitAll(tasks.ToArray());
        //        }

        //        //Console.WriteLine("Done");

        //        var ended = DateTime.Now;
        //        //Console.WriteLine($"Took {ended - started}");
        //        //Console.WriteLine($"Downloaded {count} files, totaling {bytes:N0} bytes");

        //        return Json("file moved to local");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error: {e}");
        //        return Json(0);
        //    }
        //}

        [Route("api/Authentications/preData")]
        [HttpGet]
        public IHttpActionResult preData()
        {
            using (var ctx = new saprfcdbEntities())
            {
                p_area p_area = new p_area()
                {
                    p_area_name = "Kolonnawa"
                };
                db.p_area.Add(p_area);
                db.SaveChanges();

                p_sub_area p_sub_area = new p_sub_area()
                {
                    p_sub_area_name = "Zone8"
                };
                db.p_sub_area.Add(p_sub_area);
                db.SaveChanges();

            }

            return Json(new { message = "Personell area & sub area updated @ API" });

            ////create SapConn
            //SapConn rfc = new SapConn();
            ////create RfcDestination
            //RfcDestination sapDest = rfc.SapConnection();
            ////create RfcRepository
            ///*RfcRepository sapRepo = rfc.sapRepo(sapDest);*/
            //RfcRepository sapRepo;
            //try
            //{
            //    sapRepo = rfc.sapRepo(sapDest);
            //}
            //catch (RfcCommunicationException e)
            //{
            //    return Json(new { message = e.Message });
            //}

            ////create IRfcFunction
            //IRfcFunction sapFunc = rfc.sapFunc(sapDest, sapRepo, "YH_ACT_EMP_LIST");

            ////get result as a IRfcTable       
            //IRfcTable IrfTable = sapFunc.GetTable("it_pa0001");

            ////IrfTable.Append();

            ////convert rfctable to datatable            
            //var dataTable = rfc.IrfcToDataTable(IrfTable);

            //if (dataTable.Rows.Count <= 0)
            //{
            //    return Json(new { message = "Invalid EPF" });
            //}
            //else
            //{




            //   //IrfTable.GetString("WERKS");
            //    //IrfTable.GetString("BTRTL");
            //    //empinfo.WERKS = IrfTable.GetString("WERKS");
            //    //empinfo.BTRTL = IrfTable.GetString("BTRTL");
            //    //empinfo.KOSTL = IrfTable.GetString("KOSTL");
            //    //empinfo.PERSK = IrfTable.GetString("PERSK");
            //    //empinfo.PLANS = Convert.ToInt32(IrfTable.GetString("PLANS"));
            //    //empinfo.MASSN = IrfTable.GetString("MASSN");
            //    //empinfo.MASSG = IrfTable.GetString("MASSG");


            //}           


        }

        // POST: api/Authentications/5
        [Route("api/Authentications/Authenticate")]
        [HttpGet]
        public IHttpActionResult Authenticate(int uid, string pass, int compCode)
        {

            //IList<Authentication> PersonalDataRecords = null;

            using (var ctx = new saprfcdbEntities())
            {
                List<user> user = db.users.ToList();
                List<company> company = db.companies.ToList();
                List<function> function = db.functions.ToList();
                List<cost_center> costcenters = db.cost_center.ToList();
                List<plant> plant = db.plants.ToList();
                List<location> location = db.locations.ToList();

                var passHashValue = string.Join("", MD5.Create().ComputeHash(
                   Encoding.ASCII.GetBytes(pass)).Select(s => s.ToString("x2")));

                var employeeRecord = from u in user
                                     join c in company
                                on u.emp_company equals c.company_id
                                     join f in function
                                         on u.emp_function equals f.func_id
                                     join cc in costcenters
                                         on f.cost_center equals cc.cost_id
                                     join p in plant
                                         on u.emp_plant equals p.plant_id
                                     join l in location
                                         on u.emp_location equals l.loc_id
                                     where u.emp_epf == uid && u.emp_password == passHashValue && u.is_active == 1
                                     && c.company_code == compCode
                                     select new UserDetails
                                     {
                                         emp_epf = u.emp_epf,
                                         emp_name = u.emp_name,
                                         emp_id = u.emp_id,
                                         emp_user_grp = u.emp_user_grp,
                                         emp_user_sub_grp = u.emp_user_sub_grp,
                                         emp_email = u.emp_email,
                                         emp_mobile = u.emp_mobile,
                                         is_active = u.is_active,
                                         func_id = f.func_id,
                                         func_name = f.func_name,
                                         company_id = c.company_id,
                                         company_code = c.company_code,
                                         company_name = c.company_name,
                                         cost_id = cc.cost_id,
                                         cost_code = cc.cost_code,
                                         cost_name = cc.cost_name,
                                         plant_id = p.plant_id,
                                         plant_code = p.plant_code,
                                         plant_name = p.plant_name,
                                         loc_id = l.loc_id,
                                         loc_name = l.loc_name
                                     };

                return Json(employeeRecord);



            }



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AuthenticationExists(int id)
        {
            return db.users.Count(e => e.emp_id == id) > 0;
        }
    }
}
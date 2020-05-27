using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace WMS.Controllers
{
    public class ASNController : Controller
    {
         
        private WMSEntities db = new WMSEntities();
        // GET: S
        public ActionResult Index()
        {
            IEnumerable<tbl_asn> students = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50020/api/");
                //HTTP GET
                var responseTask = client.GetAsync("ASN");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<tbl_asn>>();
                    readTask.Wait();

                    students = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    students = Enumerable.Empty<tbl_asn>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(students);
        }

        public ActionResult Create()
        {
                ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "client_name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tbl_asn student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50020/api/ASN");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<tbl_asn>("ASN", student);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(student);
        }
        public ActionResult Edit(int id)
        {
            tbl_asn student = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50020/api/");
                //HTTP GET
                var responseTask = client.GetAsync("ASN?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<tbl_asn>();
                    readTask.Wait();

                    student = readTask.Result;
                }
            }
            ViewBag.client_name = new SelectList(db.tbl_client, "client_name", "client_name");
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(tbl_asn student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50020/api/ASN");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<tbl_asn>("ASN", student);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50020/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("ASN/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi;

namespace WebApi.Controllers
{
    public class ASNController : ApiController
    {
        private WMS_dbEntities1 db = new WMS_dbEntities1();

        // GET: api/ASN
        public IQueryable<tbl_asn> Gettbl_asn()
        {
            return db.tbl_asn;
        }

        // GET: api/ASN/5
        [ResponseType(typeof(tbl_asn))]
        public IHttpActionResult Gettbl_asn(int id)
        {
            tbl_asn tbl_asn = db.tbl_asn.Find(id);
            if (tbl_asn == null)
            {
                return NotFound();
            }

            return Ok(tbl_asn);
        }

        // PUT: api/ASN/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_asn(int id, tbl_asn tbl_asn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_asn.order)
            {
                return BadRequest();
            }

            db.Entry(tbl_asn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_asnExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ASN
        [ResponseType(typeof(tbl_asn))]
        public IHttpActionResult Posttbl_asn(tbl_asn tbl_asn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_asn.Add(tbl_asn);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_asnExists(tbl_asn.order))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_asn.order }, tbl_asn);
        }

        // DELETE: api/ASN/5
        [ResponseType(typeof(tbl_asn))]
        public IHttpActionResult Deletetbl_asn(int id)
        {
            tbl_asn tbl_asn = db.tbl_asn.Find(id);
            if (tbl_asn == null)
            {
                return NotFound();
            }

            db.tbl_asn.Remove(tbl_asn);
            db.SaveChanges();

            return Ok(tbl_asn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_asnExists(int id)
        {
            return db.tbl_asn.Count(e => e.order == id) > 0;
        }
    }
}
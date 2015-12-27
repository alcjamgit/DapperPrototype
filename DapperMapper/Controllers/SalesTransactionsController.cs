using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DapperMapper.Models;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
namespace DapperMapper.Controllers
{
    public class SalesTransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SalesTransactions
        public ActionResult Index()
        {
            var salesTransactions = db.SalesTransactions.Include(s => s.Customer).Include(s => s.Vehicle);
            return View(salesTransactions.ToList());
        }

        // GET: SalesTransactions/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var salesTransaction = db.SalesTransactions
                .Include(s => s.Customer)
                .Include(s => s.Vehicle)
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (salesTransaction == null)
            {
                return HttpNotFound();
            }
            return View(salesTransaction);
        }

        // GET: SalesTransactions/Details/5
        public ActionResult GetDetails(int id)
        {
            SalesTransaction result;
            var conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            using (SqlConnection connection = new SqlConnection(conString))
            {
                const string query = "GetSalesTransaction";
                result = connection.Query<SalesTransaction>(query, new { SalesId = id },
                    commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // GET: SalesTransactions/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Description");
            return View();
        }

        // POST: SalesTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VehicleId,CustomerId,CreatedDate")] SalesTransaction salesTransaction)
        {
            if (ModelState.IsValid)
            {
                db.SalesTransactions.Add(salesTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesTransaction.CustomerId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Description", salesTransaction.VehicleId);
            return View(salesTransaction);
        }

        // GET: SalesTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTransaction salesTransaction = db.SalesTransactions.Find(id);
            if (salesTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesTransaction.CustomerId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Description", salesTransaction.VehicleId);
            return View(salesTransaction);
        }

        // POST: SalesTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleId,CustomerId,CreatedDate")] SalesTransaction salesTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesTransaction.CustomerId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "Description", salesTransaction.VehicleId);
            return View(salesTransaction);
        }

        // GET: SalesTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTransaction salesTransaction = db.SalesTransactions.Find(id);
            if (salesTransaction == null)
            {
                return HttpNotFound();
            }
            return View(salesTransaction);
        }

        // POST: SalesTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesTransaction salesTransaction = db.SalesTransactions.Find(id);
            db.SalesTransactions.Remove(salesTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

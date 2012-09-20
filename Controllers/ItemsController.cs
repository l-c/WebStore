using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{ 
    public class ItemsController : Controller
    {
        private WebStoreDBEntities db = new WebStoreDBEntities();

        //
        // GET: /Items/

        public ViewResult Index()
        {
            List<ItemsModels> itemList = new List<ItemsModels>();

            var items = from x in db.Items
                        join y in db.ItemPrices on x.ID equals y.ID
                        select new { x.ID, x.Title, x.Stock, y.Price, y.Entered };

            var latest = from n in items
                         group n by n.ID into g
                         select g.OrderByDescending(t => t.Entered).FirstOrDefault();

            foreach (var i in latest)
            {
                var item = new ItemsModels();

                item.ID = i.ID;
                item.Title = i.Title;
                item.Stock = i.Stock;
                item.Price = i.Price;
                item.Entered = i.Entered;

                itemList.Add(item);
            }

            return View(itemList);
        }

        //
        // GET: /Items/Details/5

        public ViewResult Details(int id)
        {
            Item item = db.Items.Single(i => i.ID == id);
            return View(item);
        }

        //
        // GET: /Items/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Items/Create

        [HttpPost]
        public ActionResult Create(ItemsModels item)
        {
            var _item = new Item();
            _item.Title = item.Title;
            _item.Stock = item.Stock;

            var _itemPrice = new ItemPrice();
            _itemPrice.Price = item.Price;
            _itemPrice.Entered = DateTime.Now;

            // TODO: Duplicate Title Check

            if (ModelState.IsValid)
            {
                db.Items.AddObject(_item);
                db.SaveChanges();

                _itemPrice.ID = _item.ID;

                db.ItemPrices.AddObject(_itemPrice);
                db.SaveChanges();

                return RedirectToAction("Index");  
            }

            return View(item);
        }
        
        //
        // GET: /Items/Edit/5
 
        public ActionResult Edit(int id)
        {
            Item item = db.Items.Single(i => i.ID == id);
            return View(item);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Attach(item);
                db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //
        // GET: /Items/EditPrice/5

        public ActionResult EditPrice(int id)
        {

            var items = from x in db.ItemPrices
                        where x.ID == id
                        orderby x.Entered descending
                        select new { x.ID, x.Price };

            var item = new ItemPrice();

            item.ID = items.First().ID;
            item.Price = items.First().Price;

            return View(item);
        }

        //
        // POST: /Items/EditPrice/5

        [HttpPost]
        public ActionResult EditPrice(ItemPrice item)
        {
            if (ModelState.IsValid)
            {
                var enter = new ItemPrice();
                enter.ID = item.ID;
                enter.Price = item.Price;
                enter.Entered = DateTime.Now;

                db.ItemPrices.AddObject(enter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //
        // GET: /Items/Delete/5
 
        public ActionResult Delete(int id)
        {
            Item item = db.Items.Single(i => i.ID == id);
            return View(item);
        }

        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Item item = db.Items.Single(i => i.ID == id);
            db.Items.DeleteObject(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
﻿//20180312 JPC migration notes from MVC5 to Core 2.0
//Straightforward - only a small challenge with returning statuscodes.
//eg in MVC5 - return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//Search gives this: https://stackoverflow.com/questions/37690114/how-to-return-a-specific-status-code-and-no-contents-from-controller
//Much nicer code as - return BadRequest(); // Http status code 400

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCManukauTech.Models.DB;
using Microsoft.AspNetCore.Http;
using PagedList;
using Newtonsoft.Json;

namespace MVCManukauTech.Controllers
{
    //20180910 LJB Pagination
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CatalogController : Controller
    {
        private readonly XSpy4CoreContext _context;

        public CatalogController(XSpy4CoreContext context)
        {
            _context = context;
        }

        // GET: Catalog?CategoryName=Travel
        public IActionResult Index(string searchString)
        {
            //20180910 LJB limiting the amount of products per page with the FETCH statement in SQL
            string SQL = "SELECT ProductId, Products.CategoryId AS CategoryId, Name, ImageFileName, UnitCost"
                + ", SUBSTRING(Description, 1, 100) + '...' AS Description, CategoryName "
                + "FROM Products INNER JOIN Categories ON Products.CategoryId = Categories.CategoryId ORDER BY ProductID ASC OFFSET @p0 ROWS FETCH NEXT 6 ROWS ONLY";

            string page = Request.Query["page"];

            ////140903 JPC add CategoryName to SELECT list of fields
            //string SQL = "SELECT ProductId, Products.CategoryId AS CategoryId, Name, ImageFileName, UnitCost"
            //    + ", SUBSTRING(Description, 1, 100) + '...' AS Description, CategoryName "
            //    + "FROM Products INNER JOIN Categories ON Products.CategoryId = Categories.CategoryId ";
            //string categoryName = Request.Query["CategoryName"];

            //if (categoryName != null)
            //{
            //    //140903 JPC security check - if ProductId is dodgy then return bad request and log the fact 
            //    //  of a possible hacker attack.  Excessive length or containing possible control characters
            //    //  are cause for concern!  TODO move this into a separate reusable code method with more sophistication.
            //    if (categoryName.Length > 20 || categoryName.IndexOf("'") > -1 || categoryName.IndexOf("#") > -1)
            //    {
            //        //TODO Code to log this event and send alert email to admin
            //        return BadRequest(); // Http status code 400
            //    }

            //    //140903 JPC  Passed the above test so extend SQL
            //    //150807 JPC Security improvement @p0
            //    SQL += " WHERE CategoryName = @p0";
            //    //SQL += " WHERE CategoryName = '{0}'";
            //    //SQL = String.Format(SQL, CategoryName);
            //    //Send extra info to the view that this is the selected CategoryName
            //    ViewBag.CategoryName = categoryName;
            //}

            //150807 JPC Security improvement implementation of @p0
            //    var products = _context.CatalogViewModel.FromSql(SQL, categoryName);
            //    return View(products.ToList());
            //}

            //20180910 LJB Pagination
            var products = _context.CatalogViewModel.FromSql(SQL, Convert.ToInt32(page) * 6);
            return View(products.ToList());

        }


        // GET: Catalog/Details?ProductId=1MORE4ME
        public IActionResult Details(string ProductId)
        {
            if (ProductId == null)
            {
                return BadRequest(); // Http status code 400
            }
            //140903 JPC security check - if ProductId is dodgy then return bad request and log the fact 
            //  of a possible hacker attack.  Excessive length or containing possible control characters
            //  are cause for concern!  TODO move this into a separate reusable code method with more sophistication.
            if (ProductId.Length > 20 || ProductId.IndexOf("'") > -1 || ProductId.IndexOf("#") > -1)
            {
                //TODO Code to log this event and send alert email to admin
                return BadRequest(); // Http status code 400
            }

            //150807 JPC Security improvement implementation of @p0
            //20180312 JPC change to query based on class CatalogViewModel
            //  Change above code to give all of the description rather than summarising with the first 100 characters
            string SQL = "SELECT ProductId, Products.CategoryId AS CategoryId, Name, ImageFileName, UnitCost"
            + ", Description, CategoryName "
            + "FROM Products INNER JOIN Categories ON Products.CategoryId = Categories.CategoryId "
            + " WHERE ProductId = @p0";

            //140904 JPC case of one product to look at the details.
            //  SQL gives some kind of collection where we need to clean that up with ToList() then take element [0]
            //150807 JPC Security improvement implementation of @p0 substitute ProductId
            var product = _context.CatalogViewModel.FromSql(SQL, ProductId).ToList()[0];
            if (product == null)
            {
                return NotFound(); //Http status code 404
            }
            return View(product);
        }

    }
}

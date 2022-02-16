using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Entities;
using System;


namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
        public class ItemsController : ControllerBase  
        {
            private readonly IItemsRepository repository;

            public  ItemsController(IItemsRepository repository)
            {
                this.repository = repository;
            }

            [HttpGet]
            public IEnumerable<Item> GetItems()
            {
                var items = repository.GetItems();
                return items;
            }
            [HttpGet("{id}")]
            public ActionResult<Item> GetItem(Guid Id)
            {
                var item = repository.GetItem(Id);

                if (item is null)
                {
                    return NotFound();
                }
                return item;
            }
        }  
}
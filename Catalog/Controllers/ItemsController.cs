using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;
using Catalog.Dtos;

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
            public IEnumerable<ItemDto> GetItems()
            {
                var items = repository.GetItems().Select( item => item.AsDto());

                return items;
            }
            [HttpGet("{id}")]
            public ActionResult<ItemDto> GetItem(Guid id)
            {
                var item = repository.GetItem(id);

                if (item is null)
                {
                    return NotFound();
                }
                return item.AsDto();
            }
            
            // POST / items
            [HttpPost]
            public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
            {
                Item item = new(){
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                repository.CreateItem(item);

                return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
            }

            //put / items/{id}
            [HttpPut("{id}")]
            public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
            {
                var exisitingItem = repository.GetItem(id);

                if (exisitingItem is null)
                {
                    return NotFound();
                }

                Item updatedItem = exisitingItem with {
                    Name = itemDto.Name,
                    Price = itemDto.Price
                };

                repository.UpdateItem(updatedItem);

                return NoContent();


            }


            // Delete /items/{id}
            [HttpDelete("{id}")]
            public ActionResult DeleteItem(Guid id)
            {
                var exisitingItem = repository.GetItem(id);

                if (exisitingItem is null)
                {
                    return NotFound();
                }

                repository.DeleteItem(id);

                return NoContent();
            }
        }  
}
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Api.Entities;
using System;
using System.Linq;
using Catalog.Api.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("items")]
        public class ItemsController : ControllerBase  
        {
            private readonly IItemsRepository repository;
            private readonly ILogger<ItemsController> logger;

            public  ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
            {
                this.repository = repository;
                this.logger = logger;
            }

            [HttpGet]
            public async Task<IEnumerable<ItemDto>> GetItemsAsync(string name = null)
            {
                var items = (await repository.GetItemsAsync())
                            .Select( item => item.AsDto());

                if (!string.IsNullOrWhiteSpace(name))
                {
                    items = items.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                }

                logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

                return items;
            }
            [HttpGet("{id}")]
            public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
            {
                var item = await repository.GetItemAsync(id);

                if (item is null)
                {
                    return NotFound();
                }
                return item.AsDto();
            }
            
            // POST / items
            [HttpPost]
            public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
            {
                Item item = new(){
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Description = itemDto.Description,
                    Price = itemDto.Price,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                await repository.CreateItemAsync(item);

                return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto());
            }

            //put / items/{id}
            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
            {
                var exisitingItem = await repository.GetItemAsync(id);

                if (exisitingItem is null)
                {
                    return NotFound();
                }

                exisitingItem.Name = itemDto.Name;
                exisitingItem.Price = itemDto.Price;

                await repository.UpdateItemAsync(exisitingItem);

                return NoContent();


            }


            // Delete /items/{id}
            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteItemAsync(Guid id)
            {
                var exisitingItem = await repository.GetItemAsync(id);

                if (exisitingItem is null)
                {
                    return NotFound();
                }

                await repository.DeleteItemAsync(id);

                return NoContent();
            }

        
    }  
}
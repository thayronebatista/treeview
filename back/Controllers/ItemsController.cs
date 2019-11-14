using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ItemsApi.Models;
using System.Linq;

namespace ItemsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<Item> Items = Item.Spawn();

        /// <summary>
        /// Lists all items.
        /// </summary>       
        /// <returns></returns>   
        /// <response code="200">Returns all items</response>
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return Items;
        }   

        /// <summary>
        /// Creates an Item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// POST /Item
        /// {
        ///    name: "MyItem",
        ///    parentId: "4c06d4c6-22ea-46ed-88ac-a0c2831a0a43"
        /// }
        ///
        /// </remarks>
        /// <param name="item"></param>        
        /// <returns></returns>
        /// <response code="204">Returns no content</response>
        [HttpPost()]
        public IActionResult Post(Item item)
        {
            Items.Add(item);
            return NoContent();
        }

        /// <summary>
        /// Updates an Item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// PATCH /Item
        /// {
        ///    id: "afb934f4-5551-4457-81ff-74073502d124",
        ///    name: "My item updated"
        /// }
        ///
        /// </remarks>
        /// <param name="item"></param>        
        /// <returns></returns>
        /// <response code="204">Returns no content</response>
        [HttpPatch("{id}")]
        public IActionResult Put(Item item)
        {
            if (item.ParentId == null)
            { //Raiz
                Item mItem = Items.FirstOrDefault(x => x.Id == item.Id);
                if (mItem == null)
                {
                    mItem = new Item(item.Name);
                    Items.Add(mItem);
                }
                mItem.Children = item.Children;
                mItem.Name = item.Name;
            }
            else
            {
                Item pai = getList(Items).FirstOrDefault(x => x.Id == item.ParentId );
                if(pai == null){
                    //erro
                }

                Item mItem = pai.Children.FirstOrDefault(x => x.Id == item.Id);
                if (mItem == null)
                {
                    mItem = new Item(item.Name);
                    pai.Children.Add(mItem);
                }
                mItem.Children = item.Children;
                mItem.Name = item.Name;
            }
            return NoContent();
        }
        /// <summary>
        /// Deletes an Item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// DELETE /Item/afb934f4-5551-4457-81ff-74073502d124
        /// </remarks>        
        /// <returns></returns>
        /// <response code="204">Returns no content</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var items = getList(Items);
            Item item = items.FirstOrDefault(x => x.Id == id);
            
            if(item.Children.Any()){
                foreach (var i in item.Children)
                {
                    i.ParentId = item.ParentId;
                }
            }

            items.Remove(item);
            return NoContent();
        }
        private List<Item> getList(IList<Item> itemList){
            List<Item> items = new List<Item>();
            foreach(var item in itemList){
                items.Add(item);
                if(item.Children != null)
                  items = items.Concat(getList(item.Children)).ToList();
            }
            return items;
        }
    }
}

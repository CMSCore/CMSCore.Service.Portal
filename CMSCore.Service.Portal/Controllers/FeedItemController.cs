namespace CMSCore.Service.Portal.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Library.Core.Attributes;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Library.Messages.Create;
    using Library.Messages.Update;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class FeedItemController : Controller
    {
        private readonly IClusterClient _client;

        public FeedItemController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]CreateFeedItemViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(Guid.NewGuid().ToString());
            var result = await grain.CreateFeedItem(model);
            return Json(result);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody]UpdateFeedItemViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(model.Id);
            var result = await grain.UpdateFeedItem(model);
            return Json(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeleteFeedItem(id);
            return Json(result);
        }
    }
}
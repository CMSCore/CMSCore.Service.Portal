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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class PageController : Controller
    {
        private readonly IClusterClient _client;

        public PageController(IClusterClient client)
        {
            this._client = client;
        }

        [Authorize("create:content")]
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreatePageViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(Guid.NewGuid().ToString());
            var result = await grain.CreatePage(model);
            return Json(result);
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody] UpdatePageViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(model.Id);
            var result = await grain.UpdatePage(model);
            return Json(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeletePage(id);
            return Json(result);
        }
    }
}
namespace CMSCore.Service.Portal.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/{controller}")]
    public class PageController : Controller
    {
        private readonly IClusterClient _client;

        public PageController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePageViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(Guid.NewGuid().ToString());
            var result = await grain.CreatePage(model);
            return Json(result);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdatePageViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(model.Id);
            var result = await grain.UpdatePage(model);
            return Json(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeletePage(id);
            return Json(result);
        }
    }
}
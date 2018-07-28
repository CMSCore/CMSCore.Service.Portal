namespace CMSCore.Service.Portal.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Library.Core.Attributes;
    using Library.GrainInterfaces;
    using Library.Messages;
    using Library.Messages.Update;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class ContentController : Controller
    {
        private readonly IClusterClient _client;

        public ContentController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromBody]UpdateContentViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(model.ContentId);
            var result = await grain.UpdateContent(model);
            return Json(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeleteContent(id);
            return Json(result);
        }
    }
}
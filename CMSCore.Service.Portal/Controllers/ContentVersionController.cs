namespace CMSCore.Service.Portal.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Library.Core.Attributes;
    using Library.GrainInterfaces;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class ContentVersionController : Controller
    {
        private readonly IClusterClient _client;

        public ContentVersionController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeleteContentVersion(id);
            return Json(result);
        }
    }
}
namespace CMSCore.Service.Portal.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Library.Core.Attributes;
    using Library.GrainInterfaces;
    using Library.Messages.Create;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/[controller]")]
    public class CommentController : Controller
    {
        private readonly IClusterClient _client;

        public CommentController(IClusterClient client)
        {
            this._client = client;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]CreateCommentViewModel model)
        {
            var grain = this._client.GetGrain<IManageContentGrain>(Guid.NewGuid().ToString());
            var result = await grain.CreateComment(model);
            return Json(result);
        }
  
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete([Required] string id)
        { 
            var grain = this._client.GetGrain<IManageContentGrain>(id);
            var result = await grain.DeleteComment(id);
            return Json(result);
        }
    }
}
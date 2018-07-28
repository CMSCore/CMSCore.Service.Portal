namespace CMSCore.Service.Portal.Controllers
{
    using Library.Core.Service;
    using Microsoft.AspNetCore.Mvc;
    using Orleans;

    [Route("api/v1/cluster")]
    public class ClusterController : ClusterControllerBase
    {
        public ClusterController(IClusterClient client) : base(client)
        {
        }
    }
}
//using AssetNex.API.Models.DomainModel;
//using AssetNex.API.Repositories.Interface;
//using Microsoft.AspNetCore.Mvc;

//namespace AssetNex.API.Controllers
//{
   
//        [ApiController]
//        [Route("api/test")]
//        public class TestController : ControllerBase
//        {

//            private readonly IAlertsRepository _alerts;
//            public TestController(IAlertsRepository alerts) { _alerts = alerts; }

//            [HttpPost("send")]
//            public async Task<IActionResult> Send([FromBody] InventoryAlert a)
//            {
//                await _alerts.BroadcastAlertAsync(a);
//                return Ok();
//            }
//        }


//    }
//}

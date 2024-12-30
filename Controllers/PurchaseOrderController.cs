using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopsy_Project.BL;
using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IBL_PurchaseOrder _bL_PurchaseOrder;

        public PurchaseOrderController(IBL_PurchaseOrder bL_PurchaseOrder)
        {
            _bL_PurchaseOrder = bL_PurchaseOrder;
        }

        [HttpPost("AddPurchaseOrder")]
        public IActionResult AddPurchaseOrder([FromBody] AddPurchaseOrderRequest addPurchaseOrderRequest)
        {
            if (ModelState.IsValid)
            {
                _bL_PurchaseOrder.AddPurchaseOrder(addPurchaseOrderRequest);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("AddPurchaseOrders")]
        public IActionResult AddNewPurchaseOrder([FromBody] List<PurchaseOrder> purchaseOrders)
        {
            if (ModelState.IsValid)
            {
                _bL_PurchaseOrder.AddNewPurchaseOrder(purchaseOrders);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
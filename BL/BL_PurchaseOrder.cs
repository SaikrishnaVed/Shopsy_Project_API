using Shopsy_Project.Interfaces;
using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.BL
{
    public class BL_PurchaseOrder : IBL_PurchaseOrder
    {
        private readonly IDAL_PurchaseOrder _dAL_PurchaseOrder;

        public BL_PurchaseOrder(IDAL_PurchaseOrder dAL_PurchaseOrder)
        {
            _dAL_PurchaseOrder = dAL_PurchaseOrder;
        }
        public void AddPurchaseOrder(AddPurchaseOrderRequest addPurchaseOrderRequest)
        {
            _dAL_PurchaseOrder.AddPurchaseOrder(addPurchaseOrderRequest);
        }

        public void AddNewPurchaseOrder(List<PurchaseOrder> purchaseOrders)
        {
            _dAL_PurchaseOrder.AddNewPurchaseOrder(purchaseOrders);
        }
    }
}
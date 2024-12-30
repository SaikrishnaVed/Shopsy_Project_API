using Shopsy_Project.Models;
using Shopsy_Project.Models.RequestModels;

namespace Shopsy_Project.Interfaces
{
    public interface IBL_PurchaseOrder
    {
        void AddPurchaseOrder(AddPurchaseOrderRequest addPurchaseOrderRequest);
        void AddNewPurchaseOrder(List<PurchaseOrder> purchaseOrders);
    }
}

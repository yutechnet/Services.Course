namespace BpeProducts.Services.Course.Host.TempSectionContracts
{
    public abstract class TenantResponseBase : ResponseBase
    {
        public int TenantId { get; set; }
    }
}

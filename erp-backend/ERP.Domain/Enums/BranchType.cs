namespace ERP.Domain.Enums
{
    /// <summary>
    /// Defines the type of warehouse in the hierarchy
    /// </summary>
    public enum BranchType
    {
        /// <summary>
        /// Main warehouse - can receive stock from suppliers
        /// </summary>
        Main = 1,

        /// <summary>
        /// Branch warehouse - receives stock from main warehouse
        /// </summary>
        Branch = 2,

        /// <summary>
        /// Sub warehouse - receives stock from branch or main warehouse
        /// </summary>
        Sub = 3
    }
}

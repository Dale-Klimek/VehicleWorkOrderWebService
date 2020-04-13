namespace VehicleWorkOrder.Shared.Enum
{
    using System.ComponentModel;
    
    public enum FeatureAdded
    {
        None,
        Tint,
        [Description("Paint Protection Film")]
        ProtectionFilm,
        [Description("Ceramic Coating")]
        CeramicCoating,
        [Description("Paint Protection Film & Tint")]
        All,
        
    }
}

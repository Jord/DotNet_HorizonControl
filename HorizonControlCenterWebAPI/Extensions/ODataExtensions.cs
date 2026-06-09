using Microsoft.OData.Edm;

namespace HorizonControlCenterWebAPI.Extensions
{
    public static class ODataExtensions
    {
        public static IEdmModel BuildEdmModel()
        {
            var builder = new Microsoft.OData.ModelBuilder.ODataConventionModelBuilder();
         
            return builder.GetEdmModel();
        }
    }
}

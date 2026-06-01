using System.Text.RegularExpressions;

namespace HorizonControlCenterWebAPI
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value is null)
            {
                return null;
            }

            // Replace uppercase letters with hyphen and lowercase the entire string
            return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
        }
    }
}

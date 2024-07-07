
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace CodinaxProjectMvc.Resources
{
    public class LanguageService
    {
        private readonly IStringLocalizer _stringLocalizer;

        public LanguageService( IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyname = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _stringLocalizer = factory.Create(nameof(SharedResource), assemblyname.Name);
        }

        public LocalizedString GetLocalizedHTML(string key)
        {
            return _stringLocalizer[key];
        }
    }
}

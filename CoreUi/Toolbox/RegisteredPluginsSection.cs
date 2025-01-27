using System.Collections.Generic;

namespace Toolbox
{
    public class RegisteredPluginsSection
    {
        public IEnumerable<ToolboxPlugin> RegisteredPlugins { get; set; }
    }
}

using Owin;

namespace NYSub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth();
        }
    }
}

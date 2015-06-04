using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BooksShoppingCart.Startup))]
namespace BooksShoppingCart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

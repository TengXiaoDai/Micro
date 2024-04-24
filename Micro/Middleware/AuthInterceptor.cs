using AspectCore.DynamicProxy;

namespace Micro.User.Middleware
{
    public class AuthInterceptor : IInterceptor
    {

        public bool AllowMultiple => true;
        bool IInterceptor.Inherited { get; set; }
        int IInterceptor.Order { get; set; }

        public async Task Invoke(AspectContext context, AspectDelegate next)
        {
            
        }
    }
}

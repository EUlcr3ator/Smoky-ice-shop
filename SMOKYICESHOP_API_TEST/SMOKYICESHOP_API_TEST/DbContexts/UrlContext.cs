namespace SMOKYICESHOP_API_TEST.DbContexts
{
    public class UrlContext
    {
        private IHttpContextAccessor m_httpContextAccessor;

        public UrlContext(IHttpContextAccessor contextAccessor)
        {
            m_httpContextAccessor = contextAccessor;
        }

        public HttpContext Current => m_httpContextAccessor.HttpContext;
        public string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";
        
        public string GetImageUrl(Guid imageId)
        {
            return $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}/api/Images/{imageId}";
        }
    }
}

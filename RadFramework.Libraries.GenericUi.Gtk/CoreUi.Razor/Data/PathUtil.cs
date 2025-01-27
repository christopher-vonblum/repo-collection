namespace CoreUi.Razor.Data
{
    public static class PathUtil
    {
        public static string NormalizePath(string path)
        {
            while (path.Contains("//"))
            {
                path = path.Replace("//", "/");
            }
            
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            if (path.EndsWith("/"))
            {
                path = path.TrimEnd('/');
            }
            
            return path.ToLowerInvariant();
        }
    }
}
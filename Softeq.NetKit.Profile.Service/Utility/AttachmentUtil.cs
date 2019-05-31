// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.NetKit.ProfileService.Utility
{
    public static class AttachemntsUtils
    {
        public static string GetFileExtentionByMimeType(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpeg": return "jpg";
                case "image/png": return "png";
                default: return null;
            }
        }

        public static string GetMimeTypeByExtention(string fileExtension)
        {
            switch (fileExtension)
            {
                case "jpg":
                case "jpeg": return "image/jpeg";
                case "png": return "image/png";
                default: return null;
            }
        }
    }
}

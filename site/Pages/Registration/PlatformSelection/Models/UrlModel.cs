namespace site.Pages.Registration.PlatformSelection.Models
{
    public class UrlModel
    {
        public string Url { get; set; }
        public string PlatformName { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(PlatformName);
    }
}
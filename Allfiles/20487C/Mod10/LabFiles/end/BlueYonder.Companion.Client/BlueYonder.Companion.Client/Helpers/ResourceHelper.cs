using Windows.ApplicationModel.Resources;

namespace BlueYonder.Companion.Client.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// The resource identifier of the ResourceMap that the new resource loader uses for unqualified resource references.
        /// It can then retrieve resources relative to those references.
        /// </summary>
        public static readonly ResourceLoader ResourceLoader = new ResourceLoader();
    }
}

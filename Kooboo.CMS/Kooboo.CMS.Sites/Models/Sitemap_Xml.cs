using System.Collections.Generic;

namespace Kooboo.CMS.Sites.Models
{
    public class Sitemap_Xml : FileResource
    {
        public Sitemap_Xml(Site site)
            : base(site, "sitemap.xml")
        { }

        public override IEnumerable<string> RelativePaths
        {
            get { return new string[0]; }
        }
    }
}
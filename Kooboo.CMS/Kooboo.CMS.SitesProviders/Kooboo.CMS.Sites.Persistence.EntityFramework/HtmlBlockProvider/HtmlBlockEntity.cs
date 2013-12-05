#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kooboo.CMS.Sites.Persistence.EntityFramework.HtmlBlockProvider
{
	class MyClass
	{
		public string SKu { get; set; }
	}

    [Table("Kooboo_CMS_Sites_HtmlBlocks")]
    public class HtmlBlockEntity
    {
        public HtmlBlockEntity() { }
        public HtmlBlockEntity(string siteName, string name)
        {
            this.SiteName = siteName;
            this.Name = name;

			IEnumerable<MyClass> classes = new List<MyClass>();


		var moreThan1 =	classes.GroupBy(c => c.SKu).Where(x => x.Count() > 1).ToList(); 

        }
        public HtmlBlockEntity(Kooboo.CMS.Sites.Models.HtmlBlock htmlBlock)
            : this(htmlBlock.Site.FullName, htmlBlock.Name)
        {
            this.Body = htmlBlock.Body;
        }
        [Key, Column(Order = 0)]
        public string SiteName
        {
            get;
            set;
        }
        [Key, Column(Order = 1)]
        public string Name
        {
            get;
            set;
        }
        public string Body { get; set; }

        public Kooboo.CMS.Sites.Models.HtmlBlock ToHtmlBlock()
        {
            Kooboo.CMS.Sites.Models.HtmlBlock htmlBlock = new Kooboo.CMS.Sites.Models.HtmlBlock(new Site(this.SiteName), this.Name);
            htmlBlock.Body = this.Body;
            ((IPersistable)htmlBlock).Init(htmlBlock);
            return htmlBlock;
        }
    }
}

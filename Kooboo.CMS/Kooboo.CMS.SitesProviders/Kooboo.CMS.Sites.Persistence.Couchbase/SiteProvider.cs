﻿using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Site>))]
    public class SiteProvider : Kooboo.CMS.Sites.Persistence.FileSystem.SiteProvider
    {
        #region .ctor
        SiteInitializer _initializer;
        public SiteProvider(IBaseDir baseDir, IElementRepositoryFactory elementRepositoryFactory, SiteInitializer initializer)
			: base(baseDir /*, membershipProvider*/, elementRepositoryFactory)
        {
            _initializer = initializer;
        }
        public override void Initialize(Site site)
        {
            _initializer.Initialize(site);
            base.Initialize(site);
        }
        #endregion
        Func<Site, string, Site> createModel = (Site site, string key) =>
        {
            return new Site(key);
        };
        #region Get/Update/Save/Delete
        public override void Add(Site item)
        {
            InsertOrUpdate(item, item);
            base.Add(item);
        }
        public override void Update(Site @new, Site old)
        {
            InsertOrUpdate(@new, old);
            base.Update(@new, old);
        }
        private void InsertOrUpdate(Site @new, Site old)
        {
            ((IPersistable)@new).OnSaving();            
            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.SiteDataType);
            ((IPersistable)@new).OnSaved();
        }
        public override Site Get(Site dummyObject)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.SiteDataType, dummyObject.FullName);

            var site = DataHelper.QueryByKey<Site>(dummyObject,bucketDocumentKey, createModel);

            if (site == null)
            {
                return base.Get(dummyObject);
            }
            return site;
        }
        public override void Remove(Site item)
        {
            DataHelper.DeleteItemByKey(item, ModelExtensions.GetBucketDocumentKey(ModelExtensions.SiteDataType, item.Name));
            base.Remove(item);
        }
        #endregion
    }
}

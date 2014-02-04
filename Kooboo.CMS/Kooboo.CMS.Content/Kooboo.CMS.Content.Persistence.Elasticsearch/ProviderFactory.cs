using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Elasticsearch
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProviderFactory), Order = 2)]
    public class ProviderFactory : Default.ProviderFactory
    {
        public override string Name
        {
            get { return "elasticsearch"; }
        }

    }

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRepositoryProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Common.Persistence.Non_Relational.IProvider<Repository>), Order = 2)]
    public class RepositoryProvider : Kooboo.CMS.Content.Persistence.Default.RepositoryProvider
    {
        #region .ctor
        public RepositoryProvider(IBaseDir baseDir)
            : base(baseDir) { }

        #endregion

        public override void Add(Models.Repository item)
        {
            base.Add(item);
            DatabaseHelper.InitializeDatabase(item);
        }
        public override void Remove(Models.Repository item)
        {
            try
            {
                DatabaseHelper.DisposeDatabase(item);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            base.Remove(item);
        }
        public override void Initialize(Models.Repository repository)
        {
            DatabaseHelper.InitializeDatabase(repository);
            base.Initialize(repository);
        }
        public override bool TestDbConnection()
        {

            var shareConnectionString = ElasticsearchSettings.Instance.ConnectionString;
            return MysqlHelper.TestConnection(shareConnectionString);
        }

    }
}

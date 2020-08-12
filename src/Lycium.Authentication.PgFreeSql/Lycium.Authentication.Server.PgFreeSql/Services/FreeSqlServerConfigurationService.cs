using Lycium.Authentication.Server.Model;
using System;
using System.Collections.Generic;

namespace Lycium.Authentication.Server.Services
{
    public class FreeSqlServerConfigurationService : IServerConfigurationService
    {

        private static Action<IFreeSql> InitAction;
        static FreeSqlServerConfigurationService()
        {
            InitAction = (freeSql) =>
            {
                freeSql.CodeFirst.ConfigEntity<LyciumConfig>
                (
                       item => item.Property(item => item.Id).IsIdentity(true)
                );
                InitAction = null;
            };
        }
        private readonly IFreeSql _freeSql;
        public FreeSqlServerConfigurationService(IFreeSql freesql)
        {
            _freeSql = freesql;
            InitAction?.Invoke(freesql);
        }

        public override bool AddConfig(LyciumConfig config)
        {
            return _freeSql.Insert(config).ExecuteAffrows() == 1;
        }

        public override long Count()
        {
            return _freeSql.Select<LyciumConfig>().Count();
        }

        public override LyciumConfig GetConfigById(long id)
        {
            return _freeSql.Select<LyciumConfig>().Where(item=>item.Id==id).First();
        }

        public override LyciumConfig GetConfigByName(string name)
        {
            return _freeSql.Select<LyciumConfig>().Where(item => item.Name == name).First();
        }

        public override IEnumerable<LyciumConfig> KeywordsQuery(int page, int size, string keyword)
        {
            return _freeSql.Select<LyciumConfig>().Where(item => item.Name.Contains(keyword)).Page(page,size).ToList();
        }

        public override IEnumerable<LyciumConfig> Query(int page, int size)
        {
            return _freeSql.Select<LyciumConfig>().Page(page, size).ToList();
        }

        public override bool UpdateConfig(LyciumConfig config)
        {

            var repository = _freeSql.GetRepository<LyciumConfig>();
            var item = repository.Where(item => item.Id == config.Id).First();
            return repository.Update(item) == 1;

        }
    }
}

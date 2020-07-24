using Lycium.Authentication;
using Lycium.Authentication.Common;
using System;
using System.Collections.Generic;

namespace LyciumFreesqlServer.Services
{
    public class FreeSqlServerHostService : IServerHostService
    {

        private static Action<IFreeSql> InitAction;
        static FreeSqlServerHostService()
        {
            InitAction = (freeSql) =>
            {
                freeSql.CodeFirst.ConfigEntity<LyciumHost>
                (
                       item => item.Property(item => item.Id).IsIdentity(true)
                );
                InitAction = null;
            };
        }

        private readonly IFreeSql _freeSql;
        public FreeSqlServerHostService(IFreeSql freesql, IContextInfoService infoService) : base(infoService)
        {
            _freeSql = freesql;
            InitAction?.Invoke(freesql);
        }

        public override LyciumHost GetHostBySecretKey(string secretKey)
        {
            return _freeSql.Select<LyciumHost>().Where(item => item.SecretKey == secretKey).First();
        }

        public override IEnumerable<LyciumHost> Query(int page, int size)
        {
            return _freeSql.Select<LyciumHost>().Page(page, size).ToList();
        }

        public override IEnumerable<LyciumHost> KeywordQuery(int page, int size, string keyword)
        {
            return _freeSql.Select<LyciumHost>().Where(item=>item.HostName.Contains(keyword)).Page(page, size).ToList();
        }

        public override long Count()
        {
            return _freeSql.Select<LyciumHost>().Count();
        }

        public override bool AddHost(LyciumHost host)
        {
            return _freeSql.Insert(host).ExecuteAffrows() == 1;
        }

        public override LyciumHost SecretKeyQuery(string hostName)
        {
            return _freeSql.Select<LyciumHost>().Where(item => item.HostName == hostName).First();
        }

        public override bool UpdateHost(LyciumHost host)
        {
            var repository = _freeSql.GetRepository<LyciumHost>();
            var result = repository.Where(item => item.Id == host.Id).First();
            repository.Attach(result);
            return repository.Update(host) == 1;
        }

        public override LyciumHost GetHostById(long id)
        {
            return _freeSql.Select<LyciumHost>().Where(item => item.Id == id).First();
        }
    }
}

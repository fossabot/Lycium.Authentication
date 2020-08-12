using System.Collections.Generic;
using System.Linq;

namespace Lycium.Authentication.Server
{
    /// <summary>
    /// 增量服务
    /// </summary>
    public abstract class ITokenIncrementService
    {
        public abstract void AddIncrement(TokenIncrement tokenIncrement);
        public abstract IEnumerable<TokenIncrement> GetTokenIncrements(long cid);
        public abstract void RemoveIncrement(long cid,long createTime);
        

        /// <summary>
        /// 压缩增量
        /// </summary>
        /// <param name="tokenIncrements"></param>
        /// <returns></returns>
        public virtual IEnumerable<TokenIncrement> GetCompressionIncrements(long cid)
        {

            var tokenIncrements = GetTokenIncrements(cid);
            var mapping = new Dictionary<long,Dictionary<long, List<TokenIncrement>>>();
            var result = new List<TokenIncrement>();
            foreach (var item in tokenIncrements)
            {
                if (!mapping.ContainsKey(item.Gid))
                {
                    mapping[item.Gid] = new Dictionary<long, List<TokenIncrement>>();
                    
                }
                if (!mapping[item.Gid].ContainsKey(item.Uid))
                {
                    mapping[item.Gid][item.Uid] = new List<TokenIncrement>();
                }
                mapping[item.Gid][item.Uid].Add(item);
            }
            foreach (var item in mapping)
            {
                foreach (var list in item.Value)
                {
                    result.Add(CompressionIncrements(list.Value));
                }
            }
            return result;

        }


        /// <summary>
        /// 将增量列表压缩成一个结果
        /// </summary>
        /// <param name="tokenIncrements"></param>
        /// <returns></returns>
        public virtual TokenIncrement CompressionIncrements(IEnumerable<TokenIncrement> tokenIncrements)
        {

            var list = tokenIncrements.OrderBy(item => item.CreateTime);
            var increment = list.First();
            var operatorType = increment.OperatorType;
            var isCreated = operatorType == 'c';
            var isExist = operatorType != 'c';

            //遍历增量操作列表
            foreach (var incrementInfo in list)
            {
                if (increment.OperatorType != incrementInfo.OperatorType)
                {
                    if (incrementInfo.OperatorType == 'd')
                    {
                        //如果中途创建了实体，现在又删除
                        if (isCreated)
                        {
                            //相当于什么都没做
                            isCreated = false;
                            increment.OperatorType = 'n';
                        }
                        else
                        {
                            //中途操作不涉及到新建，则结果为删除
                            increment.OperatorType = 'd';
                        }
                        
                    }
                    else if (incrementInfo.OperatorType == 'c')
                    {
                        //如果途中创建了一个实体
                        isCreated = true;
                        increment.OperatorType = 'c';
                    }
                }
                increment.CreateTime = incrementInfo.CreateTime;
            }

            //如果之前已经存在
            if (increment.OperatorType == 'c' && isExist)
            {
                increment.OperatorType = 'u';
            }
            return increment;
        }
    }
}

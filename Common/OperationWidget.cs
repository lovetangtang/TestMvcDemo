using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class OperationWidget
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// 操作失败
        /// </summary>
        public const int Failed = 100001;

        /// <summary>
        /// 系统异常
        /// </summary>
        public const int Exception = 100002;

        /// <summary>
        /// 没有权限
        /// </summary>
        public const int NoPermission = 100003;

        /// <summary>
        /// 参数错误
        /// </summary>
        public const int ArgumentError = 100004;

        /// <summary>
        /// 没有记录
        /// </summary>
        public const int NoRecord = 100005;

        /// <summary>
        /// 记录重复
        /// </summary>
        public const int IsRepeated = 100006;

        /// <summary>
        /// 登录过期
        /// </summary>
        public const int LoginExpired = 100010;

        /// <summary>
        /// 验证码错误
        /// </summary>
        public const int CodeError = 100020;
    }
}
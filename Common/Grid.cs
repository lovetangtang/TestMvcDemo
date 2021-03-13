namespace Common
{
    public class Grid
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortFiled { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string SortDirection { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreTestProject.Models
{
    /// <summary>
    /// Result object
    /// </summary>
    /// <typeparam name="T">result's data type</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// result code
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public Result() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data">result's data</param>
        /// <param name="resultCode">result's result code</param>
        /// <param name="message">result's message</param>
        public Result(T data, int resultCode, string message) : this()
        {
            Data = data;
            ResultCode = resultCode;
            Message = message;
        }
    }
}

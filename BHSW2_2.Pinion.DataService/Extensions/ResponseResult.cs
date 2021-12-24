namespace BHSW2_2.Pinion.DataService.Extensions
{
    public class ResponseResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseResult()
        {

        }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// ResponseResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : ResponseResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseResult()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="result"></param>
        public ResponseResult(T result)
        {
            Result = result;
        }

        /// <summary>
        /// Result
        /// </summary>
        public T Result { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.Results
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static OperationResult<T> Fail(string error) => new() { Success = false, ErrorMessage = error };
    }

    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Fail(string error) => new() { Success = false, ErrorMessage = error };
    }
}

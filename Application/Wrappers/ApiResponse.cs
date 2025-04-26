using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
	public class ApiResponse<T>
	{
        public bool Succecced { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
        public ApiResponse()
        {
            
        }
		public ApiResponse(T data)
		{
			Succecced = true;
			Data = data;
		}
		//fa
		//success
		public ApiResponse(T data, string message)
        {
			Succecced = true;
			Message = message;
            Data = data;
		}
        //failed
        public ApiResponse(string message)
        {
			Succecced=false;
            Message = message;

		}
    }
}

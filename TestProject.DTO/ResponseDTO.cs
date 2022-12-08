using System.Net;
using System.Text.Json.Serialization;

namespace TestProject.DTO
{
	public class ResponseDTO<T>
	{
		public bool IsSuccess { get; set; }
		public T Data { get; set; }
		public string ErrorCode { get; set; }
		[JsonIgnore]
		public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
	}
}
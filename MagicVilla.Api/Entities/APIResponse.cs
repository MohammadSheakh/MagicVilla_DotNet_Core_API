using System.Net;

namespace MagicVilla.Api.Entities
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode{ get; set; }

        public bool IsSuccessfull { get; set; } = true; // by default true set kore dilam .. 

        public List<string> ErrorMessages { get; set; }

        public object Result {  get; set; }

    }
}

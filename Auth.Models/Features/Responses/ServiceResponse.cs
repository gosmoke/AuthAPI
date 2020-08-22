using Auth.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GotMilk.Models
{
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public int ResponseCode { get; set; } = StatusCodes.Status200OK;
        public T Content { get; set; }

        public List<ServiceMessage> Messages { get; set; } = new List<ServiceMessage>();
    }
}

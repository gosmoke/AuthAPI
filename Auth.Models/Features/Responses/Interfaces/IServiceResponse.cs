using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Models
{
    public interface IServiceResponse<T>
    {
        T Content { get; set; }
        int ResponseCode { get; set; }
    }
}

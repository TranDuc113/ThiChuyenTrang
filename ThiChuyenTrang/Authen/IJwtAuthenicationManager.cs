using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoJwt
{
    public interface IJwtAuthenicationManager
    {
        AuThenticationRespone Autheniaction(string TaiKhoan, String MatKhau);
    }
}

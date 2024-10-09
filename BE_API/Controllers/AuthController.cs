using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Xác thực Google
        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Nhận phản hồi từ Google
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded) return BadRequest();

            // Xử lý thông tin người dùng tại đây (ví dụ: tạo hoặc cập nhật người dùng trong cơ sở dữ liệu)
            var claims = result.Principal.Claims;
            var userInfo = new
            {
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };

            return Ok(userInfo); // Trả về thông tin người dùng
        }
    }
}

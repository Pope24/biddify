using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using DataAccess;
using Domain.Models;
using System.Diagnostics;

namespace Biddify.Pages.Profiles.Partial
{
    public class _SecurityManagementModel : PageModel
    {
        private readonly UserManager<UserEntity> _userManager;

        public _SecurityManagementModel(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        private static string? _sentOtp;
        private static string? _otpEmail;

        public async Task<JsonResult> OnPostSendOtpAsync()
        {
            var body = await new StreamReader(Request.Body).ReadToEndAsync();
            var json = JsonDocument.Parse(body);
            var email = json.RootElement.GetProperty("email").GetString();

            if (string.IsNullOrWhiteSpace(email))
                return new JsonResult(new { success = false, message = "Email không được để trống." });

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new JsonResult(new { success = false, message = "Không tìm thấy người dùng." });

            var otp = new Random().Next(100000, 999999).ToString();
            _sentOtp = otp;
            _otpEmail = email;

            var sender = new EmailOtpSender(
                host: "smtp.gmail.com",
                port: 587,
                user: "hung1961dn@gmail.com",
                pass: "ylnh qtxw ikmy zmhk" // App password
            );
            Console.WriteLine(otp);
            Debug.WriteLine(otp);
            var success = await sender.SendOtpAsync(email, otp);
            return new JsonResult(new { success });
        }

        public JsonResult OnPostValidateOtp()
        {
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEnd();
            var json = JsonDocument.Parse(body);
            var otp = json.RootElement.GetProperty("otp").GetString();

            bool valid = otp == _sentOtp;
            return new JsonResult(new { valid });
        }

        public async Task<JsonResult> OnPostChangePasswordAsync()
        {
            var body = await new StreamReader(Request.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<PasswordRequest>(body);

            if (data == null || data.Otp != _sentOtp || data.Email != _otpEmail)
                return new JsonResult(new { success = false, message = "OTP không hợp lệ hoặc email không khớp." });

            var user = await _userManager.FindByEmailAsync(data.Email);
            if (user == null)
                return new JsonResult(new { success = false, message = "Không tìm thấy người dùng." });

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
                return new JsonResult(new { success = false, message = "Không thể xoá mật khẩu cũ." });

            var addResult = await _userManager.AddPasswordAsync(user, data.NewPassword);
            if (!addResult.Succeeded)
                return new JsonResult(new { success = false, message = "Không thể đặt mật khẩu mới." });

            _sentOtp = null;
            _otpEmail = null;

            return new JsonResult(new { success = true });
        }

        public class PasswordRequest
        {
            public string Email { get; set; }
            public string Otp { get; set; }
            public string NewPassword { get; set; }
        }
    }
}

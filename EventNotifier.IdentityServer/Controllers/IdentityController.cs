using EventNotifier.IdentityServer.Data.Models;
using EventNotifier.IdentityServer.Helpers;
using EventNotifier.IdentityServer.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace EventNotifier.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly AppSettings appSettings;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityController(UserManager<User> userManager, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(CreateUserRequestModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            var temporaryPassword = IdentityHelper.GenerateTemporaryPassword();
            var result = await userManager.CreateAsync(user, temporaryPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken", code);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = $"{appSettings.ClientUrl}/{appSettings.ResetPasswordPath}/{user.Id}/{code}";
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("natalia.gerus892@gmail.com", "kzdojfoidoznhooi"),
                    EnableSsl = true,
                };


                var mailMessage = new MailMessage
                {
                    From = new MailAddress("natalia.gerus892@gmail.com"),
                    Subject = "Астеризм акаунт створено",
                    Body = $"Ви отримали цей електронний лист, оскільки для вас було створено акаунт у застосунку Event Notifier.<br/><br/>" +
                    $"Будь ласка, налаштуйте свій пароль <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>перейшовши за посиланням</a>.<br/><br/>" +
                    $"Якщо у вас виникли труднощі, зверніться до служби підтримки  Event Notifier за адресою <a href='mailto:support@eventnotifier.com'>" +
                    $"support@eventnotifier.com</a><br/><br/><span style='font-size:13px;font-style:italic'>" +
                    $"Це повідомлення, включно з прикріпленими до нього файлами, може містити конфіденційну інформацію, призначену лише для використання АДРЕСАТАМИ, зазначеними вище." +
                    $" Якщо ви не є одержувачем, ми повідомляємо, що будь-яке розповсюдження або копіювання інформації, що міститься в цьому повідомленні," +
                    $" або вчинення будь-яких дій на основі цієї інформації суворо заборонено. Якщо ви отримали це повідомлення помилково, " +
                    $"будь ласка, негайно повідомте відправника, надіславши відповідь електронною поштою, і знищіть усі копії вихідного повідомлення.</span>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(user.Email);
                smtpClient.Send(mailMessage);

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<ActionResult> Edit(EditUserRequestModel model)
        {
            User user = await userManager.FindByIdAsync(model.Id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.Email;
            user.Email = model.Email;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        public async Task<ActionResult> Delete([FromRoute] string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<User>> GetUsers()
        {
            return await Task.Run(() =>
            {
                return userManager.Users.ToList();
            });
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult> GetCurrentUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var result = await userManager.FindByIdAsync(userId);

            return Ok(result);
        }


        [HttpGet]
        [Route("role")]
        public async Task<ActionResult> GetCurrentRole()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var result = await userManager.FindByIdAsync(userId);
            var a = userManager.GetRolesAsync(result).Result.FirstOrDefault();

            if (a == null)
            {
                return Ok(null);
            }

            return Ok(new { role = a });
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Користувача з вказаною електронною поштою не існує");
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                return Unauthorized("Неправильний пароль");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = encryptedToken
            });
        }

        [HttpPost]
        [Route("email-confirmation")]
        public async Task<ActionResult<string>> ConfirmEmail(PasswordResetSendEmailModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized("Користувача з вказаною електронною поштою не існує");
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.SetAuthenticationTokenAsync(user, "ResetPassword", "ResetPasswordToken", code);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = $"{appSettings.ClientUrl}/{appSettings.ResetPasswordPath}/{user.Id}/{code}";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("natalia.gerus892@gmail.com", "kzdojfoidoznhooi"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("natalia.gerus892@gmail.com"),
                Subject = "Зміна паролю",
                Body = $"Ви отримали цей електронний лист, оскільки ваш пароль до Event Notifier було скинуто.<br/><br/>" +
                $"Будь ласка, скиньте свій пароль <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>перейшовши за посиланням</a>.<br/><br/>" +
                $"Якщо у вас виникли труднощі, зверніться до служби підтримки EventNotifier за адресою <a href='mailto:support@eventnotifier.com'>" +
                $"support@eventnotifier.com</a><br/><br/><span style='font-size:13px;font-style:italic'>" +
                $"Це повідомлення, включно з прикріпленими до нього файлами, може містити конфіденційну інформацію, призначену лише для використання АДРЕСАТАМИ, зазначеними вище." +
                $" Якщо ви не є одержувачем, ми повідомляємо, що будь-яке розповсюдження або копіювання інформації, що міститься в цьому повідомленні," +
                $" або вчинення будь-яких дій на основі цієї інформації суворо заборонено. Якщо ви отримали це повідомлення помилково, " +
                $"будь ласка, негайно повідомте відправника, надіславши відповідь електронною поштою, і знищіть усі копії вихідного повідомлення.</span>",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(user.Email);
            smtpClient.Send(mailMessage);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password/{userId}/{resetToken}")]
        public async Task<ActionResult<string>> ResetPassword([FromRoute] string userId, [FromRoute] string resetToken, PasswordResetModel model)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            resetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetToken));
            var result = await userManager.ResetPasswordAsync(user, resetToken, model.Password);

            return Ok(result);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}

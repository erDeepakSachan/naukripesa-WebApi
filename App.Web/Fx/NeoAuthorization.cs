using System.Buffers;
using App.Web.Models.Vm;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Entity;
using App.Service;
using App.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace App.Web.Fx
{
    public class NeoAuthorization
    {
        private readonly UserService userService;
        private UserSessionService userSessionService;
        public WebHelper.SessionHelper sessionHelper { get; set; }
        public CryptographyHelper cryptographyHelper;
        private List<string> publicPages;
        private readonly WebHelper webHelper;

        public NeoAuthorization(WebHelper webHelper
            , WebHelper.SessionHelper sessionHelper
            , UserService userService
            , UserSessionService userSessionService
            , IConfiguration configuration)
        {
            this.webHelper = webHelper;
            this.userService = userService;
            this.sessionHelper = sessionHelper;
            this.userSessionService = userSessionService;
            this.cryptographyHelper = new CryptographyHelper();
            this.InitPublicPageList();
        }

        public List<UserGroupPermission> GetWebPagesByGroupId(int groupId)
        {
            return this.sessionHelper.NeoContext.WebPagePermissions(this).Where(P => P.UserGroupId == groupId).ToList();
        }

        public bool VerifyUser(ResetPasswordRequest req)
        {
            var existingUser = userService.GetUserByEmailAndMobile(req.UserName, req.MobileNo);
            if (existingUser != null)
            {
                return true;
            }

            return false;
        }

        public NeoAuthToken SignMeIn(LoginRequest req)
        {
            cryptographyHelper.EncodingType = CryptographyHelper.EncodingBaseTypes.Hex;

            var passphrase = webHelper.NeoPassPhrase;

            var password = cryptographyHelper.Encrypt(req.Password, passphrase);
            var user = userService.Authenticate(req.UserName, password);
            if (user != null)
            {
                user.RecentLogin = DateTime.UtcNow;
                userService.Update(user).Wait();


                var arrUserSession = userSessionService.GetByUser(user.UserId);
                var usersession = new UserSession();
                if (arrUserSession.Count == 0)
                {
                    // Create New Session
                    usersession = userSessionService.CreateNewUserSession(user);
                    if (usersession.UserSessionId != 0)
                    {
                        // Log the Login Activity
                        userSessionService.LogAccessActivity(user, usersession, "Login");
                        // Response.Redirect("Default.aspx?sid=" & usersession.SessionGuid.ToString)
                        return new NeoAuthToken()
                        {
                            AllowedPages = GetWebPagesByGroupId(user.UserGroupId).Select(P => P.WebpageId).ToList(),
                            DisplayName = user.Name,
                            GroupID = user.UserGroupId,
                            SID = usersession.SessionGuid,
                            UserID = user.UserId
                        };
                    }
                }
                else
                {
                    usersession = arrUserSession[0] as UserSession;
                    if (DateAndTime.DateDiff(DateInterval.Minute, usersession.StartTime, DateTime.UtcNow) +
                        (DateAndTime.DateDiff(DateInterval.Hour, usersession.StartTime, DateTime.UtcNow) * 60) >
                        usersession.ExpirationTimeFrame)
                    {
                        // Close the open session since it has expired. This can happen if the user did not log out and close the session
                        usersession.IsActive = false;
                        usersession.EndTime = DateTime.UtcNow;
                        userSessionService.Update(usersession).Wait();

                        // Create New Session Since the old one has exceeded the expiration time frame set
                        usersession = userSessionService.CreateNewUserSession(user);
                        // Log the Login Activity
                        userSessionService.LogAccessActivity(user, usersession, "Login");
                        // Response.Redirect("Default.aspx?sid=" & usersession.SessionGuid.ToString)
                        return new NeoAuthToken()
                        {
                            AllowedPages = GetWebPagesByGroupId(user.UserGroupId).Select(P => P.WebpageId).ToList(),
                            DisplayName = user.Name,
                            GroupID = user.UserGroupId,
                            SID = usersession.SessionGuid,
                            UserID = user.UserId
                        };
                    }
                }

                // Response.Redirect("Default.aspx?sid=" & usersession.SessionGuid.ToString)
                return new NeoAuthToken()
                {
                    AllowedPages = GetWebPagesByGroupId(user.UserGroupId).Select(P => P.WebpageId).ToList(),
                    DisplayName = user.Name,
                    GroupID = user.UserGroupId,
                    SID = usersession.SessionGuid,
                    UserID = user.UserId
                };
            }
            else
            {
                // User did not authenticate
                throw new NeoAuthException("Login Failed. Unable to authenticate.");
            }
        }

        public string CreateAuthToken(NeoAuthToken token)
        {
            return GenerateJwt(token);
        }

        public string CreateAuthTokenOld(NeoAuthToken token)
        {
            cryptographyHelper.EncodingType = CryptographyHelper.EncodingBaseTypes.Base64;
            var tokenJson = JsonConvert.SerializeObject(token);
            var encToken = cryptographyHelper.Encrypt(tokenJson, webHelper.NeoPassPhrase);
            return encToken;
        }

        public NeoAuthToken ParseAuthToken(string encToken)
        {
            if (String.IsNullOrWhiteSpace(encToken))
            {
                return null;
            }

            cryptographyHelper.EncodingType = CryptographyHelper.EncodingBaseTypes.Base64;
            var tokenJson = cryptographyHelper.Decrypt(encToken, webHelper.NeoPassPhrase);
            var token = JsonConvert.DeserializeObject<NeoAuthToken>(tokenJson);
            return token;
        }

        public bool IsPageAccessible()
        {
            var context = this.sessionHelper.NeoContext;
            var webPages = context.WebPages(this);
            if (webPages == null)
            {
                webPages = new Dictionary<string, int>();
            }

            var authToken = ParseAuthToken(context.AuthTokenFromRqeuest);
            this.sessionHelper.LoggedUser = authToken;
            var requestedPage = webPages.ContainsKey(context.ControllerName) ? webPages[context.ControllerName] : -1;
            if (this.publicPages.Contains(context.ControllerName) || (authToken != null &&
                                                                      authToken.AllowedPages.Count() > 0 &&
                                                                      authToken.AllowedPages.Contains(requestedPage)))
            {
                return true;
            }

            return false;
        }

        private void InitPublicPageList()
        {
            this.publicPages = new List<string>();
            this.publicPages.Add("home");
            this.publicPages.Add("leftmenu");
        }

        public string GenerateJwt(NeoAuthToken authToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webHelper.AuthKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var oldTokenData = CreateAuthTokenOld(authToken);
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, authToken.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, authToken.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, authToken.DisplayName),
                new Claim(NeoAuthToken.AuthTokenKey, oldTokenData),
            };

            var token = new JwtSecurityToken(
                issuer: webHelper.AuthDomain,
                audience: webHelper.AuthDomain,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public static Func<MessageReceivedContext, Task> OnMessageReceived => context =>
        {
            var neoCtx = context.HttpContext.RequestServices.GetService<NeoContext>();
            var token = NeoAuthToken.GetAuthTokenFromHeader(neoCtx);
            if (String.IsNullOrWhiteSpace(token))
            {
                token = NeoAuthToken.GetAuthCookie(neoCtx);
            }

            context.Token = token;
            return Task.CompletedTask;
        };

        public static void JwtOptions(JwtBearerOptions options, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(WebHelper.APP_SETTINGS_SECTION).Get<WebHelper>();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.AuthDomain,
                ValidAudience = jwtSettings.AuthDomain,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AuthKey))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = NeoAuthorization.OnMessageReceived,
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    await context.Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes("Please login."), CancellationToken.None);
                },
                OnForbidden = context => { return Task.FromResult(0); },
                OnAuthenticationFailed = context => { return Task.FromResult(0); },
                OnTokenValidated = context => { return Task.FromResult(0); },
            };
        }
    }

    public class NeoException : Exception
    {
        public NeoException(string message) : base(message)
        {
        }
    }

    public class NeoAuthException : NeoException
    {
        public NeoAuthException(string message) : base(message)
        {
        }
    }
}
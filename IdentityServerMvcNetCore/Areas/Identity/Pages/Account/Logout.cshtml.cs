using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace IdentityServerMvcNetCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger,
                          IIdentityServerInteractionService interaction)
        {
            _signInManager = signInManager;
            _logger = logger;
            _interaction = interaction;
        }

        public async Task<IActionResult> OnGet(string logoutId)
        {
            // check for user
            var user = User?.Identity.IsAuthenticated;

            if (string.IsNullOrEmpty(logoutId) )
            {
                return Page();
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            string url = context.PostLogoutRedirectUri;

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Redirect(url);
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }

        //private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        //{
        //    var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        //    if (User?.Identity.IsAuthenticated != true)
        //    {
        //        // if the user is not authenticated, then just show logged out page
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }

        //    var context = await _interaction.GetLogoutContextAsync(logoutId);
        //    if (context?.ShowSignoutPrompt == false)
        //    {
        //        // it's safe to automatically sign-out
        //        vm.ShowLogoutPrompt = false;
        //        return vm;
        //    }

        //    // show the logout prompt. this prevents attacks where the user
        //    // is automatically signed out by another malicious web page.
        //    return vm;
        //}
    }






    //class LogoutViewModel : LogoutInputModel
    //{
    //    public bool ShowLogoutPrompt { get; set; } = true;
    //}

    //class LogoutInputModel
    //{
    //    public string LogoutId { get; set; }
    //}

    //class AccountOptions
    //{
    //    public static bool AllowLocalLogin = true;
    //    public static bool AllowRememberLogin = true;
    //    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    //    public static bool ShowLogoutPrompt = true;
    //    public static bool AutomaticRedirectAfterSignOut = false;

    //    // specify the Windows authentication scheme being used
    //    public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
    //    // if user uses windows auth, should we load the groups from windows
    //    public static bool IncludeWindowsGroups = false;

    //    public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    //}
}
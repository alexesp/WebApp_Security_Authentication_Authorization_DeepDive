using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security_Authentication_Authorization_DeepDive.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Verify the credential
            if (Credential.Name == "admin" && Credential.Password == "/123-*")
            {
                //Creatin the security context
                var claims = new List<Claim>
                { new Claim (ClaimTypes.Name, "admin"),
                  new Claim (ClaimTypes.Email, "admin@website.com"),
                  new Claim ("Department", "HR"),
                  new Claim ("Admin", "true"),
                  new Claim ("Manager", "true"),
                  new Claim("EmploymentDate", "2024-12-01")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth"); 
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);


               await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}

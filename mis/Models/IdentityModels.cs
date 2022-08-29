using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mis.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
     
        public string fname { get; set; }
       
        public string lname { get; set; }
    }




    public class Employee
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "User Id")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "User Id should be Numeric.")]
        public string User_id { get; set; }
        [Display(Name = "Name")]
        [Required]
        [RegularExpression(@"^[A-Z]+[A-Za-z]{1,}$", ErrorMessage = "Name First Letter should be capital (e.g. Abc).")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Address")]
        [RegularExpression(@"^[#.0-9a-zA-Z\s,-]+[a-zA-Z]+$", ErrorMessage = "Address filled will be ,number, [,-,#] and space.")]
        public string Address { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Mobile Number should be Numeric.")]
        public string Mobile_number { get; set; }
        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        public string Email_address { get; set; }
        [Display(Name = "Aadhaar Number")]
        [Required]
        [MinLength(12)]
        [MaxLength (12)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Aadhaar Number will be numeric are allowed only.")]
        public string Aadhaar_number { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        [Display(Name = "Pan Number")]
        [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Pan Number will be Capital Alpha Numeric are allowed only.")]
        public string Pan_number { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        [ValidateBirthDate]
        public string Birth_date { get; set; }
        [Required]
        [Display(Name = "Job title")]
        [RegularExpression(@"^[A-Z]+[A-Za-z]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Job title (e.g. Abc).")]
        public string Job_title { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+[A-Za-z]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Department (e.g. Abc).")]
        public string Department { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]+[A-Za-z]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Supervisor (e.g. Abc).")]
        public string Supervisor { get; set; }
        [Required]
        [Display(Name = "Work Location")]
        [RegularExpression(@"^[A-Z]+[A-Za-z]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Work Location (e.g. Abc).")]
        public string Work_location { get; set; }
        public string Joining_date { get; set; }
        public string Resignation_date { get; set; }
    }

    public class Pc
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        [RegularExpression(@"^[A-Z]+[A-Za-z\s]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Company Name (e.g. Abc).")]
        public string Company_name { get; set; }
        [Required]
        [Display(Name = "Model No.")]
        [RegularExpression(@"^[A-Za-z]{1,}[0-9]{1,}$", ErrorMessage = "Enter Alpha Numeric are allowed only in Model No.")]
        public string Model_no { get; set; }
        [Required]
        [Display(Name = "Product Id")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Numeric are allowed only in Product Id.")]
        public string Product_id { get; set; }
    }

    public class Lp
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        [RegularExpression(@"^[A-Z]+[A-Za-z\s]{1,}$", ErrorMessage = "Enter Alphabet are allowed only in Company Name (e.g. Abc).")]
        public string Company_name { get; set; }
        [Required]
        [Display(Name = "Model No.")]
        [RegularExpression(@"^[A-Za-z]{1,}[0-9]{1,}$", ErrorMessage = "Enter Alpha Numeric are allowed only in Model No.")]
        public string Model_no { get; set; }
        [Required]
        [Display(Name = "Product Id")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Numeric are allowed only in Product Id.")]
        public string Product_id { get; set; }
    }

    public class allotment_detail
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Employee Name")]
        public string Employee_name { get; set; }
        [Required]
        [Display(Name = "Product Alloted")]
        public string Product_alloted { get; set; }
        [Required]
        [Display(Name = "Product Id")]
        public string Product_id{ get; set; }
        [Required]
        [Display(Name = "Accessories")]
        public string Accessories { get; set; }
    }

    public class Mymodel
    {
        public Employee Employee{ get; set; }
        public List<Employee> EmployeeList { get; set; }
        public Pc pc { get; set; }
        public List<Pc> PcList { get; set; }
        public Lp lp { get; set; }
        public List<Lp> LpList { get; set; }
        public allotment_detail ad { get; set; }
        public List<allotment_detail>  adList { get; set; }
        public PagedList.IPagedList<Employee> employees { get; set; }
        public PagedList.IPagedList<Pc> Pcpaged { get; set; }
        public PagedList.IPagedList<Lp> Lppaged { get; set; }
        public PagedList.IPagedList<allotment_detail> allotPaged { get; set; }
    }



    public class ValidateBirthDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime _birthDate = Convert.ToDateTime(value);

            var today = DateTime.Today;

            var date = _birthDate.AddYears(18);

            if (_birthDate > today || today < date)

                return new ValidationResult("Enter valid Birth Date (Age Limit: 18 year)");
            else

                return ValidationResult.Success;
        }
    }








    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<mis.Models.Employee> Employee { get; set; }
        public DbSet<mis.Models.Pc> Pc { get; set; }
        public DbSet<mis.Models.Lp> Lp { get; set; }
        public DbSet<mis.Models.allotment_detail> allotment_detail { get; set; }
    }
}
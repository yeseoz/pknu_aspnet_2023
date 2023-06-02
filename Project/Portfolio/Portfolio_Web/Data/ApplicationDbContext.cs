using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio_Web.Models;

namespace Portfolio_Web.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // 포트폴리오를 DB로 관리하기 위한 모델
        public DbSet<PortfolioModel> Portfolios { get; set; }
    }
}

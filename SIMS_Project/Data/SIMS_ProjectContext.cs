using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMS_Project.Models;

namespace SIMS_Project.Data
{
    public class SIMS_ProjectContext : DbContext
    {
        public SIMS_ProjectContext (DbContextOptions<SIMS_ProjectContext> options)
            : base(options)
        {
        }

        public DbSet<SIMS_Project.Models.Student> Student { get; set; } = default!;
    }
}

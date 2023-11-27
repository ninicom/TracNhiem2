using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TracNhiem2.Models;

namespace TracNhiem2.Data
{
    public class TracNhiem2Context : IdentityDbContext<IdentityUser>
    {
        public TracNhiem2Context (DbContextOptions<TracNhiem2Context> options)
            : base(options)
        {
        }

        public DbSet<Player> Player { get; set; }
        public DbSet<Question>? Question { get; set; }

        public DbSet<QuestionGroup>? QuestionGroup { get; set; }

        public DbSet<TracNhiem2.Models.RoomQuiz>? RoomQuiz { get; set; }

        public DbSet<TracNhiem2.Models.RoomQuizView>? RoomQuizView { get; set; }
        public DbSet<TracNhiem2.Models.SingleResult>? SingleResult { get; set; }
    }
}

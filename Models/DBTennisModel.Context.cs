﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PraktikaWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBTennisContext : DbContext
    {
        public DBTennisContext()
            : base("name=DBTennisContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Court> Court { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<Match_Tennis_Player> Match_Tennis_Player { get; set; }
        public virtual DbSet<Matches> Matches { get; set; }
        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Person_Profile> Person_Profile { get; set; }
        public virtual DbSet<Shot> Shot { get; set; }
        public virtual DbSet<Start_Table> Start_Table { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TennisPlayers> TennisPlayers { get; set; }
        public virtual DbSet<Tournament> Tournament { get; set; }
        public virtual DbSet<Match_Progress> Match_Progress { get; set; }
    }
}

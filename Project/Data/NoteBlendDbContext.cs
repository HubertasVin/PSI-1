﻿using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data;

public class NoteBlendDbContext : DbContext
{
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<User> Users { get; set; }
    
    public NoteBlendDbContext(DbContextOptions<NoteBlendDbContext> options) : base(options)
    {
        
    }
}
﻿using System.Linq;
using core.Models;
using Microsoft.EntityFrameworkCore;

namespace core
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        IQueryable<Streamer> IApplicationContext.Streamers => Streamers;
        IQueryable<StreamerPlatform> IApplicationContext.StreamerPlatforms => StreamerPlatforms;

        public DbSet<Streamer> Streamers { get; set; }
        public DbSet<StreamerPlatform> StreamerPlatforms { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            Remove(entity);
        }
    }
}
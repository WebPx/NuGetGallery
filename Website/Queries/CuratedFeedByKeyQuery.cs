﻿using System.Data.Entity;
using System.Linq;

namespace NuGetGallery
{
    public interface ICuratedFeedByKeyQuery
    {
        CuratedFeed Execute(
            int key,
            bool includePackages);
    }

    public class CuratedFeedByKeyQuery : ICuratedFeedByKeyQuery
    {
        private readonly IEntitiesContext _entities;

        public CuratedFeedByKeyQuery(IEntitiesContext entities)
        {
            _entities = entities;
        }

        public CuratedFeed Execute(
            int key,
            bool includePackages)
        {
            IQueryable<CuratedFeed>  qry = _entities.CuratedFeeds;

            if (includePackages)
            {
                qry = qry
                    .Include(cf => cf.Packages)
                    .Include(cf => cf.Packages.Select(cp => cp.PackageRegistration));
            }

            return qry
                .SingleOrDefault(cf => cf.Key == key);
        }
    }
}
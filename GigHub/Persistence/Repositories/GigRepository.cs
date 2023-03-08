using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistence.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetGigsUserIsAttending(string userId)
        {
            return _context.Attendances
                .Where(a => a.AttendeeId == userId &&
                            a.Gig.DateTime > DateTime.Now &&
                            !a.Gig.IsCancelled)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();
        }

        public Gig GetGig(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetUpcomingGigsByArtist(string artistId)
        {
            return _context.Gigs
                            .Where(g => g.ArtistId == artistId &&
                                        g.DateTime > DateTime.Now &&
                                        !g.IsCancelled)
                            .Include(g => g.Genre)
                            .ToList();
        }

        public IEnumerable<Gig> GetUpcomingGigs(string query = null)
        {
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now &&
                            !g.IsCancelled);

            if (!string.IsNullOrWhiteSpace(query))
                upcomingGigs = upcomingGigs.Where(ug => ug.Artist.Name.Contains(query) ||
                                                        ug.Genre.Name.Contains(query) ||
                                                        ug.Venue.Contains(query));

            return upcomingGigs.ToList();
        }

        public void Add(Gig gig)
        {
            _context.Gigs.Add(gig);
        }
    }
}
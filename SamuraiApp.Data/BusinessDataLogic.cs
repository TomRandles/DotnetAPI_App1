using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data.Database;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamuraiApp.Data
{
    public class BusinessDataLogic
    {
        private SamuraiContext _ctx;

        public BusinessDataLogic(SamuraiContext ctx)
        {
            _ctx = ctx;
        }
        public BusinessDataLogic()
        {}

        public int AddSamuraisByName(params string[] names)
        {
            foreach (var name in names)
            {
                _ctx.Samurais.Add(new Samurai { Name = name });
            }
            // Return save changes count - use in testing
            return _ctx.SaveChanges();

        }
        public int InsertNewSamurai(Samurai samurai)
        {
            _ctx.Samurais.Add(samurai);
            return _ctx.SaveChanges();
        }

        public Samurai GetSamuraiWithQuotes(int samuraiId)
        {
            var samuraiWithQuotes = _ctx.Samurais.Where(s => s.Id == samuraiId)
                                                 .Include(s => s.Quotes)
                                                 .FirstOrDefault();
            return samuraiWithQuotes;
        }
        public async ValueTask<Samurai> GetSamuraiById(int id)
        {
            return await _ctx.Samurais.FindAsync(id);
        }
        public async Task<Samurai> AddSamurai(Samurai samurai)
        {
            _ctx.Add(samurai);
            await _ctx.SaveChangesAsync();
            return samurai;
        }
        public async Task<IEnumerable<Samurai>> GetAllSamurais()
        {
            return await _ctx.Samurais.ToListAsync();
        }
        public async Task<Samurai> DeleteSamurai(int id)
        {
            var samurai = await _ctx.Samurais.FindAsync(id);
            if (samurai == null)
            {
                return null;
            }
            _ctx.Samurais.Remove(samurai);
            await _ctx.SaveChangesAsync();
            return samurai;
        }
        public async Task<bool> UpdateSamurai(Samurai samurai)
        {
            try
            {
                _ctx.Entry(samurai).State = EntityState.Modified;
                await _ctx.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
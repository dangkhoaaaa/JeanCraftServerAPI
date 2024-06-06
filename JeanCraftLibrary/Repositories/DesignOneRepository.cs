using Azure.Core;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class DesignOneRepository : GenericRepository<DesignOne>, IDesignOneRepository
    {
        private readonly JeanCraftContext _dbContext;

        public DesignOneRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<DesignOne>> GetAllDesignOnes()
        {
            return await _dbContext.DesignOnes.Include(d => d.FitNavigation).ThenInclude(c => c.TypeNavigation).Include(d => d.FrontPocketNavigation).ThenInclude(c => c.TypeNavigation)
                .Include(d => d.BackPocketNavigation).ThenInclude(c => c.TypeNavigation).Include(d => d.CuffsNavigation).ThenInclude(c => c.TypeNavigation)
                .Include(d => d.FlyNavigation).ThenInclude(c => c.TypeNavigation).Include(d => d.LengthNavigation).ThenInclude(c => c.TypeNavigation).ToListAsync();
        }
        public async Task<Guid?> FindDesignOneByComponentsAsync(Guid? fit, Guid? length, Guid? cuffs, Guid? fly, Guid? frontPocket, Guid? backPocket)
        {
            var designOne = await _dbContext.DesignOnes
                .Where(d => d.Fit == fit &&
                            d.Length == length &&
                            d.Cuffs == cuffs &&
                            d.Fly == fly &&
                            d.FrontPocket == frontPocket &&
                            d.BackPocket == backPocket)
                .FirstOrDefaultAsync();

            return designOne?.DesignOneId;
        }

        public async Task<DesignOne> GetDesignOneById(Guid designOneId)
        {
            var designOne = await _dbContext.DesignOnes.Include(d => d.BackPocketNavigation).ThenInclude(c => c.TypeNavigation)
        .Include(d => d.CuffsNavigation).ThenInclude(c => c.TypeNavigation).Include(d => d.FitNavigation).ThenInclude(c => c.TypeNavigation)
        .Include(d => d.FlyNavigation).ThenInclude(c => c.TypeNavigation).Include(d => d.FrontPocketNavigation).ThenInclude(c => c.TypeNavigation)
        .Include(d => d.LengthNavigation).ThenInclude(c => c.TypeNavigation).FirstOrDefaultAsync(d => d.DesignOneId == designOneId);

            return designOne;
        }
        public async Task<DesignOneResponse> CreateAsync(DesignOneRequest designOneRequest)
        {
            // Kiểm tra sự tồn tại của các GUID
            if (designOneRequest.Fit.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.Fit.Value))
            {
                throw new ArgumentException("Invalid Fit ID");
            }

            if (designOneRequest.Length.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.Length.Value))
            {
                throw new ArgumentException("Invalid Length ID");
            }

            if (designOneRequest.Cuffs.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.Cuffs.Value))
            {
                throw new ArgumentException("Invalid Cuffs ID");
            }

            if (designOneRequest.Fly.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.Fly.Value))
            {
                throw new ArgumentException("Invalid Fly ID");
            }

            if (designOneRequest.FrontPocket.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.FrontPocket.Value))
            {
                throw new ArgumentException("Invalid Front Pocket ID");
            }

            if (designOneRequest.BackPocket.HasValue && !await _dbContext.Components.AnyAsync(c => c.ComponentId == designOneRequest.BackPocket.Value))
            {
                throw new ArgumentException("Invalid Back Pocket ID");
            }
            var designOne = new DesignOne
            {
                DesignOneId = Guid.NewGuid(),
                Fit = designOneRequest.Fit,
                Length = designOneRequest.Length,
                Cuffs = designOneRequest.Cuffs,
                Fly = designOneRequest.Fly,
                FrontPocket = designOneRequest.FrontPocket,
                BackPocket = designOneRequest.BackPocket
            };

            _dbContext.DesignOnes.Add(designOne);
            await _dbContext.SaveChangesAsync();

            var fitNavigation = await _dbContext.Components.FindAsync(designOne.Fit);
            var lengthNavigation = await _dbContext.Components.FindAsync(designOne.Length);
            var cuffsNavigation = await _dbContext.Components.FindAsync(designOne.Cuffs);
            var flyNavigation = await _dbContext.Components.FindAsync(designOne.Fly);
            var frontPocketNavigation = await _dbContext.Components.FindAsync(designOne.FrontPocket);
            var backPocketNavigation = await _dbContext.Components.FindAsync(designOne.BackPocket);

            // Trả về thông tin đầy đủ
            return new DesignOneResponse
            {
                DesignOneId = designOne.DesignOneId,
                Fit = designOne.Fit,
                Length = designOne.Length,
                Cuffs = designOne.Cuffs,
                Fly = designOne.Fly,
                FrontPocket = designOne.FrontPocket,
                BackPocket = designOne.BackPocket,
                FitNavigation = fitNavigation != null ? new Component
                {
                    ComponentId = fitNavigation.ComponentId,
                    Description = fitNavigation.Description,
                    Image = fitNavigation.Image,
                    Prize = fitNavigation.Prize,
                    Type = fitNavigation.Type
                } : null,
                LengthNavigation = lengthNavigation != null ? new Component
                {
                    ComponentId = lengthNavigation.ComponentId,
                    Description = lengthNavigation.Description,
                    Image = lengthNavigation.Image,
                    Prize = lengthNavigation.Prize,
                    Type = lengthNavigation.Type
                } : null,
                CuffsNavigation = cuffsNavigation != null ? new Component
                {
                    ComponentId = cuffsNavigation.ComponentId,
                    Description = cuffsNavigation.Description,
                    Image = cuffsNavigation.Image,
                    Prize = cuffsNavigation.Prize,
                    Type = cuffsNavigation.Type
                } : null,
                FlyNavigation = flyNavigation != null ? new Component
                {
                    ComponentId = flyNavigation.ComponentId,
                    Description = flyNavigation.Description,
                    Image = flyNavigation.Image,
                    Prize = flyNavigation.Prize,
                    Type = flyNavigation.Type
                } : null,
                FrontPocketNavigation = frontPocketNavigation != null ? new Component
                {
                    ComponentId = frontPocketNavigation.ComponentId,
                    Description = frontPocketNavigation.Description,
                    Image = frontPocketNavigation.Image,
                    Prize = frontPocketNavigation.Prize,
                    Type = frontPocketNavigation.Type
                } : null,
                BackPocketNavigation = backPocketNavigation != null ? new Component
                {
                    ComponentId = backPocketNavigation.ComponentId,
                    Description = backPocketNavigation.Description,
                    Image = backPocketNavigation.Image,
                    Prize = backPocketNavigation.Prize,
                    Type = backPocketNavigation.Type
                } : null
            };
        }
    }
}

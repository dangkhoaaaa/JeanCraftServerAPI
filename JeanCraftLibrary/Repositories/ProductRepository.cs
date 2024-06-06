using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftLibrary.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly JeanCraftContext _context;

        public ProductRepository(JeanCraftContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetProductByID(Guid id)
        {
            var productsWithDetails = _context.Products
                .Where(p => p.ProductId.Equals(id))
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.BackPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.CuffsNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FrontPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.LengthNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FlyNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FitNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FabricColorNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FinishingNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryColorNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.BranchBackPatchNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.ButtonAndRivetNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryFontNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.MonoGramNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.StitchingThreadColorNavigation)
                .Include(p => p.ProductInventory); ;

            return await productsWithDetails.FirstOrDefaultAsync();
        }

        public async Task<Product[]> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetProductID(GetProductIDByDesignRequest request)
        {
            return await _context.Products.Where(x => x.DesignOneId == request.DesignOne && x.DesignTwoId == request.DesignTwo && x.DesignThreeId == request.DesignThree).SingleOrDefaultAsync();
        }

        public async Task<Product[]?> GetProductList()
        {
            var productsWithDetails = _context.Products
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.BackPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.CuffsNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FrontPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.LengthNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FlyNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FitNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FabricColorNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FinishingNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryColorNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.BranchBackPatchNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.ButtonAndRivetNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryFontNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.MonoGramNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.StitchingThreadColorNavigation)
                .Include(p => p.ProductInventory);

            return await productsWithDetails.ToArrayAsync();
        }

        public async Task<Product[]?> SearchProduct(SearchProductRequest filter)
        {
            var productsWithDetails = _context.Products
                .Where(p => p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice && p.Size >= filter.MinSize && p.Size <= filter.MaxSize)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.BackPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.CuffsNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FrontPocketNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.LengthNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FlyNavigation)
                .Include(p => p.DesignOne)
                    .ThenInclude(d => d.FitNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FabricColorNavigation)
                .Include(p => p.DesignTwo)
                    .ThenInclude(d => d.FinishingNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryColorNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.BranchBackPatchNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.ButtonAndRivetNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.EmbroideryFontNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.MonoGramNavigation)
                .Include(p => p.DesignThree)
                    .ThenInclude(d => d.StitchingThreadColorNavigation)
                .Include(p => p.ProductInventory); ;
            return await productsWithDetails.ToArrayAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var existingEntity = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == product.ProductId);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(product);
            }
            else
            {
                _context.Products.Update(product);
            }
            await _context.SaveChangesAsync();
            return product;
        }
    }
}

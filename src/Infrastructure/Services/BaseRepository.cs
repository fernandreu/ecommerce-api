using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

using AutoMapper;

using ECommerceAPI.ApplicationCore.Entities;
using ECommerceAPI.ApplicationCore.Interfaces;
using ECommerceAPI.Infrastructure.Entries;

namespace ECommerceAPI.Infrastructure.Services
{
    public class BaseRepository<TEntity, TEntry> : IAsyncRepository<TEntity>
        where TEntity : BaseEntity
        where TEntry : BaseEntry
    {
        public BaseRepository(IDynamoDBContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
            this.Prefix = BaseEntry.GetPrefix<TEntry>();
        }
        
        protected IDynamoDBContext Context { get; }

        protected IMapper Mapper { get; }

        protected string Prefix { get; }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var results = await this.Context.QueryAsync<TEntry>(this.Prefix + id).GetRemainingAsync();
            return this.Mapper.Map<TEntity>(results.FirstOrDefault());
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var results = await this.Context.ScanAsync<TEntry>(new[]
            {
                new ScanCondition(nameof(BaseEntry.Id), ScanOperator.BeginsWith, this.Prefix),
            }).GetRemainingAsync();

            return results.Select(x => this.Mapper.Map<TEntity>(x));
        }

        public async Task<TEntity> PutAsync(TEntity entity)
        {
            await this.Context.SaveAsync(this.Mapper.Map<TEntry>(entity));

            // There could be some validation of whether the order makes sense, although that is more
            // a job of the service using this repository
            return entity;
        }

        public async Task<TEntity> PostAsync(TEntity entity)
        {
            var entry = this.Mapper.Map<TEntry>(entity);

            // Ignore the ID passed (if any) and create a new one instead. The loop might be a bit of
            // an overkill since there is extremely low probability of collision
            do
            {
                entry.Id = Guid.NewGuid().ToString();
            }
            while (await this.GetByIdAsync(entry.Id) != null);
            
            await this.Context.SaveAsync(entry);

            // This is effectively the same as creating a copy of the original entity with a new ID so
            // that the object originally passed is unaffected
            return this.Mapper.Map<TEntity>(entry);
        }
    }
}

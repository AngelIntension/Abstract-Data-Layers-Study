﻿using DataLayer;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DomainLayer.Services
{
    public class ProductService
    {
        private IProductRepository repository;

        public ProductService([NotNull] IProductRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Product> All()
        {
            var products = repository.All()
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                }
            );
            return products;
        }
    }
}
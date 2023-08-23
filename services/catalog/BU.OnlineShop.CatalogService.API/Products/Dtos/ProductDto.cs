﻿namespace BU.OnlineShop.CatalogService.API.Products.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}

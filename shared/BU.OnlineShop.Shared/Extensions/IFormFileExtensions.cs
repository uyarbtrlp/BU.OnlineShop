﻿using Microsoft.AspNetCore.Http;

namespace BU.OnlineShop.Shared.Extensions
{
    public static class IFormFileExtensions 
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

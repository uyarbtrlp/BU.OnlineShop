namespace Bu.OnlineShop.BasketService.Domain.Shared
{
    public static class BasketServiceErrorCodes
    {
        public static string NotEnoughProductsException = "BasketService:00001";
        public static string BasketItemDoesNotExistException = "BasketService:00002";
        public static string PaymentNotSuccessfulException = "BasketService:00003";
        public static string UserAlreadyHasBasketException = "BasketService:00004";
    }
}

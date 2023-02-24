using System;
namespace Models.Constant
{
    public static class ErrorMessage
    {
        #region Common error message
        public static class CommonError
        {
            public readonly static string ID_IS_NULL = "ID request is null";
            public readonly static string INVALID_REQUEST = "Request is not valid";
        }
        #endregion

        #region Type error message
        public static class TypeError
        {
            public readonly static string TYPE_NOT_FOUND = "Type is not existed";
        }
        #endregion


        #region Product error message
        public static class ProductError
        {
            public readonly static string PRODUCT_NOT_FOUND = "Product is not existed";
            public readonly static string PRODUCT_EXISTED = "Product is existed";
        }
        #endregion

        #region User error message
        public static class UserError
        {
            public readonly static string USER_NOT_FOUND = "User is not existed";
            public readonly static string USER_EXISTED = "User is existed";
        }
        #endregion

        #region Customer error message
        public static class CustomerError
        {
            public readonly static string CUSTOMER_NOT_FOUND = "Customer is not existed";
            public readonly static string CUSTOMER_EXISTED = "Customer is existed";
        }
        #endregion

        #region Basket error message
        public static class BasketError
        {
            public readonly static string BASKET_NOT_FOUND = "Basket is not existed";
            public readonly static string BASKET_EXISTED = "Basket is existed";
        }
        #endregion

        #region Order error message
        public static class OrderError
        {
            public readonly static string ORDER_NOT_FOUND = "Order is not existed";
            public readonly static string ORDER_EXISTED = "Order is existed";
        }
        #endregion

        #region Request error message
        public static class RequestError
        {
            public readonly static string REQUEST_NOT_FOUND = "Request is not existed";
            public readonly static string REQUEST_EXISTED = "Request is existed";
        }
        #endregion
    }
}


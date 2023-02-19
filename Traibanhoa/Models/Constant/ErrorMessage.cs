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
    }
}


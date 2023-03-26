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
            public readonly static string LIST_IS_NULL = "List is null";
        }
        #endregion

        #region Type error message
        public static class TypeError
        {
            public readonly static string TYPE_NOT_FOUND = "Type is not existed";
        }
        #endregion

        #region Cart error message
        public static class CartError
        {
            public readonly static string CART_NOT_FOUND = "This cart is not found";
            public readonly static string ITEM_NOT_FOUND = "This item is not found";
            public readonly static string QUANTITY_NOT_VALID = "Quantity must be more than 0";
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
            public readonly static string USER_NOT_LOGIN = "You did not login. Login and try this action again, please!";
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
            public readonly static string BASKET_SUBCATES_LIMIT = "Max Sub categories for basket is 5";
        }
        #endregion

        #region Request Basket error message
        public static class RequestBasketError
        {
            public readonly static string REQUEST_BASKET_NOT_FOUND = "Basket is not existed";
            public readonly static string REQUEST_BASKET_EXISTED = "Basket is existed";
        }
        #endregion

        #region Order error message
        public static class OrderError
        {
            public readonly static string SHIPPING_DATE_NOT_VALID = "Shipping date not valid";
            public readonly static string ORDER_NOT_FOUND = "Order not found";
            public readonly static string COOKED_ORDER_NOT_VALID = "Cooked order cannot have ingredient or package";
            public readonly static string ORDER_CANNOT_CHANGE_STATUS = "Order status cannot be change";
            public readonly static string ORDER_SHIPPING_ADDRESS_REQUIRED = "Shipping address is required";
            public readonly static string ORDER_TOTAL_PRICE_NOT_VALID = "Total price must higher than 1";
            public readonly static string ORDER_PAYMENT_METHOD_NOT_VALID = "Payment method not valid";
        }
        #endregion

        #region Transaction error message
        public static class TransactionError
        {
            public readonly static string TRANSACTION_NOT_FOUND = "Transaction not found";
            public readonly static string TRANSACTION_NOT_ON_PENDING = "Transaction status cannot be change";
        }
        #endregion

        #region Mail service error message
        public static class MailError
        {
            public readonly static string MAIL_SENDING_ERROR = "Mail cannot be send";
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


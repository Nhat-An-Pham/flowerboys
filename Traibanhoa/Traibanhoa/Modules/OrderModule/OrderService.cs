using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.OrderModule.Interface;
using Models.Models;
using Traibanhoa.Modules.OrderModule.Request;
using Models.Constant;
using Traibanhoa.Modules.TypeModule.Request;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Net.Mail;
using System.Net;
using Traibanhoa.Modules.OrderModule.Response;
using Traibanhoa.Modules.ProductModule.Interface;
using Traibanhoa.Modules.BasketModule.Interface;
using Models.Enum;
using Traibanhoa.Modules.TransactionModule.Interface;
using Traibanhoa.Modules.CustomerModule.Interface;

namespace Traibanhoa.Modules.OrderModule
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICustomerRepository _customerRepository; 
        IConfiguration _configuration;
        public OrderService(IOrderRepository OrderRepository, ITransactionRepository transactionRepository, ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _OrderRepository = OrderRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _configuration = configuration;
        }

        public async Task<ICollection<OrderResponse>> GetOrderResponse(int status = -1)
        {
            var orders = status > -1
                ? await _OrderRepository.GetOrdersBy(
                includeProperties: "OrderDetails")
                : await _OrderRepository.GetOrdersBy(o => o.OrderStatus == status,
                includeProperties: "OrderDetails");

            var orderResponses = orders.Select(o => new OrderResponse
            {
                Email = o.Email,
                OrderBy = o.OrderBy,
                OrderDate = o.OrderDate,
                OrderId = o.OrderId,
                PaymentMethod = o.PaymentMethod,
                Phonenumber = o.Phonenumber,
                ShippedAddress = o.ShippedAddress,
                ShippedDate = o.ShippedDate,
                TotalPrice = o.TotalPrice,
                IsRequest = o.IsRequest,
                OrderDetails = o.OrderDetails,
                OrderStatus = o.OrderStatus,
            }).ToList();

            return orderResponses;
        }

        public Task<ICollection<Order>> GetOrdersBy(
                Expression<Func<Order,
                bool>> filter = null,
                Func<IQueryable<Order>,
                ICollection<Order>> options = null,
                string includeProperties = null)
        {
            return _OrderRepository.GetOrdersBy(filter, options, includeProperties);
        }

        public async Task<string> AddNewOrder(Order newOrder)
        {
            var redirectUrl = "";
            try
            {
                newOrder.OrderId = Guid.NewGuid();
                newOrder.OrderDate = DateTime.Now;
                newOrder.OrderStatus = (int)OrderStatus.PENDING;

                if (string.IsNullOrEmpty(newOrder.ShippedAddress))
                    throw new Exception(ErrorMessage.OrderError.ORDER_SHIPPING_ADDRESS_REQUIRED);
                if (newOrder.TotalPrice < 10000)
                    throw new Exception(ErrorMessage.OrderError.ORDER_TOTAL_PRICE_NOT_VALID);

                #region create transaction
                var transaction = new Transaction()
                {
                    TransactionId = newOrder.OrderId,
                    TotalAmount = newOrder.TotalPrice.Value,
                    CreatedDate = DateTime.Now,
                    TransactionStatus = (int)TransactionStatus.PENDING,
                    CustomerId = newOrder.OrderBy
                };
                #endregion

                var transactionScope = _OrderRepository.Transaction();
                using (transactionScope)
                {
                    try
                    {
                        newOrder.OrderDetails.ToList().ForEach(detail =>
                        {
                            detail.OrderDetailId = Guid.NewGuid();
                            detail.OrderId = newOrder.OrderId;
                            if (detail.BasketId == null && detail.RequestBasketId == null)
                                throw new Exception("Basket id is required");
                            else if (detail.BasketId != null && detail.RequestBasketId != null) {
                                throw new Exception("Order is not legal to execute");
                            }
                        });
                        await _OrderRepository.AddAsync(newOrder);

                        await _transactionRepository.AddAsync(transaction);

                        if (newOrder.PaymentMethod.HasValue)
                        {
                            switch (newOrder.PaymentMethod.GetValueOrDefault())
                            {
                                case (int)PaymentMethod.PAYPAL:
                                    redirectUrl = await PaymentWithPaypal(newOrder.OrderId);
                                    break;
                                case (int)PaymentMethod.COD:
                                    break;
                                default:
                                    throw new Exception(ErrorMessage.OrderError.ORDER_PAYMENT_METHOD_NOT_VALID);
                            }
                        }
                        else
                            throw new Exception(ErrorMessage.OrderError.ORDER_PAYMENT_METHOD_NOT_VALID);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Transaction fail - {e.Message}");
                    }

                    transactionScope.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Add new order fail - {ex.Message}");
            }

            return redirectUrl;
        }

        public async Task UpdateOrder(Order OrderUpdate)
        {
            await _OrderRepository.UpdateAsync(OrderUpdate);
        }

        public Order GetOrderByID(Guid? id)
        {
            return _OrderRepository.GetFirstOrDefaultAsync(x => x.OrderId.Equals(id)).Result;
        }

        public async Task DeleteOrder(Guid id)
        {
            var order = await _OrderRepository.GetByIdAsync(id);

            if (order == null)
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

            order.OrderStatus = (int)OrderStatus.DELETED;
            await _OrderRepository.UpdateAsync(order);
        }

        public async Task<ICollection<Order>> GetByCustomer(Guid id)
        {
            return await _OrderRepository.GetOrdersBy(o => o.OrderBy.Equals(id) && o.OrderStatus != (int)OrderStatus.DELETED,
                includeProperties: "OrderDetails");
        }

        public async Task AcceptOrder(Guid orderId)
        {
            var order = await _OrderRepository.GetByIdAsync(orderId);

            if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

            var customer = await _customerRepository.GetByIdAsync(order.OrderBy.Value);

            if (order.OrderStatus != (int)OrderStatus.PENDING)
                throw new Exception(ErrorMessage.OrderError.ORDER_CANNOT_CHANGE_STATUS);

            order.OrderStatus = (int)OrderStatus.ACCEPTED;

            #region update order status, create transaction if COD, update status if Paypal
            var transactionScope = _OrderRepository.Transaction();
            using (transactionScope)
            {
                await _OrderRepository.UpdateAsync(order);

                if (order.PaymentMethod == (int)PaymentMethod.PAYPAL)
                {
                    var transaction = await _transactionRepository.GetByIdAsync(orderId);
                    if (transaction == null)
                        throw new Exception(ErrorMessage.TransactionError.TRANSACTION_NOT_FOUND);
                    transaction.TransactionStatus = (int)TransactionStatus.SUCCESS;
                    await _transactionRepository.UpdateAsync(transaction);
                }
                else
                {
                    #region create transaction
                    var transaction = new Transaction()
                    {
                        TransactionId = order.OrderId,
                        TotalAmount = order.TotalPrice.Value,
                        CreatedDate = DateTime.Now,
                        TransactionStatus = (int)TransactionStatus.PENDING,
                        CustomerId = order.OrderBy
                    };
                    #endregion
                    await _transactionRepository.AddAsync(transaction);
                }

                #region sending mail
                if (customer.Email != null)
                {
                    // gui mail thong tin order
                    var mailSubject = $"[Da duyet] Thong tin don hang #{order.OrderId}";
                    var mailBody = $"Cam on ban da mua hang, don hang #{order.OrderId} da duoc duyet.\n" +
                        $"Don hang cua ban dang duoc giao";

                    SendMail(mailSubject, mailBody, customer.Email);
                }
                #endregion

                transactionScope.Commit();
            }
            #endregion

        }

        public async Task DenyOrder(Guid id)
        {
            var order = await _OrderRepository.GetByIdAsync(id);

            if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

            var customer = await _customerRepository.GetByIdAsync(order.OrderBy.Value);

            order.OrderStatus = (int)OrderStatus.DENIED;

            var transactionScope = _OrderRepository.Transaction();
            using (transactionScope)
            {
                await _OrderRepository.UpdateAsync(order);

                if (order.PaymentMethod == (int)PaymentMethod.PAYPAL)
                {
                    var transaction = await _transactionRepository.GetByIdAsync(order.OrderId);
                    if (transaction == null)
                        throw new Exception(ErrorMessage.TransactionError.TRANSACTION_NOT_FOUND);
                    transaction.TransactionStatus = (int)TransactionStatus.FAIL;
                    await _transactionRepository.UpdateAsync(transaction);
                }

                #region sending mail
                if (customer.Email != null)
                {
                    // gui mail thong bao don hang bi tu choi
                    var mailSubject = $"[Tu choi don hang] Thong tin don hang #{order.OrderId}";
                    var mailBody = $"Don hang #{order.OrderId} da bi tu choi.";

                    SendMail(mailSubject, mailBody, customer.Email);
                }
                #endregion

                transactionScope.Commit();
            }
        }

        public async Task CancelOrder(Guid id)
        {
            var order = await _OrderRepository.GetByIdAsync(id);

            if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

            var customer = await _customerRepository.GetByIdAsync(order.OrderBy.Value);

            order.OrderStatus = (int)OrderStatus.CANCEL;

            var transactionScope = _OrderRepository.Transaction();
            using (transactionScope)
            {
                await _OrderRepository.UpdateAsync(order);

                if (order.PaymentMethod == (int)PaymentMethod.PAYPAL)
                {
                    var transaction = await _transactionRepository.GetByIdAsync(order.OrderId);
                    if (transaction == null)
                        throw new Exception(ErrorMessage.TransactionError.TRANSACTION_NOT_FOUND);
                    transaction.TransactionStatus = (int)TransactionStatus.FAIL;
                    await _transactionRepository.UpdateAsync(transaction);
                }

                #region sending mail
                if (customer.Email != null)
                {
                    // gui mail thong bao don hang bi huy
                    var mailSubject = $"[Huy don hang] Thong tin don hang #{order.OrderId}";
                    var mailBody = $"Don hang #{order.OrderId} da bi huy.";

                    SendMail(mailSubject, mailBody, customer.Email);
                }
                #endregion

                transactionScope.Commit();
            }
        }

        public async Task Shipping(Guid id)
        {
            try
            {
                var order = await _OrderRepository.GetByIdAsync(id);

                if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

                if (order.OrderStatus != (int)OrderStatus.ACCEPTED)
                    throw new Exception(ErrorMessage.OrderError.ORDER_CANNOT_CHANGE_STATUS);

                order.OrderStatus = (int)OrderStatus.SHIPPING;
                await _OrderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Shipping Status fail - {ex.Message}");
                throw new Exception($"Update Shipping Status fail");
            }

        }

        public async Task Delivered(Guid id)
        {
            try
            {
                var order = await _OrderRepository.GetByIdAsync(id);

                if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

                if (order.OrderStatus != (int)OrderStatus.SHIPPING)
                    throw new Exception(ErrorMessage.OrderError.ORDER_CANNOT_CHANGE_STATUS);

                order.OrderStatus = (int)OrderStatus.DELIVERED;
                await _OrderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Delivered Status fail - {ex.Message}");
                throw new Exception($"Update Delivered Status fail");
            }

        }

        public async Task DeliveredFail(Guid id)
        {
            try
            {
                var order = await _OrderRepository.GetByIdAsync(id);

                if (order == null || order.OrderStatus == (int)OrderStatus.DELETED)
                    throw new Exception(ErrorMessage.OrderError.ORDER_NOT_FOUND);

                if (order.OrderStatus != (int)OrderStatus.SHIPPING)
                    throw new Exception(ErrorMessage.OrderError.ORDER_CANNOT_CHANGE_STATUS);

                order.OrderStatus = (int)OrderStatus.DELIVERED_FAIL;
                await _OrderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Delivered Status fail - {ex.Message}");
                throw new Exception($"Update Delivered Status fail");
            }
        }

        public void SendMail(string mailSubject, string mailBody, string receiver)
        {
            var address = _configuration.GetValue<string>("MailService:Address");
            var password = _configuration.GetValue<string>("MailService:Password");
            var smtpClientConfig = _configuration.GetValue<string>("MailService:SmtpClient");

            try
            {
                var smtpClient = new SmtpClient(smtpClientConfig)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(address, password),
                    EnableSsl = true,
                };

                smtpClient.Send(address, receiver, mailSubject, mailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorMessage.MailError.MAIL_SENDING_ERROR} - {ex.Message}");
                throw new Exception(ErrorMessage.MailError.MAIL_SENDING_ERROR);
            }
        }

        public async Task<string> PaymentWithPaypal(
            Guid orderId,
            string Cancel = null,
        string blogId = "",
        string PayerID = "",
            string guid = "")
        {
            var clientId = _configuration.GetValue<string>("Paypal:Key");
            var clientSecret = _configuration.GetValue<string>("Paypal:Secret");
            var mode = _configuration.GetValue<string>("Paypal:mode");
            PayPal.Api.APIContext apiContext = GetAPIContext(clientId, clientSecret, mode);
            string returnURI = _configuration.GetValue<string>("Paypal:returnURI");

            try
            {
                guid = Convert.ToString(orderId);

                Console.WriteLine($"Creating payment for order {guid}");
                var createdPayment = await CreatePaymentAsync(apiContext, returnURI + "guid=" + guid, blogId, orderId);
                Console.WriteLine($"Created - {createdPayment.ConvertToJson()}");

                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;

                while (links.MoveNext())
                {
                    PayPal.Api.Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        paypalRedirectUrl = lnk.href;
                    }
                }

                if (string.IsNullOrEmpty(paypalRedirectUrl))
                    throw new Exception("PaymentFailed");
                return paypalRedirectUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Creating payment Failed - " + ex.Message);
                throw new Exception("PaymentFailed");
            }
        }

        private async Task<PayPal.Api.Payment> CreatePaymentAsync(
            PayPal.Api.APIContext apiContext,
        string redirectUrl,
        string blogId,
            Guid orderId)
        {
            var currencyRate = _configuration.GetValue<string>("Paypal:currencyRate");

            var transaction = await _transactionRepository.GetByIdAsync(orderId);
            if (transaction == null)
                throw new Exception(ErrorMessage.TransactionError.TRANSACTION_NOT_FOUND);

            var payer = new PayPal.Api.Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new PayPal.Api.RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl + "&Cancel=false"
            };

            var amount = new PayPal.Api.Amount()
            {
                currency = "USD",
                total = (transaction.TotalAmount.GetValueOrDefault() / Decimal.Parse(currencyRate)).ToString("#.##")
            };

            var transactionList = new List<PayPal.Api.Transaction>();

            transactionList.Add(new PayPal.Api.Transaction()
            {
                invoice_number = transaction.TransactionId.ToString(),
                amount = amount
            });

            var payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        private static string GetAccessToken(string clientId, string clientSecret, string mode)
        {
            string accessToken = new PayPal.Api.OAuthTokenCredential(
                clientId,
                clientSecret,
                new Dictionary<string, string>()
            {
                { "mode", mode }
            }).GetAccessToken();

            return accessToken;
        }

        public static PayPal.Api.APIContext GetAPIContext(string clientId, string clientSecret, string mode)
        {
            PayPal.Api.APIContext apiContext = new PayPal.Api.APIContext(GetAccessToken(clientId, clientSecret, mode));
            apiContext.Config = new Dictionary<string, string>()
            {
                { "mode", mode }
            };
            return apiContext;
        }

        public async Task<ICollection<Order>> GetAll()
        {
            return await _OrderRepository.GetAll();
        }
    }
}

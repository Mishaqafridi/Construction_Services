using Construction_Admin_Service.Data;
using Construction_Admin_Service.Dtos;
using Construction_Admin_Service.Models;
using Construction_Admin_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPal.Api;


namespace Construction_Admin_Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly IContractor _IContractor;
        public string UserName { get; set; }
        public string Role { get; set; }
        private readonly IAuthRepository _Iauth;

        private readonly ILogger<PayPalController> _logger;
        private IHttpContextAccessor httpContextAccessor;
        IConfiguration _configuration;


        public ContractorController(IContractor IContractor, IAuthRepository Iauth, ILogger<PayPalController> logger, IHttpContextAccessor context, IConfiguration iconfiguration)
        {
            _IContractor = IContractor;
            _Iauth = Iauth;
            _logger = logger;
            httpContextAccessor = context;
            _configuration = iconfiguration;
        }


        [HttpGet("GetAll")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<List<ContractorDto>>>> GetAllContactor()
        {
            var result = await _IContractor.GetAllContactor();
            return Ok(result.Data);
        }


        [HttpGet("GetContractorDrop")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<List<GetContractorDrop>>>> GetContractorDrop()
        {
            return Ok(await _IContractor.GetContractorDrop());
        }


        [HttpGet("GetContactorById")]
        [Authorize(Roles = "admin,user,manager")]
        public async Task<ActionResult<ServiceResponse<ContractorDto>>> GetContactorById(int id)
        {
            return Ok(await _IContractor.GetContactorById (id));
        }



        [HttpPost("AddContactor")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<int>>> AddContactor(AddContractorDto Employee)
        {
            var result = await _IContractor.AddContactor(Employee);
            return Ok(result);
        }


        [HttpPost("UpdateContactor")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateContactor(UpdateContractorDto Employee)
        {

            var response = await _IContractor.UpdateContactor(Employee);
            if (response.Success == false)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);

            }
            //  }

        }


        [HttpGet("GetContactorPaymentById")]
        [Authorize(Roles = "admin,user,manager")]
        public async Task<ActionResult<ServiceResponse<GetPaymentDto>>> GetContactorPaymentById(int id)
        {
            return Ok(await _IContractor.GetContactorPaymentById(id));
        }



        [HttpPost("AddContactorPayment")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<int>>> AddContactorPayment(AddPaymentDto Employee)
        {
            Employee.Quotationstatus = true;
            Employee.Amountstatus = true;
            string Cancel = null; string blogId = ""; string PayerID = ""; string guid = ""; decimal PaidAmount = Employee.AmmountPaid;
            var result = await _IContractor.AddContactorPayment(Employee);

           var urlrestl= PaymentWithPaypal(Cancel, blogId, PayerID, guid, PaidAmount);
            // result = urlrestl;
            urlrestl.Data = "1";
            {
                if (urlrestl.Success)
                {
                    return Ok(urlrestl);
                }
                return Ok(urlrestl);

            }
        }

        [HttpPost("UpdateContactorPayment")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<ServiceResponse<int>>> UpdateContactorPayment(UpdatePaymentDto Employee)
        {

            var response = await _IContractor.UpdateContactorPayment(Employee);
            if (response.Success == false)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);


            }
            //  }

        }

        [HttpGet("PaymentWithPaypal")]
        //[Authorize(Roles = "admin")]
        ServiceResponse<string> PaymentWithPaypal(string Cancel, string blogId , string PayerID , string guid , decimal PaidAmount)
        {

            var serviceResponse = new ServiceResponse<string>();

            if (blogId == null || PayerID == null || guid == null)
            {
                blogId = ""; PayerID = ""; guid = "";
            }
            //getting the apiContext  
            var ClientID = _configuration.GetValue<string>("PayPal:Key");
            var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            var mode = _configuration.GetValue<string>("PayPal:mode");
            APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            //apiContext.AccessToken="Bearer access_token$production$j27yms5fthzx9vzm$c123e8e154c510d70ad20e396dd28287";
            try
            {
                //A resource repr//esenting a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = PayerID;
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PayPal/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guidd = Convert.ToString((new Random()).Next(100000));
                    guid = guidd;
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId, PaidAmount);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);

                    serviceResponse.Success = true;
                    serviceResponse.Message = paypalRedirectUrl;
                    return serviceResponse;
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
                    var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "";
                        return serviceResponse;
                    }
                    var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "";
                    return serviceResponse;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = "";
                return serviceResponse;
            }
            //on successful payment, show success page to user.  
            serviceResponse.Success = false;
            serviceResponse.Message = "blogId == null || PayerID == null || guid == null";
            return serviceResponse;
            ////getting the apiContext  
            //var ClientID = _configuration.GetValue<string>("PayPal:Key");
            //var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
            //var mode = _configuration.GetValue<string>("PayPal:mode");
            //APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
            ////apiContext.AccessToken="Bearer access_token$production$j27yms5fthzx9vzm$c123e8e154c510d70ad20e396dd28287";
            //try
            //{
            //    //A resource repr//esenting a Payer that funds a payment Payment Method as paypal  
            //    //Payer Id will be returned when payment proceeds or click to pay  
            //    string payerId = PayerID;
            //    if (string.IsNullOrEmpty(payerId))
            //    {
            //        //this section will be executed first because PayerID doesn't exist  
            //        //it is returned by the create function call of the payment class  
            //        // Creating a payment  
            //        // baseURL is the url on which paypal sendsback the data.  
            //        string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/PayPal/PaymentWithPayPal?";
            //        //here we are generating guid for storing the paymentID received in session  
            //        //which will be used in the payment execution  
            //        var guidd = Convert.ToString((new Random()).Next(100000));
            //        guid = guidd;
            //        //CreatePayment function gives us the payment approval url  
            //        //on which payer is redirected for paypal account payment  
            //        var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId, PaidAmount);
            //        //get links returned from paypal in response to Create function call  
            //        var links = createdPayment.links.GetEnumerator();
            //        string paypalRedirectUrl = null;
            //        while (links.MoveNext())
            //        {
            //            Links lnk = links.Current;
            //            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
            //            {
            //                //saving the payapalredirect URL to which user will be redirected for payment  
            //                paypalRedirectUrl = lnk.href;
            //            }
            //        }
            //        // saving the paymentID in the key guid  
            //        httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
            //        return Redirect(paypalRedirectUrl);
            //    }
            //    else
            //    {
            //        // This function exectues after receving all parameters for the payment  
            //        var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
            //        var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
            //        //If executed payment failed then we will show payment failure message to user  
            //        if (executedPayment.state.ToLower() != "approved")
            //        {
            //            return Ok(0);
            //        }
            //        var blogIds = executedPayment.transactions[0].item_list.items[0].sku;
            //        return Ok(1);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return Ok(0);
            //}
            ////on successful payment, show success page to user.  
            //return Ok(1);
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId, decimal PaidAmount)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Detail",
                currency = "USD",
                price = PaidAmount.ToString(),
                quantity = "1",
                sku = "asd"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            //var details = new Details()
            //{
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "1"
            //};
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = PaidAmount.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                //details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Guid.NewGuid().ToString(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}

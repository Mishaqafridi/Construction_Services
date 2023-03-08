using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace Construction_Contractor
{
    public class PayPalService
    {
        private readonly PayPalHttpClient _client;
        private readonly string _baseUrl;

        public PayPalService(IConfiguration config)
        {
            _client = new PayPalHttpClient(new SandboxEnvironment(config["PayPal:AXJJKOETK1WVPAJPQQwX2LGJP4_tXG_fiWkjLB5RB62S716pOAHsm-iUfTUDXolBi4EFuwdjsTMwW1Nw"], config["PayPal:EJqqW-DrY-t7cCNcP3fOcxSFXJOLM2rqnvGLMGW7makSTh6aLPTxOiSCYGDx0h2m8GjoBl4e_WGcECdU"]));
            _baseUrl = config["PayPal:BaseUrl"];
        }

        public async Task<string> CreateOrder(decimal amount, string currencyCode)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(BuildRequestBody(amount, currencyCode));

            var response = await _client.Execute(request);

            return response.Result<Order>().Id;
        }

        private OrderRequest BuildRequestBody(decimal amount, string currencyCode)
        {
            var requestBody = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",

                PurchaseUnits = new List<PurchaseUnitRequest>()
            {
                new PurchaseUnitRequest()
                {
                    AmountWithBreakdown = new AmountWithBreakdown()
                    {
                        Value = amount.ToString("0.00"),
                        CurrencyCode = currencyCode
                    }
                }
            },

                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = $"{_baseUrl}/pay/success",
                    CancelUrl = $"{_baseUrl}/pay/cancel"
                }
            };

            return requestBody;
        }
    }
}

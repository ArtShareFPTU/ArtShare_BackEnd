using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOS;
using Newtonsoft.Json;
using PayPal.Api;


namespace Presentation.Pages;

public class CheckoutPage : PageModel
{
    private IHttpContextAccessor httpContextAccessor;
    private readonly HttpClient _client = new HttpClient();
    private readonly IConfiguration _configuration;

    public CheckoutPage(IHttpContextAccessor context, IConfiguration configuration)
    {
        httpContextAccessor = context;
        _configuration = configuration;
    }

    public List<Carts> CartsList { get; set; }

    public async Task OnGetAsync()
    {
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(json);
    }
    
    //public async Task<ActionResult> OnPostPayment(string Cancel = null, string blogId = "", string PayerID = "", string guid = "")
    //    {
    //        //getting the apiContext  
    //        var ClientID = _configuration.GetValue<string>("PayPal:Key");
    //        var ClientSecret = _configuration.GetValue<string>("PayPal:Secret");
    //        var mode = _configuration.GetValue<string>("PayPal:mode");
    //        APIContext apiContext = PaypalConfiguration.GetAPIContext(ClientID, ClientSecret, mode);
    //        // apiContext.AccessToken="Bearer access_token$production$j27yms5fthzx9vzm$c123e8e154c510d70ad20e396dd28287";
    //        try
    //        {
    //            //A resource representing a Payer that funds a payment Payment Method as paypal  
    //            //Payer Id will be returned when payment proceeds or click to pay  
    //            string payerId = PayerID;
    //            if (string.IsNullOrEmpty(payerId))
    //            {
    //                //this section will be executed first because PayerID doesn't exist  
    //                //it is returned by the create function call of the payment class  
    //                // Creating a payment  
    //                // baseURL is the url on which paypal sendsback the data.  
    //                string baseURI = this.Request.Scheme + "://" + this.Request.Host + "/Home/PaymentWithPayPal?";
    //                //here we are generating guid for storing the paymentID received in session  
    //                //which will be used in the payment execution  
    //                guid = GetIdFromJwt(_configuration["Tokens:Key"]);
    //                //CreatePayment function gives us the payment approval url  
    //                //on which payer is redirected for paypal account payment  
    //                var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, blogId);
    //                //get links returned from paypal in response to Create function call  
    //                var links = createdPayment.links.GetEnumerator();
    //                string paypalRedirectUrl = null;
    //                while (links.MoveNext())
    //                {
    //                    Links lnk = links.Current;
    //                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
    //                    {
    //                        //saving the payapalredirect URL to which user will be redirected for payment  
    //                        paypalRedirectUrl = lnk.href;
    //                    }
    //                }
    //                // saving the paymentID in the key guid  
    //                httpContextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
    //                return Redirect(paypalRedirectUrl);
    //            }
    //            else
    //            {
    //                // This function exectues after receving all parameters for the payment  

    //                var paymentId = httpContextAccessor.HttpContext.Session.GetString("payment");
    //                var executedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
    //                //If executed payment failed then we will show payment failure message to user  
    //                if (executedPayment.state.ToLower() != "approved")
    //                {

    //                    return RedirectToPage("PaymentFailed");
    //                }
    //                var blogIds = executedPayment.transactions[0].item_list.items[0].sku;


    //                return RedirectToPage("PaymentSuccess");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return RedirectToPage("PaymentFailed");
    //        }
    //    }
    

    public string GetIdFromJwt(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);

        tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        }, out SecurityToken validatedToken);

        var jwtTokenDecoded = (JwtSecurityToken)validatedToken;

        // Truy cập vào các thông tin trong payload
        string userId = jwtTokenDecoded.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        return userId;
    }
    
    private Payment payment;
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

    private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
    {
        // get cart from session
        var json = HttpContext.Session.GetString("cart");
        // deserialize cart
        if (json != null) CartsList = JsonConvert.DeserializeObject<List<Carts>>(json);
        
        //create itemlist and add item objects to it  

        var itemList = new ItemList()
        {
            items = new List<Item>()
        };
        //Adding Item Details like name, currency, price etc  
        foreach (var c in CartsList)
        {
            itemList.items.Add(new Item()
            {
                name = c.Title,
                currency = "USD",
                price = c.Price.ToString()
            });
        }
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
            total = "1.00", // Total must be equal to sum of tax, shipping and subtotal.  
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
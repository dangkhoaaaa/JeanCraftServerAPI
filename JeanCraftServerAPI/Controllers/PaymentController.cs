using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using JeanCraftLibrary.Entity;
using ZaloPay.Helper;
using ZaloPay.Helper.Crypto;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using JeanCraftServerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Net.payOS;
using Net.payOS.Types;
using JeanCraftLibrary.Model.Response;

namespace JeanCraftServerAPI.Controllers
{
    [ApiController]
    [Route("api/Payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVnPayService _vnPayservice;
        private readonly IConfiguration _config;
        public PaymentController(IPaymentService paymentService, IVnPayService vnPayservice, IConfiguration config)
        {
            _paymentService = paymentService;
            _vnPayservice = vnPayservice;
            _config = config;
        }


        private static readonly HttpClient client = new HttpClient();
        [HttpPost("payWithPayOS")]
        public async Task<IActionResult> PayWithPayOS([FromBody] PaymentModel requestModel)
        {
            Random r = new Random();
            PayOS payOS = new PayOS(_config["PayOS:clientId"], _config["PayOS:apiKey"], _config["PayOS:checksumKey"]);
            PaymentData paymentData = new PaymentData(r.NextInt64(1000000), ((int)requestModel.Amount), "Thanh toan JeanCraft", null, requestModel.RedirectUrl, requestModel.RedirectUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return Ok(createPayment);
        }

        [HttpPost("payWithVnPay")]
        public async Task<IActionResult> PayWithVnPay([FromBody] PaymentModel requestModel)
        {
            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = requestModel.Amount,
                CreatedDate = DateTime.Now,
                Description = $"JeanCraft-" + requestModel.OrderId.ToString,
                FullName = "model.HoTen",
                OrderId = requestModel.OrderId.ToString(),
                RedirectUrl = requestModel.RedirectUrl
            };
            return Ok(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
        }

        [HttpPost("payWithZaloPay")]
        public async Task<IActionResult> PayWithZaloPay([FromBody] PaymentModel requestModel)
        {
            string app_id = "2553";
            string key1 = "PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL";
            string create_order_url = "https://sb-openapi.zalopay.vn/v2/create";
            Random r = new Random();
            var embed_data = new { redirecturl = requestModel.RedirectUrl,
            orderId = requestModel.OrderId};
            var items = new[] { new { } };
            var param = new Dictionary<string, string>();
            var app_trans_id = r.Next(1000000);

            param.Add("app_id", app_id);
            param.Add("app_user", "JeanCraft");
            param.Add("app_time", Utils.GetTimeStamp().ToString());
            param.Add("amount", requestModel.Amount.ToString());
            param.Add("app_trans_id", DateTime.Now.ToString("yyMMdd") + "_" + app_trans_id); // mã giao dich có định dạng yyMMdd_xxxx
            param.Add("embed_data", JsonConvert.SerializeObject(embed_data));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", "JeanCraft - Thanh toán đơn hàng #" + app_trans_id);
            param.Add("bank_code", "zalopayapp");

            var data = app_id + "|" + param["app_trans_id"] + "|" + param["app_user"] + "|" + param["amount"] + "|"
                + param["app_time"] + "|" + param["embed_data"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));
            var quickPayResponse = await HttpHelper.PostFormAsync(create_order_url, param);
            
            return Ok(quickPayResponse);
        }



        [HttpPost("payWithMomo")]
        public async Task<IActionResult> PayWithMomo([FromBody] PaymentModel requestModel)
        {

            var request = new CollectionLinkRequest
            {
                orderInfo = "pay with MoMo",
                partnerCode = "MOMO",
                redirectUrl = requestModel.RedirectUrl,
                ipnUrl = Constants.IPNMOMO,
                amount = (long)requestModel.Amount,
                orderId = requestModel.OrderId.ToString() + ":" + Guid.NewGuid(),
                requestId = Guid.NewGuid().ToString(),
                requestType = "payWithMethod",
                extraData = "",
                partnerName = "JeanCraft Payment",
                storeId = "JeanCraft",
                orderGroupId = "",
                autoCapture = true,
                lang = "vi"
            };

            var rawSignature = $"accessKey={Constants.ACCESSKEY}&amount={request.amount}&extraData={request.extraData}&ipnUrl={request.ipnUrl}&orderId={request.orderId}&orderInfo={request.orderInfo}&partnerCode={request.partnerCode}&redirectUrl={request.redirectUrl}&requestId={request.requestId}&requestType={request.requestType}";
            request.signature = GetSignature(rawSignature, Constants.SECRETKEY);

            var httpContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var quickPayResponse = await client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);

            if (!quickPayResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)quickPayResponse.StatusCode, await quickPayResponse.Content.ReadAsStringAsync());
            }

            var responseContent = await quickPayResponse.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }


        [HttpPost("ipn")]
        public IActionResult ReceiveMoMoIPN([FromBody] MoMoIPNRequest request)
        {
            var rawData = $"partnerCode={request.PartnerCode}&orderId={request.OrderId}&requestId={request.RequestId}&amount={request.Amount}&orderInfo={request.OrderInfo}&orderType={request.OrderType}&transId={request.TransId}&resultCode={request.ResultCode}&message={request.Message}&payType={request.PayType}&responseTime={request.ResponseTime}&extraData={request.ExtraData}";
            var signature = GetSignature(rawData, Constants.SECRETKEY);

            if (signature != request.Signature)
            {
                return BadRequest("Invalid signature");
            }

            // TODO: Process the IPN request and update the order status in your database

            return NoContent(); // HTTP 204 No Content
        }

        private static string GetSignature(string text, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key)))
            {
                var hashBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(text));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


        [HttpGet("GetAllPayments")]
        public async Task<ActionResult<PaymentResult>> GetAllPayments([FromQuery] FormSearch search)
        {
            var paymentResult = _paymentService.GetAllPaging(search.currentPage, search.pageSize);

            if (paymentResult == null || paymentResult.Payments == null || paymentResult.Payments.Count == 0)
            {
                return NotFound("No payments found.");
            }

            return Ok(paymentResult);
        }

        [HttpGet("GetTotalAmount")]
        public async Task<ActionResult<double>> GetTotalAmount()
        {
            var totalAmount = _paymentService.GetTotalAmountOfPayments();

            return Ok(totalAmount);
        }

        [HttpGet("GetTotalCount")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            var totalCount = _paymentService.GetTotalCount();
            return Ok(totalCount);
        }

        //[HttpGet("GetAllSuccessfulPayments")]
        //public async Task<ActionResult<Payment>> GetAllSuccessfulPaymentsWithPaging(int currentPage, int pageSize)
        //{
        //    try
        //    {
        //        var result = _paymentService.GetAllSuccessfulPaymentsWithPaging(currentPage, pageSize);

        //        var successfulPaymentResult = new SuccessfulPaymentResult
        //        {
        //            Payments = result.Payments.Select(p => new Payment
        //            {
        //                Id = p.Id,
        //                OrderId = p.OrderId,
        //                Status = p.Status,
        //                Amount = p.Amount,
        //                Method = p.Method,
        //            }).ToList(),
        //            TotalAmount = result.TotalAmount
        //        };
        //        return Ok(successfulPaymentResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpGet("GetTotalAmountOfSuccessfulPayments")] 
        public async Task<ActionResult<double>> GetTotalAmountOfSuccessfulPayments()
        {
            try
            {
                var totalAmount = _paymentService.GetTotalAmountOfSuccessfulPayments();
                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPaymentById/{id}")]
        public async Task<ActionResult<PaymentModel>> GetPaymentById(Guid id, [FromQuery] FormSearch search)
        {
            var payment = _paymentService.GetDetailOne(id, search.currentPage, search.pageSize);
            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }
            return Ok(payment);
        }

        [HttpPost("AddPayment")]
        public async Task<ActionResult<PaymentCreateRequestModel>> AddPayment([FromBody] PaymentCreateRequestModel payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment is null.");
            }

            try
            {
                await _paymentService.Add(payment);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdatePayment")]
        public async Task<ActionResult<PaymentUpdateRequestModel>> UpdatePayment(Guid id, [FromBody] PaymentUpdateRequestModel payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment is null.");
            }

            if (id != payment.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingPayment = await _paymentService.GetOne(id);
            if (existingPayment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }

            try
            {
                await _paymentService.Update(payment);
                var updatedPayment = await _paymentService.GetOne(id); // Fetch the updated entity
                return CreatedAtAction(nameof(GetPaymentById), new { id = updatedPayment.Id }, updatedPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeletePayment/{id}")]
        public async Task<ActionResult> DeletePayment(Guid id)
        {
            var payment = await _paymentService.GetOne(id);
            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }

            try
            {
                await _paymentService.Delete(id);
                return Ok($"Payment with ID {id} was successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPost("PaymentCallBack")]
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return Ok(response);
            }


            // Lưu đơn hàng vô database

            return Ok(response);
        }
        [HttpPost("GetAAmountAndCountForDay")]
        public async Task<ActionResult<AmountAndCountForDay>> getAAmountAndCountForDay([FromQuery] string day, [FromQuery] string month, [FromQuery] string year)
        {
            if (day.Length > 2 || month.Length > 2 || year.Length > 4 || day.Length == 0 || month.Length == 0 || year.Length == 0)
            {
                return BadRequest("Invalid date");
            }
            if(day.Length == 1) day = 0 + day;
            if (month.Length == 1) month = 0 + month;
            string date = day + "/" + month + "/" + year;
            date = String.Format(date, "dd/mm/yyyy");
            var payment = _paymentService.getAAmountAndCountForDay(String.Format(date, "dd/mm/yyyy"));
            if (payment == null)
            {
                return NotFound($"Payment in date {day + "/" + month + "/" + year} not found.");
            }
            return Ok(payment.Result);
        }
    }

}

using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftServerAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace JeanCraftServerAPI.Controllers
{
    [ApiController]
    [Route("api/Payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        private static readonly HttpClient client = new HttpClient();
        private const string AccessKey = "F8BBA842ECF85";
        private const string SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentModel requestModel)
        {
            var request = new CollectionLinkRequest
            {
                orderInfo = "pay with MoMo",
                partnerCode = "MOMO",
                redirectUrl = "https://www.linkedin.com/in/danggkhoaaaa/",
                ipnUrl = "https://localhost:7123/api/Payment/ipn",
                amount = (long)requestModel.Amount,
                orderId = requestModel.OrderId.ToString(),
                requestId = Guid.NewGuid().ToString(),
                requestType = "payWithMethod",
                extraData = "",
                partnerName = "ToTe Payment",
                storeId = "test",
                orderGroupId = "",
                autoCapture = true,
                lang = "vi"
            };

            var rawSignature = $"accessKey={AccessKey}&amount={request.amount}&extraData={request.extraData}&ipnUrl={request.ipnUrl}&orderId={request.orderId}&orderInfo={request.orderInfo}&partnerCode={request.partnerCode}&redirectUrl={request.redirectUrl}&requestId={request.requestId}&requestType={request.requestType}";
            request.signature = GetSignature(rawSignature, SecretKey);

            var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
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
            var signature = GetSignature(rawData, SecretKey);

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
        public async Task<ActionResult<IEnumerable<PaymentModel>>> GetAllPayments([FromQuery] FormSearch search)
        {
            var payments = _paymentService.GetAllPaging(search.currentPage, search.pageSize);
            if (payments == null || payments.Count == 0)
            {
                return NotFound("No payments found.");
            }
            return Ok(payments);
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
    }
}

using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model;
using JeanCraftLibrary;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;
using JeanCraftLibrary.Repositories;
using System.Globalization;
using JeanCraftServerAPI.Util;
using Microsoft.EntityFrameworkCore;
using JeanCraftLibrary.Model.Response;

namespace JeanCraftServerAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(PaymentCreateRequestModel paymentModel)
        {
            try
            {
                paymentModel.Id = Guid.NewGuid();
                var Entity = _mapper.Map<Payment>(paymentModel);

                var repos = _unitOfWork.PaymentRepository;

                // Ensure the generated ID is unique
                Guid newGuid;
                do
                {
                    newGuid = Guid.NewGuid();
                    var exist = await repos.FindAsync(newGuid);
                    if (exist == null)
                    {
                        break;
                    }
                } while (true);

                Entity.Id = newGuid;

                await repos.AddAsync(Entity);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task Delete(Guid paymentId)
        {
            try
            {
                var paymentRepository = _unitOfWork.PaymentRepository;

                var paymentEntity = await paymentRepository.GetAsync(a => a.Id == paymentId);
                if (paymentEntity == null)
                {
                    throw new KeyNotFoundException("Order not found");
                }

              

                // Delete the order itself
                await paymentRepository.DeleteAsync(paymentEntity);

                // Commit the transaction
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        //public SuccessfulPaymentResult GetAllSuccessfulPaymentsWithPaging(int currentPage, int pageSize)
        //{
        //    try
        //    {
        //        Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null;
        //        Expression<Func<Payment, bool>> filter = p => p.Status == "Thành công"; 

        //        var payments = _unitOfWork.PaymentRepository.GetDetail(filter, orderBy, "", currentPage, pageSize).ToList();

        //        double totalAmount = (double)payments.Sum(p => p.Amount);

        //        return new SuccessfulPaymentResult
        //        {
        //            Payments = payments,
        //            TotalAmount = totalAmount
        //        };
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public async Task<IList<PaymentModel>> GetAll()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();
            return _mapper.Map<IList<PaymentModel>>(payments);
        }

        public Payment GetDetailOne(Guid id, int currentPage, int pageSize)
        {
            Expression<Func<Payment, bool>> filter = order => order.Id == id;
            Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null;
            var Entity = _unitOfWork.PaymentRepository.GetDetail(filter, orderBy, "", currentPage, pageSize).FirstOrDefault();
            return Entity;
        }
        public PaymentResult GetAllPaging(int currentPage, int pageSize)
        {
            try
            {
                var query = _unitOfWork.PaymentRepository.GetDetail(null, null, "").OrderByDescending(x => revertDate(x.Status));

                var totalCount = query.Count();
                var totalAmount = query.Sum(payment => payment.Amount);

                // Lấy dữ liệu cho trang hiện tại
                var payments = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Tính tổng số trang
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return new PaymentResult
                {
                    Payments = payments,
                    TotalAmount = (double)totalAmount,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<PaymentModel> GetOne(Guid paymentId)
        {
            var Entity = await _unitOfWork.PaymentRepository.FindAsync(paymentId);
            return _mapper.Map<PaymentModel>(Entity);
        }

        public async Task Update(PaymentUpdateRequestModel paymentModel)
        {
            try
            {
                var repos = _unitOfWork.PaymentRepository;
                var existingPayment = await repos.FindAsync(paymentModel.Id);
                if (existingPayment == null)
                    throw new KeyNotFoundException();
                existingPayment.Id = paymentModel.Id;
                existingPayment.Status = paymentModel.Status;
                existingPayment.Amount = paymentModel.Amount;
               

                _mapper.Map(existingPayment, paymentModel);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public double GetTotalAmountOfSuccessfulPayments() 
        {
            try
            {
                Expression<Func<Payment, bool>> filter = p => p.Status.Contains("Success");
                var payments = _unitOfWork.PaymentRepository.GetDetail(filter).ToList();
                return (double)payments.Sum(p => p.Amount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double GetTotalAmountOfPayments()
        {
            try
            {
                return (double)_unitOfWork.PaymentRepository.GetDetail(null, null, "").Sum(payment => payment.Amount);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetTotalCount()
        {
            try
            {
                return _unitOfWork.PaymentRepository.GetDetail(null, null, "").Count();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private String revertDate(String date) 
        {
            var parts = date.Split(' ');
            var datePart = parts[2];
            var timePart = parts[1];
            DateTime parsedDate = DateTime.ParseExact(datePart + " " + timePart, "d/M/yyyy HH:mm", CultureInfo.InvariantCulture);
            string outputDateString = parsedDate.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
            return outputDateString;
        }

        public async Task<AmountAndCountForDay> getAAmountAndCountForDay(String date)
        {
            AmountAndCountForDay x = new AmountAndCountForDay();
            try
            {
                x.Amount = (double)_unitOfWork.PaymentRepository.GetDetail(null, null, "").Where(x => x.Status.Contains(date)).Sum(payment => payment.Amount);
                x.Date = date;
                x.Count = (int)_unitOfWork.PaymentRepository.GetDetail(null, null, "").Where(x => x.Status.Contains(date)).Count();
            }
            catch (Exception e)
            {
                throw e;
            }
            return x;
        }
    }
}

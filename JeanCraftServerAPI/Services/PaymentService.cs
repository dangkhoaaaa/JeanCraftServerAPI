using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model;
using JeanCraftLibrary;
using JeanCraftServerAPI.Services.Interface;
using System.Linq.Expressions;
using JeanCraftLibrary.Repositories;

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


        public async Task<IList<PaymentModel>> GetAll()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync();
            return _mapper.Map<IList<PaymentModel>>(payments);
        }

        public PaymentModel GetDetailOne(Guid id, int currentPage, int pageSize)
        {
            Expression<Func<Payment, bool>> filter = order => order.Id == id;
            Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null;
            var Entity = _unitOfWork.PaymentRepository.GetDetail(filter, orderBy, "Orders", currentPage, pageSize).FirstOrDefault();
            return _mapper.Map<PaymentModel>(Entity);
        }
        public IList<PaymentModel> GetAllPaging(int currentPage, int pageSize)
        {

            Func<IQueryable<Payment>, IOrderedQueryable<Payment>> orderBy = null;
            var Entity = _unitOfWork.PaymentRepository.GetDetail(null, orderBy, "Orders", currentPage, pageSize);
            return _mapper.Map<IList<PaymentModel>>(Entity);
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

    }
}

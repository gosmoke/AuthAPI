using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.Common;
using Auth.Data;
using Auth.Entities;
using Auth.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoggingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(Models.AppException exception)
        {
            using (var scopeContext = _unitOfWork.Create())
            {
                var entity = _mapper.Map<Entities.AppException>(exception);
                _unitOfWork.LoggingRepository.Save(entity);

                await scopeContext.SaveChangesAsync();
            }
        }

        public async Task<List<Models.AppException>> GetAllAsync(Severity severity)
        {
            using (var scopeContext = _unitOfWork.Create())
            {
                var entities = await _unitOfWork.LoggingRepository.GetAll().Where(a => a.Severity == (int)severity).ToListAsync();

                return _mapper.Map<List<Models.AppException>>(entities);
            }
        }
    }
}

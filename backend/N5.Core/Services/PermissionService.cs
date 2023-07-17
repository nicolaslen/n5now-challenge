using N5.Core.Domain.Events;
using N5.Core.DTOs;
using N5.Core.Messaging;
using N5.Core.Shared;
using N5.Shared.Pagination;
//using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N5.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        /*private readonly ElasticClient _client;
        private readonly IKafkaProducer _kafkaProducer;*/
        private const string Topic = "permission-topic";

        public PermissionService(
            IUnitOfWork unitOfWork/*,
            ElasticClient client,
            IKafkaProducer kafkaProducer*/)
        {
            _unitOfWork = unitOfWork;
            /*_client = client;
            _kafkaProducer = kafkaProducer;*/
        }

        public async Task<bool> Request(PermissionDto permission)
        {
            await _unitOfWork.PermissionRepository.InsertAsync(permission);
            var commitResult = await _unitOfWork.CommitAsync();
            if (commitResult != 1)
            {
                throw new Exception();
            }

            /*var indexResponse = await _client.IndexDocumentAsync(permission);
            await _kafkaProducer.PublishEvent(Topic, new PermissionRequestedEvent("request"));*/
            return true;
        }

        public async Task<bool> Modify(PermissionDto permission)
        {
            _unitOfWork.PermissionRepository.Update(permission);
            var commitResult = await _unitOfWork.CommitAsync();
            if (commitResult != 1)
            {
                throw new Exception();
            }

            //await _kafkaProducer.PublishEvent(Topic, new PermissionRequestedEvent("modify"));
            return true;
        }

        public async Task<PagedResult<PermissionDto>> Get(PaginationParams paginationParams)
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAsync(null, 
                queryable => queryable.OrderBy(permission => permission.Id), 
                paginationParams, 
                "Tipo");

            //await _kafkaProducer.PublishEvent(Topic, new PermissionRequestedEvent("get"));
            return permissions;
        }

        public async Task<IEnumerable<PermissionTypeDto>> GetTypes()
        {
            var types = await _unitOfWork.PermissionTypeRepository.GetAllAsync();
            return types;
        }
    }
}
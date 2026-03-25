using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.Farmers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

public class FarmerAppService : ArilyAppService, IFarmerAppService
{
    private readonly IRepository<Farmer, Guid> _farmerRepository;

    public FarmerAppService(IRepository<Farmer, Guid> farmerRepository)
    {
        _farmerRepository = farmerRepository;
    }

    public async Task<FarmerDto> GetAsync(Guid id)
    {
        var farmer = await _farmerRepository.GetAsync(id);
        return ObjectMapper.Map<Farmer, FarmerDto>(farmer);
    }

    public async Task<PagedResultDto<FarmerDto>> GetListAsync(GetFarmerListInput input)
    {
        var query = await _farmerRepository.GetQueryableAsync();

        query = query
            .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                x => x.FullName.Contains(input.Filter!) ||
                     x.Code.Contains(input.Filter!) ||
                     x.PhoneNumber.Contains(input.Filter!) ||
                     (x.NickName != null && x.NickName.Contains(input.Filter!))
            )
            .WhereIf(input.Status.HasValue, x => x.Status == input.Status!.Value)
            .WhereIf(!input.ProvinceCode.IsNullOrWhiteSpace(), x => x.ProvinceCode == input.ProvinceCode);

        var totalCount = query.Count();

        var farmers = query
            .OrderBy(x => x.FullName)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        return new PagedResultDto<FarmerDto>(
            totalCount,
            ObjectMapper.Map<List<Farmer>, List<FarmerDto>>(farmers)
        );
    }

    public async Task<FarmerDto> CreateAsync(CreateUpdateFarmerDto input)
    {
        var farmer = new Farmer(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            input.Code,
            input.FullName,
            input.PhoneNumber
        );

        farmer.NickName = input.NickName;
        farmer.ProvinceCode = input.ProvinceCode;
        farmer.DistrictCode = input.DistrictCode;
        farmer.WardCode = input.WardCode;
        farmer.Address = input.Address;
        farmer.Latitude = input.Latitude;
        farmer.Longitude = input.Longitude;
        farmer.Note = input.Note;
        farmer.Status = input.Status;

        await _farmerRepository.InsertAsync(farmer);

        return ObjectMapper.Map<Farmer, FarmerDto>(farmer);
    }

    public async Task<FarmerDto> UpdateAsync(Guid id, CreateUpdateFarmerDto input)
    {
        var farmer = await _farmerRepository.GetAsync(id);

        farmer.Code = input.Code;
        farmer.FullName = input.FullName;
        farmer.NickName = input.NickName;
        farmer.PhoneNumber = input.PhoneNumber;
        farmer.ProvinceCode = input.ProvinceCode;
        farmer.DistrictCode = input.DistrictCode;
        farmer.WardCode = input.WardCode;
        farmer.Address = input.Address;
        farmer.Latitude = input.Latitude;
        farmer.Longitude = input.Longitude;
        farmer.Note = input.Note;
        farmer.Status = input.Status;

        await _farmerRepository.UpdateAsync(farmer);

        return ObjectMapper.Map<Farmer, FarmerDto>(farmer);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _farmerRepository.DeleteAsync(id);
    }
}

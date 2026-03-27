using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arily.Crm.FarmerGardens;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Arily.Crm;

[RemoteService(IsEnabled = false)]
public class FarmerGardenAppService : ArilyAppService, IFarmerGardenAppService
{
    private readonly IRepository<FarmerGarden, Guid> _gardenRepository;
    private readonly IRepository<Farmer, Guid> _farmerRepository;

    public FarmerGardenAppService(
        IRepository<FarmerGarden, Guid> gardenRepository,
        IRepository<Farmer, Guid> farmerRepository)
    {
        _gardenRepository = gardenRepository;
        _farmerRepository = farmerRepository;
    }

    public async Task<FarmerGardenDto> GetAsync(Guid farmerId, Guid id)
    {
        var garden = await _gardenRepository.GetAsync(x => x.Id == id && x.FarmerId == farmerId);
        return ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(garden);
    }

    public async Task<ListResultDto<FarmerGardenDto>> GetListAsync(Guid farmerId)
    {
        var query = await _gardenRepository.GetQueryableAsync();

        var gardens = query
            .Where(x => x.FarmerId == farmerId)
            .OrderBy(x => x.GardenName)
            .ToList();

        return new ListResultDto<FarmerGardenDto>(
            ObjectMapper.Map<List<FarmerGarden>, List<FarmerGardenDto>>(gardens)
        );
    }

    public async Task<FarmerGardenDto> CreateAsync(Guid farmerId, CreateUpdateFarmerGardenDto input)
    {
        await _farmerRepository.GetAsync(farmerId);

        var garden = new FarmerGarden(
            GuidGenerator.Create(),
            CurrentTenant.Id,
            farmerId,
            input.ProductCategoryId,
            input.GardenName
        );

        garden.AreaHectare = input.AreaHectare;
        garden.EstimatedYieldKg = input.EstimatedYieldKg;
        garden.Latitude = input.Latitude;
        garden.Longitude = input.Longitude;
        garden.Address = input.Address;
        garden.Note = input.Note;
        garden.Status = input.Status;

        await _gardenRepository.InsertAsync(garden);

        return ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(garden);
    }

    public async Task<FarmerGardenDto> UpdateAsync(Guid farmerId, Guid id, CreateUpdateFarmerGardenDto input)
    {
        var garden = await _gardenRepository.GetAsync(x => x.Id == id && x.FarmerId == farmerId);

        garden.ProductCategoryId = input.ProductCategoryId;
        garden.GardenName = input.GardenName;
        garden.AreaHectare = input.AreaHectare;
        garden.EstimatedYieldKg = input.EstimatedYieldKg;
        garden.Latitude = input.Latitude;
        garden.Longitude = input.Longitude;
        garden.Address = input.Address;
        garden.Note = input.Note;
        garden.Status = input.Status;

        await _gardenRepository.UpdateAsync(garden);

        return ObjectMapper.Map<FarmerGarden, FarmerGardenDto>(garden);
    }

    public async Task DeleteAsync(Guid farmerId, Guid id)
    {
        await _gardenRepository.DeleteAsync(x => x.Id == id && x.FarmerId == farmerId);
    }
}

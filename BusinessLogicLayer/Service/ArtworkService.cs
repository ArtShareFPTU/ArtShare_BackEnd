using AutoMapper;
using BusinessLogicLayer.IService;
using DataAccessLayer.BussinessObject.IRepository;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.BussinessObject;
using ModelLayer.DTOS.Request.Artwork;
using ModelLayer.DTOS.Response;

namespace BusinessLogicLayer.Service;

public class ArtworkService : IArtworkService
{
    private readonly IArtworkRepository _ArtworkRepository;
    private readonly IMapper _mapper;
    public ArtworkService(IArtworkRepository ArtworkRepository, IMapper mapper)
    {
        _ArtworkRepository = ArtworkRepository;
        _mapper = mapper;
    }

    public async Task<List<Artwork>> GetAllArtworkAsync()
    {
        return await _ArtworkRepository.GetAllArtworkAsync();
    }

    public async Task<Artwork> GetArtworkByIdAsync(Guid id)
    {
        return await _ArtworkRepository.GetArtworkByIdAsync(id);
    }

    public async Task<IActionResult> AddArtworkAsync(ArtworkCreation Artwork)
    {
        return await _ArtworkRepository.AddArtworkAsync(Artwork);
    }

    public async Task<IActionResult> UpdateArtworkAsync(ArtworkUpdate Artwork)
    {
        return await _ArtworkRepository.UpdateArtworkAsync(Artwork);
    }

    public async Task<IActionResult> DeleteArtworkAsync(Guid id)
    {
        return await _ArtworkRepository.DeleteArtworkAsync(id);
    }

    public async Task<List<ArtworkRespone>> GetArtworkByArtistId(Guid artistId)
    {
        var response = await _ArtworkRepository.GetArtworkByArtistId(artistId);
        return _mapper.Map<List<ArtworkRespone>>(response);
    }
    public async Task<IActionResult> UpdateCategory4Artwork(ArtworkCategoryUpdate artworkCategoryUpdate)
    {
        return await _ArtworkRepository.UpdateCategory4Artwork(artworkCategoryUpdate);
    }
    public async Task<IActionResult> AddCategory4Artwork(ArtworkCategoryAddition artworkCategoryAddition)
    {
        return await _ArtworkRepository.AddCategory4Artwork(artworkCategoryAddition);
    }
    public async Task<IActionResult> UpdateTag4Artwork(ArtworkTagUpdate artworkTagUpdate)
    {
        return await _ArtworkRepository.UpdateTag4Artwork(artworkTagUpdate);
    }
    public async Task<IActionResult> AddTag4Artwork(ArtworkTagAddition artworkTagAddition)
    {
        return await _ArtworkRepository.AddTag4Artwork(artworkTagAddition);
    }
    public async Task<IActionResult> RemoveTag4Artwork(Guid id)
    {
        return await _ArtworkRepository.RemoveTag4Artwork(id);
    }
    public async Task<IActionResult> RemoveCategory4Artwork(Guid id)
    {
        return await _ArtworkRepository.RemoveCategory4Artwork(id);
    }
}
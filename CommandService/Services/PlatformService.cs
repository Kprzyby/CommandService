using CommandService.Data.DTOs.Platform;
using CommandService.Data.Entities;
using CommandService.Data.Repos;
using CommandService.Helpers;
using X.PagedList;

namespace CommandService.Services
{
    public class PlatformService
    {
        #region Constructors

        public PlatformService(PlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        #endregion Constructors

        #region Properties

        private readonly PlatformRepo _platformRepo;

        #endregion Properties

        #region Methods

        public async Task<ReadPlatformDTO> GetPlatformAsync(int id)
        {
            try
            {
                Platform platform = await _platformRepo.GetPlatformByIdAsync(id);

                if (platform == null)
                {
                    return null;
                }

                ReadPlatformDTO dto = new ReadPlatformDTO()
                {
                    Id = platform.Id,
                    Name = platform.Name
                };

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReadPlatformsResponseDTO> GetPlatformsAsync(PlatformFilteringDTO filteringDTO)
        {
            try
            {
                IQueryable<Platform> platforms = _platformRepo.GetPlatforms();

                if (String.IsNullOrEmpty(filteringDTO.NameFilterValue) == false)
                {
                    platforms = platforms.
                        Where(p => p.Name
                        .ToUpper()
                        .StartsWith(filteringDTO.NameFilterValue.ToUpper()));
                }

                if (filteringDTO.SortInfo.HasValue == false)
                {
                    platforms.OrderBy(p => p.Name);
                }
                else
                {
                    List<KeyValuePair<string, string>> sortInfo = new List<KeyValuePair<string, string>>();

                    sortInfo.Add((KeyValuePair<string, string>)filteringDTO.SortInfo);

                    platforms = SortingHelper<Platform>.Sort(platforms, sortInfo);
                }

                ReadPlatformsResponseDTO result = new ReadPlatformsResponseDTO();

                result.TotalCount = platforms.Count();
                result.Platforms = await platforms
                    .Select(platform => new ReadPlatformDTO()
                    {
                        Id = platform.Id,
                        Name = platform.Name
                    })
                    .ToPagedListAsync(filteringDTO.PageNumber, filteringDTO.PageSize);
                result.PageSize = filteringDTO.PageSize;
                result.PageNumber = filteringDTO.PageNumber;
                result.SortInfo = filteringDTO.SortInfo;
                result.NameFilterValue = filteringDTO.NameFilterValue;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ReadPlatformDTO> AddPlatformAsync(CreatePlatformDTO dto)
        {
            try
            {
                Platform platform = new Platform()
                {
                    Name = dto.Name,
                    ExternalId = dto.ExternalId
                };

                platform = await _platformRepo.AddPlatformAsync(platform);

                ReadPlatformDTO result = new ReadPlatformDTO()
                {
                    Id = platform.Id,
                    Name = platform.Name,
                };

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdatePlatformAsync(UpdatePlatformDTO dto)
        {
            try
            {
                Platform oldPlatform = await _platformRepo.
                    GetPlatformByExternalIdAsync(dto.ExternalId);

                if (oldPlatform == null)
                {
                    return false;
                }

                oldPlatform.Name = dto.Name;

                await _platformRepo.UpdatePlatformAsync(oldPlatform);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemovePlatformByExternalIdAsync(int id)
        {
            try
            {
                Platform platform = await _platformRepo.
                    GetPlatformByExternalIdAsync(id);

                if (platform == null)
                {
                    return false;
                }

                await _platformRepo.RemovePlatformAsync(platform);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion Methods
    }
}
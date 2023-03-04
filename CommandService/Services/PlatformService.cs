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

        #endregion Methods
    }
}
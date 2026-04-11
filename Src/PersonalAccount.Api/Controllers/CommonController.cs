using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Domain.Core.Interfaces;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILoadingService _loadService;

        public CommonController(ILoadingService loadingService)
            => _loadService = loadingService;

        /// <summary>
        /// Получить версию
        /// </summary>
        /// <returns></returns>
        [HttpGet("version")]
        public IActionResult GetVersion() => Ok("1.0.0");

        /// <summary>
        /// Отправить данные.
        /// </summary>
        /// <returns></returns>
        [HttpPost("push")]
        public IActionResult PushData(Organisation company, IEnumerable<DtoJournalEntry> entries, CancellationToken token)
        {
            var result = _loadService.Push(company, entries, CancellationToken.None);
            
            return Ok(result);
        }
    }
}

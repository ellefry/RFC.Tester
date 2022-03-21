using BHSW2_2.Pinion.DataService.AppServices.Dtos;
using BHSW2_2.Pinion.DataService.AppServices.Interfaces;
using BHSW2_2.Pinion.DataService.Clients.Dtos;
using BHSW2_2.Pinion.DataService.Extensions;
using BHSW2_2.Pinion.DataService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BHSW2_2.Pinion.DataService.AppServices;
using System.IO;
using Newtonsoft.Json;
using RFC.Common;
using System;
using SapWebService.Common.Abstracts;

namespace BHSW2_2.Pinion.DataService.Controllers
{
    [ApiController]
    [Route("api/saps")]
    public class SapController : ControllerBase
    {
        private readonly ISapRequestAppService _sapRequestAppService;
        private readonly ISapSwitcher _sapSwitcher;

        private readonly IOutboundService _outboundService;

        public SapController(ISapRequestAppService sapRequestAppService, ISapSwitcher sapSwitcher, 
            IOutboundService outboundService)
        {
            _sapRequestAppService = sapRequestAppService;
            _sapSwitcher = sapSwitcher;
            _outboundService = outboundService;
        }

        [HttpPost("FinishPart")]
        public async Task<ResponseResult> FinishPart(FinishPartInput input)
        {
            await _sapRequestAppService.FinishPartAsync(input);
            return new ResponseResult();
        }

        [HttpPost("ScrapPart")]
        public async Task<ResponseResult> ScrapPart(ScrapPartInput input)
        {
            await _sapRequestAppService.ScrapPartAsync(input);
            return new ResponseResult();
        }

        [HttpGet("")]
        public async Task<List<SapRequest>> GetSapRequests([FromQuery] GetSapRequestsInput input)
        {
            return await _sapRequestAppService.GetSapRequests(input);
        }

        [HttpPost("ReSend")]
        public async Task ReSendSapRequest([FromBody] ReSendSapRequestInput input)
        {
            await _sapRequestAppService.ReSendSapRequest(input);
        }

        [HttpGet("switcher")]
        public async Task<bool> GetSapSwitcher()
        {
            return await Task.FromResult(_sapSwitcher.IsEnabled);
        }

        [HttpPost("switcher/{enabled}")]
        public async Task GetSapSwitcher(bool enabled)
        {
            _sapSwitcher.IsEnabled = enabled;
            await Task.CompletedTask;
        }

        [HttpGet("Histories")]
        public async Task<List<SapRequestHistory>> GetSapHistories()
        {
            return await _sapRequestAppService.GetSapHistories();
        }

        [HttpGet("Transfer")]
        public async Task<IActionResult> Tranfer()
        {
            await _outboundService.Transfer();
            return Ok();
        }
    }
}

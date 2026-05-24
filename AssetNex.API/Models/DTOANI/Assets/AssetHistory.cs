using AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI;
using Microsoft.AspNetCore.Mvc;
using System;
using static Dropbox.Api.TeamLog.TimeUnit;

namespace AssetNexAPI.AssetNexITAPI.AssetNex.API.Models.DTOANI.Assets
{
    public class AssetHistoryCreateDto
    {

        public int AssetId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string EventType { get; set; }

        public int ReferenceTicketId { get; set; }

        public string Remarks { get; set; }

        public string Vendor { get; set; }
        public int CostIncurred { get; set; }



    } }

using AssetNexAPI.AssetNexITAPI.AssetNex.API.ControllerANI;
using IdentityModel;
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
//}

//| Entity / Controller                           | Needs DTO ?                | Reason                                                               |
//| --------------------------------------------- | ------------------------- | -------------------------------------------------------------------- |
//| **AssetsMasterController**                    | ❌ Not required(optional) | Entity is simple, no risk accepting full model                       |
//| **UsersController**                           | ❌ Not required (optional) | Safe to expose directly for CRUD                                     |
//| **SupportTicketsController**                  | ❌ Not required (optional) | Can be directly used unless attachments later                        |
//| **AssetRequestsController**                   | ✅ YES                     | Request lifecycle changes → internal fields must be protected        |
//| **AssetAssignmentsController**                | ✅ YES                     | User & Asset mapping should not be modified directly                 |
//| **SoftwareAssignController (Asset_Software)** | ✅ YES                     | Many-to-many mapping requires controlled input                       |
//| **AssetsHistoryController**                   | ⚠ HIGHLY Recommended      | Must be system generated, not via client — use only POST from system |
//| **Warranty (not priority for 12 hours)**      | ❌ Skip for now            | Not included in your required API list                               |

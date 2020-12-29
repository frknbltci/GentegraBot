using Genbot.BLL.Repository.Service;
using Genbot.UI.Attribute;
using Genbot.UI.PYService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Genbot.UI.Controllers
{
    public class BaseController : Controller
    {
        protected EntityService service = new EntityService();
        protected LicenceCheck licheck = new LicenceCheck();
        protected TySearchService tyservice = new TySearchService();
        protected N11SearchService n11Service = new N11SearchService();
        protected GGSearchService ggService = new GGSearchService();
        protected MHPSearchService mhpService = new MHPSearchService();
        protected HBSearchService hbService = new HBSearchService();
        protected TYSearchService2 tyService2 = new TYSearchService2();
        

    }
}
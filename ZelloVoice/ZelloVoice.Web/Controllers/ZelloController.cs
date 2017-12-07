using PttLib;
using AxPttLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ZelloVoiceWeb.Controllers;

using Trimble.MobileSolutions.Business.Entities.Assets;
using Trimble.MobileSolutions.Business.Entities.Events;
using Trimble.MobileSolutions.Business.Entities.Security;
using Trimble.MobileSolutions.Data.Mappers.Assets;
using Trimble.MobileSolutions.Data.Mappers.Security;
using BusinessAssets = Trimble.MobileSolutions.Business.Entities.Assets;
using Trimble.MobileSolutions.Business.Entities.Events.RealTime.AssetInfo;
//using Trimble.MobileSolutions.Map.Objects;
//using Trimble.MobileSolutions.Map.ConnectedFleet.ALKMaps.JsModel;
using Newtonsoft.Json;


namespace ZelloVoice.Web.Controllers
{

    
    

    [AllowAnonymous]
    public class ZelloController : Controller
    {



       // AssetService.AssetWS AssetWS = new AssetService.AssetWS();
       
         // GET: Zello
         [AllowAnonymous]
        public ActionResult Index()
        {          

            try
            {

                //ISecurity mapper = Trimble.MobileSolutions.Data.Mappers.Factory.GetMapper<ISession, ISecurity>();
                //ISession session = null;
                //session = mapper.GetSession("QA FT", "tester22", "test");
                //result = AssetWS.GetAssets(Test)


            }
            catch (Exception ex)
            {

               
              
            }
            
            return View();
        }

       
    }
}
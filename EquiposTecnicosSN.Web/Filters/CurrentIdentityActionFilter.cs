using Salud.Security.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace EquiposTecnicosSN.Web.Filters
{
    /// <summary>
    /// Filtro que valida la autenticacion con el SSO y agrega el nombre del usuario logueado en el ViewBag de cada pagina.
    /// </summary>
    public class CurrentIdentityActionFilter : System.Web.Mvc.ActionFilterAttribute
    {
        private string currentIdFullName { get; set; }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            SSOHelper.Authenticate();
            if (SSOHelper.CurrentIdentity == null)
            {
                string ssoUrl = SSOHelper.Configuration["SSO_URL"] as string;
                filterContext.RequestContext.HttpContext.Response.Redirect(ssoUrl + "/Login.aspx");
            }

            filterContext.Controller.ViewBag.CurrentIdentity = (SSOHelper.CurrentIdentity != null ? SSOHelper.CurrentIdentity.Fullname : "Usuario Anónimo");
            //filterContext.Controller.ViewBag.CurrentIdentity = "Usuario Anónimo";


        }

    }
}
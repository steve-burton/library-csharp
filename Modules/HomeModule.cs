using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace Catalog
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => View["index.cshtml"];
    }
  }
}

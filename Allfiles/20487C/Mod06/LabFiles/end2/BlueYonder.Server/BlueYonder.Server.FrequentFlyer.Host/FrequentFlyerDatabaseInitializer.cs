using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.DataAccess;

namespace BlueYonder.FrequentFlyerService.Host
{
    public class FrequentFlyerDatabaseInitializer : DropCreateDatabaseIfModelChanges<FrequentFlyersContext>
    {
        protected override void Seed(FrequentFlyersContext context)
        {
            base.Seed(context);
        }
    }
}

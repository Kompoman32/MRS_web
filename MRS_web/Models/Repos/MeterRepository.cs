using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS_web.Models.EDM;

namespace MRS_web.Models.Repos
{
    public class MeterRepository
    {
        private ModelContainer1  cont;

        public MeterRepository(ModelContainer1 _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Meter> Meters()
        {
            return (from m in cont.MeterSet orderby m.ProductionId select m);
        }

        public Meter GetMeter(long id)
        {
            return cont.MeterSet.Find(id);
        }

        //TODO: добавление удаление edit
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS_web.Models.EDM;

namespace MRS_web.Models.Repos
{
    public class InstMeterRepository
    {
        private ModelContainer1  cont;

        public InstMeterRepository(ModelContainer1 _cont)
        {
            cont = _cont;
        }

        public IEnumerable<InstalledMeter> InstMaterss()
        {
            return (from m in cont.MeterSet orderby m.ProductionId select m as InstalledMeter);
        }

        public InstalledMeter GetInstMeter(long id)
        {
            Meter met = cont.MeterSet.Find(id);
            return met as InstalledMeter;
        }

        public void EditMeter(long meterId, InstalledMeter.Fields fieldToEdit, string value)
        {
            InstalledMeter met = GetInstMeter(meterId);

            if (met == null)
                return;

            switch (fieldToEdit)
            {
                case InstalledMeter.Fields.ExpirationDate:
                    if (DateTime.TryParse(value, out DateTime dtVal) && met.ExpirationDate != dtVal)
                        met.ExpirationDate = dtVal;
                    break;
                default:
                    throw new NotImplementedException();
            }

            cont.SaveChanges();
        }
        
        //TODO: добавление
    }
}
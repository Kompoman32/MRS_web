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

        public void Edit(long meterId, Meter.Fields fieldToEdit, string value)
        {
            Meter met = GetMeter(meterId);

            switch (fieldToEdit)
            {
                case Meter.Fields.Name:
                    if (met.Name != value)
                        met.Name = value;
                    break;
                case Meter.Fields.Discription:
                    if (met.Discription != value)
                        met.Discription = value;
                    break;
                case Meter.Fields.SumReadings:
                  { if (double.TryParse(value, out double doubleVal) && met.SumReadings != doubleVal)
                        met.SumReadings = doubleVal;}
                    break;

                case Meter.Fields.Capacity:
                  { if (double.TryParse(value, out double doubleVal) && met.Capacity != doubleVal)
                        met.Capacity = doubleVal;}
                    break;

                case Meter.Fields.ProductionDate:
                    if (DateTime.TryParse(value, out DateTime dtValue) && met.ProductionDate != dtValue)
                        met.ProductionDate = dtValue;
                    break;

                default:
                    throw new NotImplementedException();
            }

            cont.SaveChanges();
        }

        public void Edit(long meterId, InstalledMeter.Fields fieldToEdit, string value)
        {
            InstalledMeter met = GetMeter(meterId) as InstalledMeter;

            if (met ==null)
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
        //TODO: добавление удаление
    }
}
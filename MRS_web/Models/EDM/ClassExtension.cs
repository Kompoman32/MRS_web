namespace MRS_web.Models.EDM
{
    public partial class Document
    {
        public override string ToString()
        {
            return this.SigningDate.ToString("d") + " - " + this.Title;
        }
    }
    public partial class Meter
    {
        public override string ToString()
        {
            return this.ProductionId + " - " + this.Name;
        }
    }

    public partial class Parametr
    {
        public override string ToString()
        {
            return this.Name + " - " + this.Measure;
        }
    }

    public partial class Reading
    {
        public override string ToString()
        {
            return this.Value + " " + Meter.Type.Unit;
        }
    }

    public partial class Tariff
    {
        public override string ToString()
        {
            return this.Name;
        }
    }

    public partial class TimeSpan
    {
        public override string ToString()
        {
            return this.TimeStart.ToString("g") + " - " + this.TimeEnd.ToString("g");
        }
    }

    public partial class Type
    {
        public override string ToString()
        {
            return this.Name + " (" + this.Unit + ")";
        }
    }

    public partial class User
    {
        public override string ToString()
        {
            return this.Login + " - " + this.FullName;

        }
    }
}
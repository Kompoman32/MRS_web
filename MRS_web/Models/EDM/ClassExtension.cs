namespace MRS_web.Models.EDM
{
    public partial class Document
    {
        public enum Fields { Title, Discription, SigningDate, Meter}

        public override string ToString()
        {
            return this.SigningDate.ToString("d") + " - " + this.Title;
        }
    }
    public partial class Meter
    {
        public enum Fields { Name, Discription, SumReadings, Capacity, ProductionId, ProductionDate, Parametrs, Tariff, Type, Documents, User, Readings}

        public override string ToString()
        {
            return this.ProductionId + " - " + this.Name;
        }
    }

    public partial class InstalledMeter
    {
        public new enum Fields { InstallDate, ExpirationDate}
    }

    public partial class Parametr
    {
        public enum Fields { Name, Measure, Meters}

        public override string ToString()
        {
            return this.Name + " - " + this.Measure;
        }
    }

    public partial class Reading
    {
        public enum Fields { Value, TariffNumber, Meter}

        public override string ToString()
        {
            return this.Value + " " + Meter.Type.Unit;
        }
    }

    public partial class Tariff
    {
        public enum Fields { Name, Meters, TimeSpans}

        public override string ToString()
        {
            return this.Name;
        }
    }

    public partial class TimeSpan
    {
        public enum Fields { Name, TimeStart, TimeEnd, Tariff}

        public override string ToString()
        {
            return this.TimeStart.ToString("g") + " - " + this.TimeEnd.ToString("g");
        }
    }

    public partial class Type
    {
        public enum Fields { Name, Unit, Meters}

        public override string ToString()
        {
            return this.Name + " (" + this.Unit + ")";
        }
    }

    public partial class User
    {
        public enum Fields { Login, Password, FullName, AdminPrivileges, Maters}

        public override string ToString()
        {
            return this.Login + " - " + this.FullName;

        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using MainLib;
using MRS_web.Models.EDM;
using MRS_web.Models.Repos;

namespace MRS_web.Models
{
    public class DataManager
    {
        ModelContainer1  cont;

        public UserRepository UserRepo;
        public MeterRepository MetRepo;
        public InstMeterRepository InstMetRepo;
        public ParametrRepository ParRepo;
        public TypeRepository TypeRepo;
        public ReadingRepository ReadRepo;
        public TariffRepository TarRepo;
        public TimeSpanRepository TimeSpanRepo;
        public DocumentRepository DocRepo;


        public DataManager()
        {
            cont = new ModelContainer1();
            UserRepo = new UserRepository(cont);
            MetRepo = new MeterRepository(cont);
            InstMetRepo = new InstMeterRepository(cont);
            ParRepo = new ParametrRepository(cont);
            TypeRepo = new TypeRepository(cont);
            ReadRepo = new ReadingRepository(cont);
            TarRepo = new TariffRepository(cont);
            TimeSpanRepo = new TimeSpanRepository(cont);
            DocRepo = new DocumentRepository(cont);
        }

        //public static IEnumerable<string[,]> GetDataTables(params IEnumerable[] collections)
        //{
        //    if (collections == null) return null;

        //    List<string[,]> array = new List<string[,]>();

        //    foreach (IEnumerable collection in collections)
        //        array.Add(GetDataTable(collection));
            
        //    return array;
        //}

        public static string[,] GetDataTable<T>(IEnumerable<T> collection)
        {
            if (!collection.Any()) return new string[0,0];
            
            string[,] output = null;
            var @switch = new Dictionary<System.Type, Action>()
            {
                {typeof(Meter), () => output = Meter.GetDataTableOfMeters(collection.Cast<Meter>())},
                {typeof(Document), () => output = Document.GetDataTableOfDocuments(collection.Cast<Document>())},
                {typeof(Parametr), () => output = Parametr.GetDataTableOfParametrs(collection.Cast<Parametr>())},
                {typeof(Reading), () => output = Reading.GetDataTableOfReadings(collection.Cast<Reading>())},
                {typeof(Tariff), () => output = Tariff.GetDataTableOfTariffs(collection.Cast<Tariff>())},
                {
                    typeof(EDM.TimeSpan),
                    () => output = EDM.TimeSpan.GetDataTableOfTimeSpans(collection.Cast<EDM.TimeSpan>())
                },
                {typeof(EDM.Type), () => output = EDM.Type.GetDataTableOfTypes(collection.Cast<EDM.Type>())},
                {typeof(User), () => output = User.GetDataTableOfUsers(collection.Cast<User>())},
                {typeof(object), () => output = new string[0,0] }
            };

            @switch[typeof(T)]();

           return output;
        }

        public static void ExportToExcel(string fileName, IEnumerable<string[,]> toExport)
        {
            MainLib.ExportImport.Export.ToExcel($"C:\\c#\\MRS_web\\MRS_web\\Output_Excel\\{fileName}", toExport);
        }
    }
}
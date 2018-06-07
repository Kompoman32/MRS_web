using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}
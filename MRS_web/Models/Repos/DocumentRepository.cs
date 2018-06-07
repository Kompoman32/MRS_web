using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS_web.Models.EDM;

namespace MRS_web.Models.Repos
{
    public class DocumentRepository
    {
        private ModelContainer1  cont;

        public DocumentRepository(ModelContainer1 _cont)
        {
            cont = _cont;
        }

        public IEnumerable<Document> Documents()
        {
            return (from d in cont.DocumentSet orderby d.Title select d);
        }

        public Document GetDocument(int id)
        {
            return cont.DocumentSet.Find(id);
        }

        //TODO: добавление удаление edit
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obavestavac
{
    public class Informacija
    {

        public DateTime DatumObavestenja;
        public DateTime DatumDesavanja;
        public string Naziv;
        public bool Notifikacije;
        public Informacija(DateTime datumobavestenja, DateTime datumdesavanja, string naziv)
        {
            DatumObavestenja = datumobavestenja;
            DatumDesavanja = datumdesavanja;
            Naziv = naziv;
            Notifikacije = true;
        }

        public Informacija(DateTime datumobavestenja, DateTime datumdesavanja, string naziv, bool notifikacije)
        {
            DatumObavestenja = datumobavestenja;
            DatumDesavanja = datumdesavanja;
            Naziv = naziv;
            Notifikacije = notifikacije;
        }

        public void Toggle()
        {
            if (Notifikacije)
                Notifikacije = false;
            else
                Notifikacije = true;
        }
    }
}

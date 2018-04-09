using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZodiacPlanner
{
    class LicenceCollection
    {

        Dictionary<string, Licence> licences; //pairs to licence

        const string DATA = "licenceinfo.txt";

        public LicenceCollection()
        {
            var data = File.ReadAllLines(DATA);
            licences = new Dictionary<string, Licence>();
            foreach (var line in data)
            {
                var bits = line.Split(',');
                char licenceType = char.Parse(bits[0]);
                string pair1 = bits[1];
                string pair2 = bits[2];
                string name = bits[3];
                int lpCost = int.Parse(bits[4]);
                var list = new List<string>();
                for (int i = 5; i < bits.Length; i++)
                    list.Add(bits[i]);
                licences.Add($"{pair1}{pair2}", new Licence(licenceType, pair1, pair2, lpCost, name, list.ToArray()));
            }
        }

        public IEnumerable<Licence> Search(string query)
        {
            if (query.Length == 0)
                return licences.Values.AsEnumerable();
            else
                return licences.Values.Where(o => o.Query(query));
        }

        public Licence GetLicence(string pairs)
        {
            return licences[pairs];
        }

    }
}

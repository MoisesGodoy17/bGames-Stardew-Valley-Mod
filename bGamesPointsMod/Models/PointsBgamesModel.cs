using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bGamesPointsMod.Models
{
    public class PointsBgamesModel
    {
        public string Id_attributes { get; set; }
        public string Name { get; set; }

        public string Data { get; set; }

        public PointsBgamesModel(string id_attributes, string name, string data)
        {
            Id_attributes = id_attributes;
            Name = name;
            Data = data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelUpModel
{
    public int Experience { get; set; }
    public string Description { get; set; }

    public LevelUpModel(int experience, string description)
    {
        Experience = experience;
        Description = description;
    }
}
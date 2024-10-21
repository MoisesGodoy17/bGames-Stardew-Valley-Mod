using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelUpModel
{
    public int Experience { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }

    public LevelUpModel(int experience, string description, int level)
    {
        Experience = experience;
        Description = description;
        Level = level;
    }
}
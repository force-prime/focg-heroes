using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IGameStatics
{
    IMainMapObjectDescription GetDescription(string descriptionId);
    IUnitDescription GetUnitDescription(string descriptionId);
}

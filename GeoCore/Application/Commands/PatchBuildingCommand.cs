using MediatR;
using System.Collections.Generic;

namespace GeoCore.Application.Commands
{
    public class PatchBuildingCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public Dictionary<string, object> Patch { get; set; }
        public PatchBuildingCommand(string code, Dictionary<string, object> patch)
        {
            Code = code;
            Patch = patch;
        }
    }
}
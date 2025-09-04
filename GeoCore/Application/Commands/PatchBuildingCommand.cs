using MediatR;
using GeoCore.DTOs;
using System.Collections.Generic;

namespace GeoCore.Application.Commands
{
    public class PatchBuildingCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public List<PatchOperation> Operations { get; set; }
        public PatchBuildingCommand(string code, List<PatchOperation> operations)
        {
            Code = code;
            Operations = operations;
        }
    }
}
using MediatR;

namespace GeoCore.Application.Commands
{
    public class PatchBuildingStatusCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public PatchBuildingStatusCommand(string code, string status)
        {
            Code = code;
            Status = status;
        }
    }
}
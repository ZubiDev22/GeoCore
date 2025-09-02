using MediatR;

namespace GeoCore.Application.Commands
{
    public class DeleteBuildingCommand : IRequest<bool>
    {
        public string Code { get; set; }
        public DeleteBuildingCommand(string code)
        {
            Code = code;
        }
    }
}
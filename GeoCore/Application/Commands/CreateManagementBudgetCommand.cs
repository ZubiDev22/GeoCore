using MediatR;
using GeoCore.DTOs;

namespace GeoCore.Application.Commands
{
    public class CreateManagementBudgetCommand : IRequest<ManagementBudgetDto>
    {
        public ManagementBudgetDto ManagementBudget { get; set; }
        public CreateManagementBudgetCommand(ManagementBudgetDto managementBudget)
        {
            ManagementBudget = managementBudget;
        }
    }
}

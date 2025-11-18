namespace MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands
{
    public class RemoveOrderDetailByIdCommand
    {
        public int Id { get; set; }

        public RemoveOrderDetailByIdCommand(int id)
        {
            Id = id;
        }
    }
}

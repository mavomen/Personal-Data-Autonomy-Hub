using MediatR;

namespace PDH.Application.Commands.UpdateActivityCategory;

public record UpdateActivityCategoryCommand(Guid ActivityId, string Category) : IRequest;

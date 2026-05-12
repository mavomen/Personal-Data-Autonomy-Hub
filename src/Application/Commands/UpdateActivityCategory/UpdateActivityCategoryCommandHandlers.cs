using MediatR;
using PDH.Shared.Infrastructure;
using PDH.Shared.Infrastructure.Persistence;
using PDH.Shared.Infrastructure.Repositories;
using PDH.Modules.Activities;

namespace PDH.Application.Commands.UpdateActivityCategory;

public class UpdateActivityCategoryCommandHandler : IRequestHandler<UpdateActivityCategoryCommand>
{
    private readonly IRepository<ActivityEvent> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateActivityCategoryCommandHandler(IRepository<ActivityEvent> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateActivityCategoryCommand request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetByIdAsync(request.ActivityId, cancellationToken);
        if (activity == null) throw new InvalidOperationException("Activity not found");

        if (Enum.TryParse<ActivityCategory>(request.Category, out var category))
        {
            activity.UpdateCategory(category);
            _repository.Update(activity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new InvalidOperationException($"Invalid category: {request.Category}");
        }
    }
}

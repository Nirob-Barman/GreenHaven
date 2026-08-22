using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(int Id) : IRequest<Unit>;

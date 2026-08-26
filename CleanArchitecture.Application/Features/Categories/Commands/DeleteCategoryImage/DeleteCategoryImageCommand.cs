using MediatR;

namespace CleanArchitecture.Application.Features.Categories.Commands.DeleteCategoryImage;

public sealed record DeleteCategoryImageCommand(int CategoryId) : IRequest<Unit>;

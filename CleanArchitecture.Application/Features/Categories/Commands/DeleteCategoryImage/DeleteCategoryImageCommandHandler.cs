using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Categories.Commands.DeleteCategoryImage;

public sealed class DeleteCategoryImageCommandHandler : IRequestHandler<DeleteCategoryImageCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IImageStorage _imageStorage;

    public DeleteCategoryImageCommandHandler(
        IApplicationDbContext context,
        IImageStorage imageStorage)
    {
        _context = context;
        _imageStorage = imageStorage;
    }

    public async Task<Unit> Handle(DeleteCategoryImageCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }

        var publicId = category.ImagePublicId;
        if (!string.IsNullOrEmpty(publicId))
        {
            await _imageStorage.DeleteAsync(publicId, cancellationToken);
        }

        category.RemoveImage();
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

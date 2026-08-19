namespace CleanArchitecture.Application.Common.Interfaces;

public interface IPaymentProcessor
{
    Task<PaymentProcessingResult> CreatePaymentAsync(
        PaymentProcessingRequest request,
        CancellationToken cancellationToken);
}

public sealed record PaymentProcessingRequest(
    int OrderId,
    decimal Amount,
    string Currency,
    string PaymentMethod);

public sealed record PaymentProcessingResult(
    string Status,
    string? ProviderPaymentId,
    string? ClientSecret);

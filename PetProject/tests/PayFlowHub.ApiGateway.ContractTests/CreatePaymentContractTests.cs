using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PayFlowHub.Contracts.Http;
using PayFlowHub.Contracts.Payments;
using Xunit;

namespace PayFlowHub.ApiGateway.ContractTests;

public sealed class CreatePaymentContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CreatePaymentContractTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreatePayment_ReturnsAccepted_ForValidRequest()
    {
        using var client = _factory.CreateClient();
        var request = BuildValidRequest("order-001");

        using var message = CreateMessage(request, "idem-001", "corr-001");
        using var response = await client.SendAsync(message);
        var payload = await response.Content.ReadFromJsonAsync<CreatePaymentResponse>();

        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("accepted", payload.Status);
        Assert.Equal("idem-001", payload.IdempotencyKey);
        Assert.Equal("corr-001", payload.CorrelationId);
    }

    [Fact]
    public async Task CreatePayment_ReturnsOk_WhenIdempotencyKeyAndPayloadAreRepeated()
    {
        using var client = _factory.CreateClient();
        var request = BuildValidRequest("order-repeat");

        using var first = CreateMessage(request, "idem-repeat", "corr-repeat");
        using var second = CreateMessage(request, "idem-repeat", "corr-repeat");

        using var firstResponse = await client.SendAsync(first);
        using var secondResponse = await client.SendAsync(second);

        var firstPayload = await firstResponse.Content.ReadFromJsonAsync<CreatePaymentResponse>();
        var secondPayload = await secondResponse.Content.ReadFromJsonAsync<CreatePaymentResponse>();

        Assert.Equal(HttpStatusCode.Accepted, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);
        Assert.NotNull(firstPayload);
        Assert.NotNull(secondPayload);
        Assert.Equal(firstPayload.PaymentId, secondPayload.PaymentId);
    }

    [Fact]
    public async Task CreatePayment_ReturnsConflict_WhenIdempotencyKeyReusedWithDifferentPayload()
    {
        using var client = _factory.CreateClient();
        using var first = CreateMessage(BuildValidRequest("order-a"), "idem-conflict", "corr-a");
        using var second = CreateMessage(BuildValidRequest("order-b"), "idem-conflict", "corr-b");

        using var firstResponse = await client.SendAsync(first);
        using var secondResponse = await client.SendAsync(second);

        Assert.Equal(HttpStatusCode.Accepted, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task CreatePayment_ReturnsBadRequest_WhenRequiredHeadersAreMissing()
    {
        using var client = _factory.CreateClient();
        using var message = new HttpRequestMessage(HttpMethod.Post, "/api/v1/payments")
        {
            Content = JsonContent.Create(BuildValidRequest("order-missing-header"))
        };

        using var response = await client.SendAsync(message);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePayment_ReturnsBadRequest_WhenAmountIsInvalid()
    {
        using var client = _factory.CreateClient();
        var invalidRequest = new CreatePaymentRequest("merchant-1", "order-invalid", 0, "USD", null);

        using var message = CreateMessage(invalidRequest, "idem-invalid", "corr-invalid");
        using var response = await client.SendAsync(message);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static CreatePaymentRequest BuildValidRequest(string orderId)
        => new("merchant-1", orderId, 1_250, "USD", "test payment");

    private static HttpRequestMessage CreateMessage(
        CreatePaymentRequest request,
        string idempotencyKey,
        string correlationId)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "/api/v1/payments")
        {
            Content = JsonContent.Create(request)
        };

        message.Headers.Add(PlatformHeaders.IdempotencyKey, idempotencyKey);
        message.Headers.Add(PlatformHeaders.CorrelationId, correlationId);
        return message;
    }
}

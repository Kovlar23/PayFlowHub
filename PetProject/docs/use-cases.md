# Use Cases

## Merchant Payment Authorization

1. Merchant sends a payment authorization request to the public API.
2. API Gateway validates request shape and operational headers.
3. Payment Orchestrator creates a payment aggregate and initial attempt.
4. Routing Engine selects the best provider for the attempt.
5. Provider Gateway executes the provider call.
6. Result is persisted, emitted as an event, and surfaced to operations.

## Capture Previously Authorized Payment

1. Merchant requests capture for an authorized payment.
2. Orchestrator verifies invariants for partial or full capture.
3. Provider Gateway executes capture against the PSP.
4. Lifecycle state and audit timeline are updated.

## Refund Processing

1. Operator or merchant requests refund creation.
2. Orchestrator validates refundable amount and current payment state.
3. Refund workflow coordinates provider execution and compensations.
4. Status becomes visible in API responses and operational timelines.

## Provider Degradation And Fallback

1. Provider timeout or failure signals are collected.
2. Routing Engine lowers provider health score.
3. Next attempts prefer healthy providers.
4. Operations UI can inspect why routing changed.

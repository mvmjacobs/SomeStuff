# Create Final `class-scheduling.md` Spec

## Summary
Replace the current partial prompt in [class-scheduling.md](/Users/jacobs/Projects/@mvmjacobs/SomeStuff/docs/specs/class-scheduling.md) with a complete feature specification that matches the implemented class-booking behavior in the repo. The rewritten spec should read as a source-of-truth API contract, not as an assignment.

## Key Changes
- Rewrite the document title/introduction to describe the implemented Class Scheduling API and its use-case-only architecture.
- Add a complete endpoint contract for `POST /api/classes/{classId}/book`:
  - request body with `userId`
  - success responses `201` and `200`
  - error responses `400`, `404`, `409`, and `429`
  - note that `429` includes `Retry-After`
- Add the concrete response payload definition:
  - `bookingId`
  - `classId`
  - `userId`
  - `timestamp`
  - `wasCreated`
- Add business rules:
  - default class capacity is 20
  - one booking per user per class
  - repeat requests are idempotent and return the existing booking
  - repository starts empty; class creation/listing is out of scope
- Add implementation details that matter to consumers and maintainers:
  - `ClassEntity` and `BookingEntity`
  - `IClassRepository` and `IBookingRepository`
  - in-memory implementations
  - per-class locking for race-safe booking
  - rate-limit key uses `userId` first, then IP fallback
- Add validation and concurrency guarantees:
  - invalid `classId` or `userId` returns `400`
  - missing class returns `404`
  - full class returns `409`
  - exactly one request succeeds when many race for the last seat

## Test Coverage to Capture
- booking succeeds when capacity is available
- duplicate booking returns existing booking without consuming another seat
- unknown class returns `404`
- full class returns `409`
- invalid identifiers return `400`
- 50 concurrent requests for the last seat produce exactly 1 success
- 6th request inside one minute for the same effective rate-limit key returns `429`
- different users on the same IP are rate-limited independently when `userId` is present

## Assumptions
- The updated spec should describe the implementation as it exists today and should not introduce new features.
- The spec should stay compact and structured, with sections such as Overview, Data Model, API Contract, Business Rules, Concurrency/Rate Limiting, and Validation/Test Scenarios.
- No additional endpoints, persistence layer, or class-management workflow should be documented beyond a brief out-of-scope note.

# Feature Specification: Class Scheduling API

## 1. Context and Objective
We are adding a new "Class Scheduling" (booking) feature to an existing API. Your task is to implement the necessary domain logic, repository interfaces, controllers, and middleware to support this feature.

Since the application currently lacks a database layer, you must design a simple Repository Interface and provide an in-memory implementation for it.

## 2. Core Requirements
- **Feature:** Users can book a spot in a specific class.
- **Capacity Constraint:** Each class has a strict maximum limit of **20 students**.
- **Concurrency (Race Conditions):** The booking logic must be bulletproof against race conditions. If 50 simultaneous requests attempt to book the final remaining spot, exactly 1 must succeed, and 49 must be rejected gracefully.
- **Rate Limiting:** Implement a rate limit on the booking endpoint to prevent abuse and API spam (e.g., maximum 5 booking attempts per user/IP per minute).

## 3. Data Models
Please implement the following basic entities (adjust syntax to the project's programming language):

- **ClassEntity:**
  - `id` (String/UUID)
  - `title` (String)
  - `capacity` (Integer, default 20)
  - `enrolledCount` (Integer)

- **BookingEntity:**
  - `id` (String/UUID)
  - `classId` (String/UUID)
  - `userId` (String/UUID)
  - `timestamp` (DateTime)

## 4. Repository Interface (Data Layer)
Since there is no DB, abstract the data access. Create the following:

1. **`IClassRepository` Interface:**
   - `findById(classId)` -> Returns ClassEntity
   - `incrementEnrollment(classId, maxCapacity)` -> Atomically increments `enrolledCount` if it is strictly less than `maxCapacity`. Must return a boolean or throw an error if capacity is reached.

2. **`InMemoryClassRepository` (Implementation):**
   - Implement the interface using an in-memory data structure (e.g., a dictionary/Map).
   - **Crucial:** Use appropriate thread-safe structures or Mutex/Locks within this in-memory implementation to simulate database-level atomic transactions and prevent race conditions.

## 5. Rate Limiting Middleware
- Implement a middleware or interceptor to limit requests to the booking endpoint.
- **Rules:** 5 requests per minute per IP or User ID.
- **Response:** Return HTTP 429 (Too Many Requests) when the limit is exceeded.

## 6. API Endpoint Specification
**Endpoint:** `POST /api/classes/{classId}/book`

**Request Body:**
```json
{
  "userId": "uuid-of-the-user"
}

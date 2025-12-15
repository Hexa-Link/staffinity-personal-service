# Allowed AI Intents per Role (Staffinity)

## Goal

Ensure AI usage is safe, controlled, and aligned with HR rules. The AI module must not be accessible to non-authorized roles.

## Roles

- Employee: ❌ Not allowed to use AI
- HR: ✅ Allowed
- Admin: ✅ Allowed

## Allowed Intents Catalog

The system supports a closed set of intents (AiIntent). Intents must be validated before any context building or model invocation.

### Employee (❌)

No intents allowed.

### HR (✅)

Allowed intents:

- HrKpiSummary
- EmployeeHeadcountSnapshot
- VacationRequestsOverview
- TurnoverRiskSignals
- WorkforceAnomalies
- VacationPolicyCompliance
- ActionRecommendations

### Admin (✅)

Allowed intents:

- Same as HR (full access to the current catalog)

## Enforcement Notes

- Validation must occur BEFORE:
  - building context
  - calling external AI providers
- If a role is not allowed for an intent:
  - throw UnauthorizedAiIntentException
- If an intent would require restricted data access:
  - throw ForbiddenAiDataAccessException

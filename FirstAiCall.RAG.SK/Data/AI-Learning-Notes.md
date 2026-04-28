# Contoso Helpdesk Knowledge Base

This sample document is fictional data for demonstrating Retrieval-Augmented Generation.
It does not contain personal plans, private notes, customer records, credentials, or real business information.

## Support Workflow

Contoso Helpdesk uses a three-step workflow for incoming support tickets:

1. Classify the issue by product area.
2. Retrieve the most relevant troubleshooting article.
3. Provide a short answer with a recommended next action.

Ticket categories include account access, billing, device setup, application errors, and feature requests.

## Account Access

Users who cannot sign in should first confirm that their email address is correct.
If the password was recently changed, they should wait two minutes before retrying.
If multi-factor authentication fails, the support team should verify the user's registered device and reset the MFA challenge only after identity verification.

## Billing Questions

Billing questions usually involve invoice copies, plan upgrades, payment failures, or renewal dates.
The assistant should never invent invoice numbers or payment status.
If billing data is not present in the retrieved context, the assistant should say that the billing system must be checked.

## Device Setup

New devices require the Contoso Companion app and a verified user account.
The setup flow is to install the companion app, sign in, pair the device using the six-digit code, and confirm the device appears on the dashboard.

If pairing fails, users should restart the device, check network connectivity, and generate a new pairing code.

## Application Errors

For application crashes, collect the app version, operating system, error message, and last action before the crash.
The assistant should recommend updating to the latest version before deeper troubleshooting.

Known error examples:

- `SYNC_TIMEOUT`: The app could not reach the sync service.
- `AUTH_EXPIRED`: The session token expired and the user must sign in again.
- `DEVICE_NOT_FOUND`: The paired device is offline or removed from the account.

## Escalation Rules

Escalate the ticket when the user reports possible account compromise, the same payment fails more than twice, a device cannot pair after three attempts, the app crashes repeatedly after reinstalling, or the retrieved context does not contain enough information to answer confidently.

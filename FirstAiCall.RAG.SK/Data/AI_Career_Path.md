# Contoso Product FAQ

This fictional FAQ provides neutral sample content for the local RAG proof of concept.
It is designed to exercise retrieval across product features, support rules, and answer grounding.

## Product Overview

Contoso Hub is a sample productivity dashboard for small teams.
It combines task tracking, device monitoring, and lightweight reporting in one workspace.
The product is fictional and exists only as demo content.

## Core Features

Contoso Hub includes team task boards, device status monitoring, weekly activity summaries, basic role-based access, notification preferences, and exportable reports.

## Roles And Permissions

Contoso Hub has three roles:

- Viewer: Can read dashboards and reports.
- Editor: Can update tasks and device labels.
- Admin: Can manage users, roles, integrations, and billing settings.

Admins should review role changes every quarter.
Editors cannot invite new users.
Viewers cannot export billing reports.

## Reporting

Weekly summaries include completed tasks, open tasks, device uptime, and high-priority alerts.
Reports can be exported as CSV files.
The sample product does not currently support PDF exports.

## Notifications

Users can configure email notifications for task assignment, device alerts, and weekly summaries.
Device alerts are sent immediately.
Weekly summaries are sent every Monday morning.
Task assignment notifications are sent when a task is created or reassigned.

## Integrations

The sample product supports webhook integrations for alerts.
Each workspace can configure up to five webhook endpoints.
Webhook retries happen three times with exponential backoff.

The product does not include native integrations for CRM, ERP, or chat platforms in this sample documentation.

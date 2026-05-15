// ---
// skill: the-standard-csharp-comments
// type: example
// source-section: "1. Comments and Documentation — 12.2 Methods"
// demonstrates: "tsc-csharp-comments-003 — correct method documentation for complex/inaccessible methods"
// ---

// ✅ GOOD: Method documentation with Purposing, Incomes, Outcomes, Side Effects.

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

public class EmailBroker : IEmailBroker
{
    /// <summary>
    /// Sends an email message via the configured SMTP provider.
    /// </summary>
    /// <remarks>
    /// Purposing:  Delivers a transactional email to one or more recipients.
    /// Incomes:    recipients — list of destination email addresses.
    ///             subject    — the subject line of the email.
    ///             content    — the HTML or plain-text body of the email.
    /// Outcomes:   Email is queued and delivered by the SMTP provider.
    /// Side Effects: Triggers SMTP network I/O; subject to rate limiting by the provider.
    /// </remarks>
    public async ValueTask SendMailAsync(
        List<string> recipients,
        string subject,
        string content)
    {
        Message message = BuildMessage(recipients, subject, content);
        await SendEmailMessageAsync(message);
    }
}

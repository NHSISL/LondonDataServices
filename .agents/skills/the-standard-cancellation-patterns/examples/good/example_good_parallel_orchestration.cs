// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "8.0 Parallel Orchestration"
// demonstrates: "tsc-csharp-cp-014 — same token passed to all parallel tasks"
// ---

// ✅ The same CancellationToken is passed to every parallel task.
//    Task.WhenAll receives all tasks, each holding the same token.

public partial class PatientOrchestrationService : IPatientOrchestrationService
{
    public async ValueTask<PatientSummary> RetrievePatientSummaryAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        Task<Patient> patientTask =
            this.patientService.RetrievePatientByIdAsync(
                patientId,
                cancellationToken)
            .AsTask();

        Task<Address> addressTask =
            this.addressService.RetrieveAddressByPatientIdAsync(
                patientId,
                cancellationToken)
            .AsTask();

        Task<Insurance> insuranceTask =
            this.insuranceService.RetrieveInsuranceByPatientIdAsync(
                patientId,
                cancellationToken)
            .AsTask();

        await Task.WhenAll(
            patientTask,
            addressTask,
            insuranceTask);

        return new PatientSummary
        {
            Patient = patientTask.Result,
            Address = addressTask.Result,
            Insurance = insuranceTask.Result
        };
    }
}

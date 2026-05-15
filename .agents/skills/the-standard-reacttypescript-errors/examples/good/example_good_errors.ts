// ---
// skill: the-standard-reacttypescript-errors
// type: example
// source-section: "13. Error Handling"
// demonstrates: "tsr-errors-001, tsr-errors-002, tsr-errors-004, tsr-errors-005"
// ---

// ✅ Broker throws — no UI message conversion (tsr-errors-001)
export class PatientApiBroker implements IPatientApiBroker {
    public async getPatientAsync(patientId: string): Promise<Patient> {
        const response = await fetch(`/api/patients/${patientId}`);

        if (!response.ok) {
            throw new Error(
                `GET /api/patients/${patientId} failed with status ${response.status}.`);
        }

        return response.json() as Promise<Patient>;
    }
}

// ✅ Service localizes broker exception as domain exception (tsr-errors-002)
export class PatientService implements IPatientService {
    public async retrievePatientAsync(patientId: string): Promise<Patient> {
        validatePatientId(patientId);

        try {
            return await this.patientApiBroker.getPatientAsync(patientId);
        } catch (error: unknown) {
            throw new PatientDependencyException(
                "A dependency error occurred while retrieving the patient.",
                error);
        }
    }
}

// ✅ Page renders error state explicitly — no silent empty render (tsr-errors-004)
export default function PatientProfilePage() {
    const { patient, isLoading, error } = usePatientProfilePage();

    if (isLoading) return <LoadingIndicator />;
    if (error) return <ErrorSummary error={error} />;  // ✅ structured display
    if (!patient) return <EmptyState message="Patient not found." />;

    return <PatientProfileComponent patientProfile={patient} />;
}

// ✅ Component receives structured prop — never inspects raw error (tsr-errors-005)
export function ErrorSummary(props: { error: unknown }) {
    return <div className="alert alert-danger">An error occurred.</div>;
}

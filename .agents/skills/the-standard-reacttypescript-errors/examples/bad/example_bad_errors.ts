// ---
// skill: the-standard-reacttypescript-errors
// type: example
// source-section: "13. Error Handling"
// demonstrates: "tsr-errors-001, tsr-errors-005, tsr-errors-006"
// ---

// ❌ BAD: Multiple error handling violations.

// ❌ Broker converts error to UI string — violates tsr-errors-001
export class PatientApiBroker {
    public async getPatientAsync(patientId: string): Promise<Patient | string> {
        try {
            const response = await fetch(`/api/patients/${patientId}`);
            return response.json();
        } catch {
            return "Error loading patient"; // ❌ caller cannot distinguish from valid data
        }
    }
}

// ❌ Silent swallow — violates tsr-errors-006
export class PatientService {
    public async retrievePatientAsync(patientId: string): Promise<Patient | null> {
        try {
            return await this.broker.getPatientAsync(patientId) as Patient;
        } catch {
            return null; // ❌ error disappears — no log, no re-throw
        }
    }
}

// ❌ Component inspects raw infrastructure error — violates tsr-errors-005
export function PatientCard(props: { error: any }) {
    if (props.error?.status === 404) {
        return <p>Patient not found</p>; // ❌ HTTP status in component
    }
    return null;
}

// ---
// skill: the-standard-reacttypescript-brokers
// type: example
// source-section: "4. Brokers"
// demonstrates: "tsr-brokers-002, tsr-brokers-003, tsr-brokers-005"
// ---

// ❌ BAD: Business rule in broker, component shaping, no interface.

// ❌ No interface — violates tsr-brokers-005
export class PatientApiBroker {

    public async getPatientStatusAsync(
        patientId: string)
        : Promise<string> {

        const response = await fetch(`/api/patients/${patientId}`);
        const patient = await response.json();

        // ❌ Business rule inside broker — violates tsr-brokers-002
        return patient.age >= 18 ? "Adult" : "Child";
    }

    public async getPatientCardDataAsync(
        patientId: string)
        : Promise<{ displayName: string; badge: string }> {

        const response = await fetch(`/api/patients/${patientId}`);
        const patient = await response.json();

        // ❌ Component shaping inside broker — violates tsr-brokers-003
        return {
            displayName: `${patient.firstName} ${patient.lastName}`,
            badge: patient.isActive ? "Active" : "Inactive"
        };
    }
}

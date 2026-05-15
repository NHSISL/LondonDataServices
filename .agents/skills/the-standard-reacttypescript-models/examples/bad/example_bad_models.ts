// ---
// skill: the-standard-reacttypescript-models
// type: example
// source-section: "3. Models"
// demonstrates: "tsr-models-004, tsr-models-005"
// ---

// ❌ BAD: Behavior in model — violates tsr-models-005
export type Patient = {
    id: string;
    firstName: string;
    lastName: string;
    age: number;
    getDisplayName(): string; // ❌ method in model
};

// ❌ BAD: Raw API model used directly as component prop — violates tsr-models-004
// Component receives the foundation Patient model with raw fields.
// The component is now forced to format dateOfBirth and compute age group.
export type PatientCardProps = {
    patient: Patient; // ❌ should be PatientCardView with display-ready fields
};

// ❌ BAD: Foundation model returned directly from view service as the view model.
// View service skips transformation — no display-ready fields produced.
// violates tsr-models-002
export async function retrievePatientProfileViewAsync(
    patientId: string): Promise<Patient> { // ❌ should return PatientProfileView

    return await patientApiBroker.getPatientAsync(patientId);
}

// ---
// skill: the-standard-reacttypescript-models
// type: example
// source-section: "3. Models"
// demonstrates: "tsr-models-001, tsr-models-002, tsr-models-003, tsr-models-005"
// ---

// ✅ GOOD: Foundation model — domain/API fields only, no behavior.
export type Patient = {
    id: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    age: number;
};

// ✅ GOOD: View model — display-ready fields produced by view service.
export type PatientProfileView = {
    id: string;
    displayName: string;
    ageGroupDisplayText: string;
};

// ✅ GOOD: Prop model — rendering input for one component only.
export type PatientCardProps = {
    patientProfile: PatientProfileView;
};

// ✅ GOOD: View service produces the view model from the foundation model.
// (view service code — not a model file)
export function mapToPatientProfileView(patient: Patient): PatientProfileView {
    return {
        id: patient.id,
        displayName: `${patient.firstName} ${patient.lastName}`,
        ageGroupDisplayText: patient.age >= 18 ? "Adult" : "Child"
    };
}

// ---
// skill: the-standard-reacttypescript-typescript
// type: example
// source-section: "2. TypeScript Rules"
// demonstrates: "tsr-typescript-002, tsr-typescript-003, tsr-typescript-004"
// ---

// ❌ BAD: Multiple TypeScript violations.

// ❌ interface for data shape — violates tsr-typescript-003
interface Patient {
    id: string;
    name: string;
}

// ❌ any in parameter and return type — violates tsr-typescript-004
export async function retrievePatientAsync(id: any): Promise<any> {
    return fetch(`/api/patients/${id}`).then(r => r.json());
}

// ❌ Missing return type at boundary — violates tsr-typescript-002
export async function getPatient(patientId: string) {
    return fetch(`/api/patients/${patientId}`);
}

// ❌ type for behavioral contract — violates tsr-typescript-003
type IPatientService = {
    retrievePatientAsync(id: string): Promise<Patient>;
};
